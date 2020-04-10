﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorRune : MonoBehaviour
{

	[SerializeField] private Transform source;
	[SerializeField] private Transform transf;
	[SerializeField] private Transform complement;
	[SerializeField] private Transform basic;

	[SerializeField] [ReadOnly] private MinorRune sourceRune;
	[SerializeField] [ReadOnly] private MinorRune transformationRune;
	[SerializeField] [ReadOnly] private MinorRune basicRune;

	[SerializeField] [ReadOnly] private List<MinorRune> complementRunes;

	[SerializeField] [ReadOnly] private EnergyInteractable attachedObject;

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
		MinorRune minorRuneScript = minorRune.GetComponent<MinorRune>();

		MinorRune.RuneTypes runeType = minorRuneScript.GetRuneType();

		Transform runeParent = null;

		switch (runeType)
		{
			case MinorRune.RuneTypes.source:
				runeParent = source;
				sourceRune = minorRuneScript;
				break;

			case MinorRune.RuneTypes.transformation:
				runeParent = transf;
				transformationRune = minorRuneScript;
				break;
			case MinorRune.RuneTypes.complement:
				runeParent = complement;
				AddComplementRune(minorRuneScript);
				break;

			case MinorRune.RuneTypes.basic:
				runeParent = basic;
				basicRune = minorRuneScript;
				break;

			default:
				Debug.LogWarning("minor rune type not detected: " + runeType);
				break;
		}

		if (runeType != MinorRune.RuneTypes.complement)
		{
			foreach (Transform child in runeParent)
			{
				Destroy(child.gameObject);
			}
		}

		minorRune.parent = runeParent;
		minorRune.position = runeParent.position;
		minorRune.rotation = runeParent.rotation;
		minorRuneScript.SetMajorRune(this);

	}

	public MinorRune GetAttachedRuneOfType(MinorRune.RuneTypes runeType)
	{
		MinorRune runeToReturn = null;

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
		}

		return runeToReturn;
	}

	public EnergyInteractable GetAttachedObject()
	{
		return attachedObject;
	}

	private void AddComplementRune(MinorRune minorRuneScript)
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
	}

	private MinorRune GetComplementRune()
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
	}

	public bool CheckComplementRuneIsTypeOf(RunesIdealWorld.MinorRunesTypes type)
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
	}

}
