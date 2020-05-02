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
				ExtraMinorRune extraScript = (ExtraMinorRune)newRune.runeScript;
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


		/*switch (runeType)
		{
			case MinorRune.RuneTypes.center:
				break;
			case MinorRune.RuneTypes.inverse:
				newRune.priority = 2;
				break;
			case MinorRune.RuneTypes.ambient:
				break;
			case MinorRune.RuneTypes.direct:
				break;
			case MinorRune.RuneTypes.extra:
				ExtraMinorRune extraScript = (ExtraMinorRune)newRune.runeScript;
				if(extraScript.GetTargetClassification() == MinorRune.RuneClassifications.source)
				{
					Debug.Log("extra source rune");
					newRune.priority = 1;
				}
				else if (extraScript.GetTargetClassification() == MinorRune.RuneClassifications.transformation)
				{
					Debug.Log("extra transformation rune");
					newRune.priority = 4;
				}
				else
				{
					Debug.LogWarning("extra rune with no priority");
				}
				break;
			case MinorRune.RuneTypes.twin:
				newRune.priority = 6;
				break;
			case MinorRune.RuneTypes.thermic:
				break;
			case MinorRune.RuneTypes.kinetic:
				break;
			default:
				break;
		}*/


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
		else if (runeType == MinorRune.RuneTypes.twin)
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
		//MinorRune.RuneTypes runeType = _rune.runeScript.GetRuneType();

		//Destroying the destroyRune
		//Destroy(lastRuneDrawn.runeScript.gameObject);

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

		//Destroy the template
		//Destroy(_rune.runeScript.gameObject);

		//Update positions and create a fake lastRuneDrawn
		SortAndUpdateRunePositions();
		//lastRuneDrawn = new RuneSorted();

		Debug.Log("deleting rune type of: " + _typeToDelete);

	}

	public void DestroyMinorRune(MinorRune.RuneTypes _typeToDelete, MinorRune.RuneClassifications _attachedRune)
	{

		//Searching and destroying the desired rune	
		foreach (RuneSorted rune in runesList)
		{
			if (rune.runeScript != null &&
				rune.runeScript.GetRuneType() == _typeToDelete &&
				rune.runeScript.GetComponent<ExtraMinorRune>().GetTargetClassification() == _attachedRune)
			{
				runesList.Remove(rune);
				Destroy(rune.runeScript.gameObject);
				break;
			}
		}

		//Destroy the template
		//Destroy(_rune.runeScript.gameObject);

		//Update positions and create a fake lastRuneDrawn
		SortAndUpdateRunePositions();
		//lastRuneDrawn = new RuneSorted();

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

	/*private void DestroyLastRuneDrawn()
	{
		/*if (lastRuneDrawn.isFake == false)
		{
			runesList.Remove(lastRuneDrawn);
			Destroy(lastRuneDrawn.runeScript.gameObject);
			SortAndUpdateRunePositions();
			lastRuneDrawn = new RuneSorted();
			lastRuneDrawn.isFake = true;
		}
	}*/

}



public class RuneSorted
{
	public MinorRune runeScript;
	public int priority;
}