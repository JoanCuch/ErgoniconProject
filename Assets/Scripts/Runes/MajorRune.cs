using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MajorRune : MonoBehaviour
{
	//[SerializeField] [ReadOnly] private MinorRune sourceRune;
	//[SerializeField] [ReadOnly] private MinorRune transformationRune;
	//[SerializeField] [ReadOnly] private MinorRune basicRune;

	//[SerializeField] [ReadOnly] private List<MinorRune> complementRunes;

	[SerializeField] [ReadOnly] private EnergyInteractable attachedObject;

	private List<RuneSorted> runesList = new List<RuneSorted>();

	[SerializeField] private Transform centerPoint;
	[SerializeField] private Transform rightPoint;
	[SerializeField] private float runeInterspace;


	[SerializeField] private Transform startPos;
	/*public enum RuneFunction
	{
		source,
		sourceExtra,
		inverse,
		basic,
		transformationExtra,
		transformation,
		twin
	}

	[SerializeField] private List<RuneFunction> runesPriority;*/


	//The child objects of the rune. Perfect to get them in any moment
	[SerializeField] private Transform source; //Here can only be one child
	[SerializeField] private Transform transf; //Here can only be one child
	[SerializeField] private Transform complement; //
	[SerializeField] private Transform basic; //Here can only be one child


	// Start is called before the first frame update
	void Start()
	{
		attachedObject = transform.parent.GetComponent<EnergyInteractable>();
	}

	// Update is called once per frame
	void Update()
	{
		if(attachedObject == null)
		{
			attachedObject = transform.parent.GetComponent<EnergyInteractable>();
		}
	}

	/// <summary>
	/// Gets a minorRune gameobject and asigns it
	/// </summary>
	public void AddMinorRune(Transform minorRune)
	{
		
		RuneSorted newRune = new RuneSorted();

		MinorRune minorRuneScript = minorRune.GetComponent<MinorRune>();
		newRune.runeScript = minorRuneScript;

		MinorRune.RuneClassifications runeClassification = minorRuneScript.GetRuneClassification();
		MinorRune.RuneTypes runeType = minorRuneScript.GetRuneType();

		Transform runeParent = null;

		//Transform runeParent = null;
		/*
		source, 0
		sourceExtra, 1
		inverse, 2
		basic, 3
		transformationExtra, 4
		transformation, 5
		twin 6
		 */

		switch (runeClassification)
		{
			case MinorRune.RuneClassifications.source:
				runeParent = source;
				newRune.priority = 0;
				break;

			case MinorRune.RuneClassifications.transformation:
				runeParent = transf;
				newRune.priority = 5;
				break;
			case MinorRune.RuneClassifications.complement:
				runeParent = complement;
				newRune.priority = 1;
				break;

			case MinorRune.RuneClassifications.basic:
				runeParent = basic;
				newRune.priority = 3;
				break;

			default:
				Debug.LogWarning("minor rune type not detected: " + runeClassification);
				break;
		}
		switch (runeType)
		{
			case MinorRune.RuneTypes.basic:
				break;
			case MinorRune.RuneTypes.inverse:
				newRune.priority = 2;
				break;
			case MinorRune.RuneTypes.physicalObject:
				break;
			case MinorRune.RuneTypes.ambient:
				break;
			case MinorRune.RuneTypes.direct:
				break;
			case MinorRune.RuneTypes.extra:
				break;
			case MinorRune.RuneTypes.twin:
				newRune.priority = 6;
				break;
			case MinorRune.RuneTypes.heat:
				break;
			case MinorRune.RuneTypes.force:
				break;
			default:
				break;
		}


		//Adding the rune to the list and sorting it
		if (runesList == null)
		{
			Debug.LogWarning("instantiating the list again");
			runesList = new List<RuneSorted>();
		}
		
		//Checking that can only be one source, transf and basic
		if (runeClassification != MinorRune.RuneClassifications.complement ||
			runeType == MinorRune.RuneTypes.inverse ||
			runeType == MinorRune.RuneTypes.twin)
		{
			foreach (Transform child in runeParent)
			{
				foreach (RuneSorted sort in runesList)
				{
					if (sort.runeScript.gameObject == child.gameObject)
					{
						runesList.Remove(sort);
						break;
					}
				}
				Destroy(child.gameObject);
			}
		}
		minorRune.parent = runeParent;


		

		runesList.Add(newRune);
		runesList.Sort((x, y) => x.priority.CompareTo(y.priority));

		//Givin the position to all the elements on the list.

		Vector3 direction = -(rightPoint.position - centerPoint.position).normalized;

		float length = 0;
		foreach(RuneSorted sorted in runesList)
		{
			length += sorted.runeScript.GetSpriteWidth();
			length += runeInterspace;
		}
		length -= runeInterspace;


		Vector3 startPosition = centerPoint.position - direction * (length / 2);
		startPos.position = startPosition;

		for (int i = 0; i < runesList.Count; i++)
		{
			MinorRune rune = runesList[i].runeScript;
			float runeWidth = rune.GetSpriteWidth();

			rune.transform.rotation = this.transform.rotation;

			rune.transform.position = startPosition + direction * (runeWidth/2);
			startPosition = startPosition + direction * ((runeWidth) + runeInterspace);
			startPos.position = startPosition;
		}

		//minorRune.position = runeParent.position;
		//minorRune.rotation = runeParent.rotation;
		minorRuneScript.SetMajorRune(this);

		string temp = "";
		foreach(RuneSorted sort in runesList)
		{
			temp += sort.runeScript.name + " ";
		}

		Debug.Log("runesList: "+ temp);

	}

	

	public MinorRune GetAttachedRuneOfType(MinorRune.RuneClassifications runeType)
	{
		MinorRune runeToReturn = null;

		/*
		switch (runeType)
		{
			case MinorRune.RuneTypes.source:
				runeToReturn = sourceRune;
				break;

			case MinorRune.RuneTypes.transformation:
				runeToReturn = transformationRune;
				break;

			case MinorRune.RuneTypes.complement:
				runeToReturn = GetComplementRune();
				break;

			case MinorRune.RuneTypes.basic:
				runeToReturn = basicRune;
				break;

			default:
				Debug.LogWarning("Error with selecting a rune type");
				break;
		}*/
		foreach(RuneSorted rune in runesList)
		{
			if(rune.runeScript.GetRuneClassification()== runeType)
			{
				runeToReturn = rune.runeScript;
			}
		}

		return runeToReturn;
	}

	public EnergyInteractable GetAttachedObject()
	{
		return attachedObject;
	}






	/*private void AddComplementRune(MinorRune minorRuneScript)
	{
		bool AlreadyExists = false;

		foreach (MinorRune rune in complementRunes)
		{
			if(rune.GetType() == typeof(InverseMinorRune) ||
				rune.GetType() == typeof(TwinMinorRune))
			{
				AlreadyExists = true;
			}
		}

		if(AlreadyExists== false)
		{
			complementRunes.Add(minorRuneScript);
		}
	}*/

	/*private MinorRune GetComplementRune()
	{
		MinorRune inverseRune = null;
		
		foreach(MinorRune rune in complementRunes)
		{
			if(rune.GetType() == typeof(InverseMinorRune))
			{
				inverseRune = rune;
			}
		}
		return inverseRune;
	}*/

	/*public bool CheckComplementRuneIsTypeOf(RunesIdealWorld.MinorRunesTypes type)
	{	
		string tagToCheck = GameManager.gameManager.runesIdealWorld.GetMinorRune(type).tag;
		bool toReturn = false;

		foreach (MinorRune rune in complementRunes)
		{
			if (tagToCheck == rune.tag)
			{
				toReturn = true;
			}
		}

		return toReturn;
	}*/

}

public struct RuneSorted
{
	public MinorRune runeScript;
	public int priority;
}