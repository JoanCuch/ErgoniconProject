using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorRune : MonoBehaviour
{

	[SerializeField]  private Transform source;
	[SerializeField]  private Transform transf;
	[SerializeField]  private Transform complement;
	[SerializeField]  private Transform basic;
	[Space]
	[SerializeField] [ReadOnly] private MinorRune sourceRune;
	[SerializeField] [ReadOnly] private MinorRune transformationRune;
	[SerializeField] [ReadOnly] private MinorRune complementRune;
	[SerializeField] [ReadOnly] private MinorRune basicRune;
	[SerializeField] [ReadOnly] private EnergyInteractable attachedObject;

	// Start is called before the first frame update
	void Start()
	{

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
				complementRune = minorRuneScript;
				break;

			case MinorRune.RuneTypes.basic:
				runeParent = basic;
				basicRune = minorRuneScript;
				break;

			default:
				Debug.LogWarning("minor rune type not detected: " + runeType);
				break;
		}

		foreach (Transform child in runeParent)
		{
			Destroy(child.gameObject);
		}

		minorRune.parent = runeParent;
		minorRune.position = runeParent.position;
		minorRune.rotation = runeParent.rotation;
		minorRuneScript.SetParent(this);

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
				runeToReturn = complementRune;
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

}
