using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorRune : EnergyInteractable
{
	
	//Prefabs
	//public enum minorRunes { basic, inverse, physicObject, direct, ambient, heat, force, error}

	public GameObject prefabBase;
	public GameObject prefabInverse;
	public GameObject prefabObject;
	public GameObject prefabAmbient;
	public GameObject prefabDirect;
	public GameObject prefabHeat;
	public GameObject prefabForce;


	//Variables
	[SerializeField] private Transform source;
	[SerializeField] private Transform transf;
	[SerializeField] private Transform complement;
	[SerializeField] private Transform basic;

	private MinorRune sourceRune;
	private MinorRune transformationRune;
	private MinorRune complementRune;
	private MinorRune basicRune;

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
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
				break;

			case MinorRune.RuneTypes.transformation:
				runeParent = transf;
				break;
			case MinorRune.RuneTypes.complement:
				runeParent = complement;
				break;

			case MinorRune.RuneTypes.basic:
				runeParent = basic;
				break;

			default:
				Debug.LogWarning("minor rune type not detected: " + runeType);
				break;
		}

		foreach(Transform child in runeParent)
		{
			Destroy(child.gameObject);
		}

		minorRune.parent = runeParent;
		minorRune.position = runeParent.position;
		minorRune.rotation = runeParent.rotation;
		
	}
}
