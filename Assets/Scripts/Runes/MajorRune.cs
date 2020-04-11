using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MajorRune : MonoBehaviour
{
	[SerializeField] [ReadOnly] private EnergyInteractable attachedObject;

	[SerializeField] private Transform centerPoint;
	[SerializeField] private Transform rightPoint;
	[SerializeField] private float runeInterspace;
	[SerializeField] private float extraLine;

	private List<RuneSorted> runesList = new List<RuneSorted>();
	private Transform line;

	private RuneSorted lastRuneDrawn;




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
		 * sourceExtra, 1
		 * inverse, 2
		 * basic, 3
		 * transformationExtra, 4
		 * transformation, 5
		 * twin 6
		 */

		switch (runeClassification)
		{
			case MinorRune.RuneClassifications.source:
				newRune.priority = 0;
				break;

			case MinorRune.RuneClassifications.transformation:
				newRune.priority = 5;
				break;
			case MinorRune.RuneClassifications.complement:
				newRune.priority = 1;
				break;

			case MinorRune.RuneClassifications.basic:
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


		//Adding the rune to the list and sorting it.
		if (runesList == null)
		{
			Debug.LogWarning("instantiating the list again");
			runesList = new List<RuneSorted>();
		}

		//Checking that can only be one source, transformation or basic rune.
		//In the case of complementRunes, can only be one inverse and one twin.
		if (runeClassification != MinorRune.RuneClassifications.complement)
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
		else if (runeType == MinorRune.RuneTypes.inverse || runeType == MinorRune.RuneTypes.twin)
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

		//If everything went correct
		lastRuneDrawn = newRune;
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

	public MinorRune GetAttachedRuneOfType(MinorRune.RuneClassifications runeType)
	{
		MinorRune runeToReturn = null;

		foreach(RuneSorted rune in runesList)
		{
			if(rune.runeScript.GetRuneClassification()== runeType)
			{
				runeToReturn = rune.runeScript;
				break;
			}
		}

		return runeToReturn;
	}

	public EnergyInteractable GetAttachedObject()
	{
		return attachedObject;
	}

	public void DestroyLastRuneDrawn()
	{
		if (lastRuneDrawn.isFake == false)
		{
			runesList.Remove(lastRuneDrawn);
			Destroy(lastRuneDrawn.runeScript.gameObject);
			SortAndUpdateRunePositions();
			lastRuneDrawn = new RuneSorted();
			lastRuneDrawn.isFake = true;
		}
	}

}



public class RuneSorted
{
	public MinorRune runeScript;
	public int priority;
	public bool isFake;

	public RuneSorted()
	{
		isFake = false;
	}
}