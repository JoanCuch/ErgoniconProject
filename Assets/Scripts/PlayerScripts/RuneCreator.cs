using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class duties;
///  - Make the gesture recognition of the minorRunes
///  - Instantiate minor runes and add to his attached major runes
///  - Instantiate major runes and add to his attached physical object
/// </summary>
public class RuneCreator : MonoBehaviour
{
	
	//OtherScripts
	private GameManager gameManager;
	private GlobalBlackboard globalBlackboard;

	//Gameobjects prefabs
	public GameObject majorRunePrefab;

	//Other
	private Transform targetObject;
	private MajorRune targetMajorRune;
	private RaycastHit targetHit;
	[SerializeField] [TagSelector] private string majorRuneTag;

	private MinorRune lastRune;
	private string lastRuneName;

	private MinorRune penultimateRune;


	// Start is called before the first frame update
	void Start()
    {
		gameManager = GameManager.gameManager;
		globalBlackboard = gameManager.globalBlackboard;

		targetObject = null;
		targetMajorRune = null;
		lastRune = null;
		lastRuneName = null;

	}

    // Update is called once per frame
    void Update()
    {
		if(gameManager == null)
		{
			gameManager = GameManager.gameManager;
		}		
	}

	/// <summary>
	/// Instantiates a minor rune using his name
	/// </summary>
	public void CreateMinorRune(string runeName)
	{

		//If there is no targetMajorRune, get one
		if (targetMajorRune == null)
		{
			
			if(targetObject == null)
			{
				Debug.Log("No target selected");
				return;
			}
			targetMajorRune = GetRuneFromTarget(targetObject, targetHit);		
		}

		//Getting the minorRune prefab to instantiate it
		GameObject minorRunePrefab = globalBlackboard.GetMinorRune(runeName);

		if(minorRunePrefab == null)
		{
			Debug.Log("No rune prefab");
			return;
		}

		MinorRune currentRune = minorRunePrefab.GetComponent<MinorRune>();


		//Check special cases: destroy and complement runes, that requires two shapes to be created
		if (currentRune.GetRuneType() == MinorRune.RuneTypes.destroy)
		{
			//An information rute, doesn't need for creating or destroying any rune
			penultimateRune = lastRune;
			lastRune = currentRune;
			return;
		}
		else if(currentRune.GetRuneClassification() == MinorRune.RuneClassifications.complement){
			penultimateRune = lastRune;
			lastRune = currentRune;
			lastRuneName = runeName;
			return;
		}

		//

		if (lastRune != null && lastRune.GetRuneType() == MinorRune.RuneTypes.destroy)
		{
			//Destroy a rune that is the same os the currentRune type
			targetMajorRune.DestroyMinorRune(currentRune.GetRuneType());			
			lastRune = null;
			lastRuneName = null;
			penultimateRune = null;
		}
		else if (lastRune != null && lastRune.GetRuneClassification() == MinorRune.RuneClassifications.complement)
		{
			if (penultimateRune != null && penultimateRune.GetRuneType() == MinorRune.RuneTypes.destroy)
			{
				//Destroy a complement rune
				targetMajorRune.DestroyComplementRune(lastRune.GetRuneType(), currentRune.GetRuneClassification());
			}
			else
			{
				//Instantiate the last drawn complement rune attached to a rune of the same type of the currentRune
				//But first let's check that the powered rune is a correct classifaction
				if (currentRune.GetRuneClassification() == MinorRune.RuneClassifications.source
					|| currentRune.GetRuneClassification() == MinorRune.RuneClassifications.transformation)
				{
					GameObject complementRunePrefab = globalBlackboard.GetMinorRune(lastRuneName);
					GameObject complementRune = Instantiate(complementRunePrefab);
					complementRune.layer = globalBlackboard.targetLayer;

					ComplementMinorRune complementScript = complementRune.GetComponent<ComplementMinorRune>();

					complementScript.SetTargetClassification(currentRune.GetRuneClassification());
					targetMajorRune.AddMinorRune(complementRune.transform);
					//Debug.Log("complement rune: " + lastRuneName + " attached to: " + runeName);
				}
			}
			lastRune = null;
			lastRuneName = null;
			penultimateRune = null;

		}
		else
		{		
			GameObject newMinorRune = Instantiate(minorRunePrefab);
			newMinorRune.layer = globalBlackboard.targetLayer;
			targetMajorRune.AddMinorRune(newMinorRune.transform);
			lastRune = null;
			lastRuneName = null;
			penultimateRune = null;

			Debug.Log("creating minor rune type of: " + runeName);
		}
	
	}

	/// <summary>
	/// Returns the MajorRune script of a gameObject. If it doesn't have one, it creates a new one.
	/// The gameobject can be a scene prop or a major rune. 
	/// The hit is the position and direction where the major rune will appear.
	/// </summary>
	private MajorRune GetRuneFromTarget(Transform _newTarget, RaycastHit hit)
	{
		MajorRune majorRuneToReturn = null;

		MajorRune runeScript = _newTarget.GetComponent<MajorRune>();
		PhysicObject objectScript = _newTarget.GetComponent<PhysicObject>();

		if (runeScript != null)
		{
			//If the target is a rune then get it.
			majorRuneToReturn = runeScript;
		}
		else if (objectScript != null)
		{
			//If the target is an object, then add a major rune to it.
			GameObject newRune = Instantiate(majorRunePrefab, hit.point, Quaternion.LookRotation(hit.normal));
			newRune.layer = globalBlackboard.targetLayer;////////
			objectScript.AttachMajorRune(newRune.transform);
			majorRuneToReturn = newRune.GetComponent<MajorRune>();		
		}
		else
		{
			Debug.LogWarning("The selected object is not magicable");
		}

		if (majorRuneToReturn == null)
			Debug.LogWarning("There is no rune to attach");

		return majorRuneToReturn;		
	}

	public void SetTarget(Transform _newTarget, RaycastHit hit)
	{
		PhysicObject oldTarget = null;


		if (targetObject != null)
		{
			oldTarget = targetObject.transform.GetComponent<PhysicObject>();
			if (oldTarget != null)
			{
				Debug.Log("desactivate highlight");
				oldTarget.SetActiveHightlight(false);
			}
		}

		targetObject = _newTarget;
		targetHit = hit;
		targetMajorRune = null;

		if (targetObject != null)
		{
			oldTarget = targetObject.transform.GetComponent<PhysicObject>();

			if (oldTarget != null)
			{
				Debug.Log("activate highlight");
				oldTarget.SetActiveHightlight(true);
			}
		}
	}

	public MajorRune GetTargetRune()
	{
		return targetMajorRune;
	}

	public void ChangeTargetLayer(int _newLayer)
	{
		

		if (targetObject != null)
		{
			Transform changingTarget = (targetObject.tag == majorRuneTag) ? targetObject.parent : targetObject;
			changingTarget.gameObject.layer = _newLayer;
			
			foreach(Transform child in changingTarget.transform)
			{
				child.gameObject.layer = _newLayer;

				foreach(Transform baby in child.transform)
				{
					baby.gameObject.layer = _newLayer;
				}

			}
		}
	}


}
