using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telemetry;


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
	//private RaycastHit targetHit;
	[SerializeField] Transform hitTransform;
	[SerializeField] private string majorRuneTag;
	[SerializeField] private string magicableTag;

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
			targetMajorRune = GetRuneFromTarget(targetObject, hitTransform);		
		}

		//Getting the minorRune prefab to instantiate it
		GameObject minorRunePrefab = globalBlackboard.GetMinorRune(runeName);

		if(minorRunePrefab == null)
		{
			Debug.Log("No rune prefab");
			return;
		}

		MinorRune currentRuneScriptPrefab = minorRunePrefab.GetComponent<MinorRune>();


		//Check special cases: destroy and complement runes, that requires two shapes to be created
		if (currentRuneScriptPrefab.GetRuneType() == MinorRune.RuneTypes.destroy)
		{
			//An information rute, doesn't need for creating or destroying any rune
			penultimateRune = lastRune;
			lastRune = currentRuneScriptPrefab;
			return;
		}
		else if(currentRuneScriptPrefab.GetRuneClassification() == MinorRune.RuneClassifications.complement){
			penultimateRune = lastRune;
			lastRune = currentRuneScriptPrefab;
			lastRuneName = runeName;
			return;
		}

		//

		if (lastRune != null && lastRune.GetRuneType() == MinorRune.RuneTypes.destroy)
		{
			//Destroy a rune that is the same os the currentRune type
			targetMajorRune.DestroyMinorRune(currentRuneScriptPrefab.GetRuneType());			
			lastRune = null;
			lastRuneName = null;
			penultimateRune = null;
		}
		else if (lastRune != null && lastRune.GetRuneClassification() == MinorRune.RuneClassifications.complement)
		{
			if (penultimateRune != null && penultimateRune.GetRuneType() == MinorRune.RuneTypes.destroy)
			{
				//Destroy a complement rune
				targetMajorRune.DestroyComplementRune(lastRune.GetRuneType(), currentRuneScriptPrefab.GetRuneClassification());
			}
			else
			{
				//Instantiate the last drawn complement rune attached to a rune of the same type of the currentRune
				//But first let's check that the powered rune is a correct classifaction
				if (currentRuneScriptPrefab.GetRuneClassification() == MinorRune.RuneClassifications.source
					|| currentRuneScriptPrefab.GetRuneClassification() == MinorRune.RuneClassifications.transformation)
				{
					GameObject complementRunePrefab = globalBlackboard.GetMinorRune(lastRuneName);
					GameObject complementRune = Instantiate(complementRunePrefab);
					complementRune.layer = globalBlackboard.targetLayer;

					ComplementMinorRune complementScript = complementRune.GetComponent<ComplementMinorRune>();

					complementScript.SetTargetClassification(currentRuneScriptPrefab.GetRuneClassification());
					targetMajorRune.AddMinorRune(complementRune.transform);
					//Debug.Log("complement rune: " + lastRuneName + " attached to: " + runeName);

					SendEvent(
						DataManager.Actions.runeCreation,
						runeName,
						currentRuneScriptPrefab.GetRuneClassification(),
						currentRuneScriptPrefab.GetRuneType(),
						targetMajorRune
						);


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

			SendEvent(
						DataManager.Actions.runeCreation,
						runeName,
						currentRuneScriptPrefab.GetRuneClassification(),
						currentRuneScriptPrefab.GetRuneType(),
						targetMajorRune
						);

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
	private MajorRune GetRuneFromTarget(Transform _newTarget, Transform _hitTransform)
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
			GameObject newRune = Instantiate(majorRunePrefab, _hitTransform.position, _hitTransform.rotation);
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

		Debug.Log(_newTarget);
		if (_newTarget.tag != magicableTag)
			return;
		
		
		PhysicObject oldTarget = null;


		if (targetObject != null)
		{
			oldTarget = targetObject.transform.GetComponent<PhysicObject>();
			if (oldTarget != null)
			{
				//Debug.Log("desactivate highlight");
				oldTarget.SetActiveHightlight(false);
			}
		}

		if(hitTransform == null)
		{
			hitTransform = new GameObject().transform;
		}


		targetObject = _newTarget;
		//targetHit = hit;
		targetMajorRune = null;
		hitTransform.position = hit.point;
		hitTransform.rotation = Quaternion.LookRotation(hit.normal);
		hitTransform.parent = targetObject;

		if (targetObject != null)
		{
			oldTarget = targetObject.transform.GetComponent<PhysicObject>();

			if (oldTarget != null)
			{
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

	private void SendEvent(
		DataManager.Actions _action,
		string _result,
		MinorRune.RuneClassifications _classification,
		MinorRune.RuneTypes _type,
		MajorRune _majorRune
		)
	{
		string info =
			"classification: " + _classification.ToString("g") + ", " +
			"type: " + _type.ToString("g") + ", " +
			"majorRuneName: " + _majorRune.name;

		DataManager.dataManager.AddAction(
			DataManager.Actors.game,
			DataManager.Actions.runeCreation,
			_result,
			Time.time,
			Time.time,
			info
			);
	}


}
