using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MajorRune : MonoBehaviour
{
	[SerializeField] private EnergyInteractable attachedObject;

	[SerializeField] private Transform centerPoint;
	[SerializeField] private Transform rightPoint;
	[SerializeField] private float runeInterspace;
	[SerializeField] private float extraLine;

	private List<RuneSorted> runesList = new List<RuneSorted>();
	private Transform line;

	void Start()
	{
		attachedObject = transform.parent.GetComponent<EnergyInteractable>();		
	}

	void Update()
	{
		if(attachedObject == null)
		{
			attachedObject = transform.parent.GetComponent<EnergyInteractable>();
		}
	}

	/// <summary>
	/// Adds an already instantiated to the major rune.
	/// </summary>
	public void AddMinorRune(Transform minorRune)
	{

		MinorRune minorRuneScript = minorRune.GetComponent<MinorRune>();
		MinorRune.RuneClassifications runeClassification = minorRuneScript.GetRuneClassification();
		MinorRune.RuneTypes runeType = minorRuneScript.GetRuneType();

		//Creating the struct that will be used to save the rune.
		RuneSorted newRune = new RuneSorted();
		newRune.runeScript = minorRuneScript;

		/*
		 * CURRENT SORT PRIORITY ORDER OF MINOR RUNES
		 * source, 0
		 * source inverse 10
		 * source range 20
		 * source flow 30
		 * center 40
		 * transformation 50
		 * transformation inverse 60
		 * transformation range 70
		 * transformation flow 80
		 * twin 90
		 */

		switch (runeClassification)
		{
			case MinorRune.RuneClassifications.source:
				newRune.priority = 0;
				break;

			case MinorRune.RuneClassifications.transformation:
				newRune.priority = 50;
				break;

			case MinorRune.RuneClassifications.complement:
				ComplementMinorRune extraScript = (ComplementMinorRune)newRune.runeScript;
				if (extraScript.GetTargetClassification() == MinorRune.RuneClassifications.source)
				{
					if (runeType == MinorRune.RuneTypes.inverse)newRune.priority = 10;
					else if (runeType == MinorRune.RuneTypes.range)newRune.priority = 20;
					else if (runeType == MinorRune.RuneTypes.flow) newRune.priority = 30;
				}
				else if (extraScript.GetTargetClassification() == MinorRune.RuneClassifications.transformation)
				{
					if (runeType == MinorRune.RuneTypes.inverse)newRune.priority = 60;
					else if (runeType == MinorRune.RuneTypes.range)newRune.priority = 70;
					else if (runeType == MinorRune.RuneTypes.flow)newRune.priority = 80;
				}
				break;

			case MinorRune.RuneClassifications.root:

				if (runeType == MinorRune.RuneTypes.center) newRune.priority = 40;
				else if (runeType == MinorRune.RuneTypes.twin) newRune.priority = 90;
				break;

			default:
				Debug.LogWarning("minor rune type not detected: " + runeClassification);
				break;
		}


		if (runesList == null)
		{
			Debug.LogWarning("instantiating the list again");
			runesList = new List<RuneSorted>();
		}

		//In a major rune can only exist one source and one transformation rune
		if (runeClassification == MinorRune.RuneClassifications.source ||
			runeClassification == MinorRune.RuneClassifications.transformation)
		{
			foreach (RuneSorted sort in runesList)
			{
				if (sort.runeScript.GetRuneClassification() == runeClassification)
				{
					runesList.Remove(sort);
					Destroy(sort.runeScript.gameObject);
					break;
				}
			}
		}
		//In a major rune can only exist one type rune for the root runes.
		else if (runeClassification == MinorRune.RuneClassifications.root)
		{
			foreach (RuneSorted sort in runesList)
			{
				if (sort.runeScript.GetRuneType() == runeType)
				{
					runesList.Remove(sort);
					Destroy(sort.runeScript.gameObject);
					break;
				}
			}
		}


		//Assigning the rune parents, adding to the list and sorting.
		minorRuneScript.SetMajorRune(this);
		minorRune.parent = this.gameObject.transform;

		runesList.Add(newRune);
		SortAndUpdateRunePositions();
	}

	public void DestroyMinorRune(MinorRune.RuneTypes _typeToDelete)
	{
		//Searching and destroying the desired rune
		foreach (RuneSorted rune in runesList)
		{
			if (rune.runeScript.GetRuneType() == _typeToDelete)
			{
				runesList.Remove(rune);
				Destroy(rune.runeScript.gameObject);
				break;
			}
		}

		SortAndUpdateRunePositions();
		Debug.Log("deleting rune type of: " + _typeToDelete);
	}

	public void DestroyComplementRune(MinorRune.RuneTypes _typeToDelete, MinorRune.RuneClassifications _attachedRune)
	{
		Debug.Log("runes list has: " + runesList.Count + " elements");
		//Searching and destroying the desired rune	
		foreach (RuneSorted rune in runesList)
		{
			if (rune.runeScript.GetRuneClassification() == MinorRune.RuneClassifications.complement)
			{
				if (rune.runeScript != null &&
					rune.runeScript.GetRuneType() == _typeToDelete &&
					rune.runeScript.GetComponent<ComplementMinorRune>().GetTargetClassification() == _attachedRune)
				{
					runesList.Remove(rune);
					Destroy(rune.runeScript.gameObject);
					break;
				}
			}
		}

		//Update positions and create a fake lastRuneDrawn
		SortAndUpdateRunePositions();
		Debug.Log("deleting rune type of: " + _typeToDelete);
	}

	private void SortAndUpdateRunePositions()
	{
		runesList.Sort((x, y) => x.priority.CompareTo(y.priority));

		//Givin the position to all the elements on the list.
		Vector3 direction = -(rightPoint.position - centerPoint.position).normalized;

		float length = 0;
		foreach (RuneSorted sorted in runesList)
		{
			length += sorted.runeScript.GetSpriteWidth();
			length += runeInterspace;
		}
		length -= runeInterspace;

		Vector3 startPosition = centerPoint.position - direction * (length / 2);

		for (int i = 0; i < runesList.Count; i++)
		{
			MinorRune rune = runesList[i].runeScript;
			float runeWidth = rune.GetSpriteWidth();

			rune.transform.rotation = this.transform.rotation;

			rune.transform.position = startPosition + direction * (runeWidth / 2);
			startPosition = startPosition + direction * ((runeWidth) + runeInterspace);
		}

		//Update de line.
		if (line == null)
		{
			line = Instantiate(GameManager.gameManager.globalBlackboard.GetLinePrefab().transform, this.transform);
		}

		float extra = extraLine;
		if (length == 0)
			extra = 0;

		line.position = centerPoint.position;
		line.rotation = this.transform.rotation;
		line.localScale = new Vector3(length + extra, line.localScale.y, line.localScale.z);
	}

	public MinorRune GetMinorRune(MinorRune.RuneClassifications _runeClassification)
	{
		MinorRune runeToReturn = null;

		foreach(RuneSorted rune in runesList)
		{
			if(rune.runeScript.GetRuneClassification()== _runeClassification)
			{
				runeToReturn = rune.runeScript;
				break;
			}
		}

		return runeToReturn;
	}
	public List<RuneSorted> GetAllMinorRunes() { return runesList; }

	public EnergyInteractable GetAttachedObject()
	{
		return attachedObject;
	}
}



public class RuneSorted
{
	public MinorRune runeScript;
	public int priority;
}