using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorRune : EnergyInteractable
{
	
	//Prefabs
	public enum minorRunes { basic, inverse, physicObject, direct, ambient, heat, force, error}

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

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void AddMinorRune(minorRunes newRune)
	{
		switch (newRune)
		{
			case minorRunes.basic:
				InstantiateRune(prefabBase, basic);	
				break;

			case minorRunes.inverse:
				InstantiateRune(prefabInverse, complement);
				break;

			case minorRunes.physicObject:
				InstantiateRune(prefabObject, source);
				break;

			case minorRunes.direct:
				InstantiateRune(prefabDirect, source);
				break;

			case minorRunes.ambient:
				InstantiateRune(prefabAmbient, source);
				break;

			case minorRunes.heat:
				InstantiateRune(prefabHeat, transf);
				break;

			case minorRunes.force:
				InstantiateRune(prefabForce, transf);
				break;

			case minorRunes.error:
				Debug.LogWarning("Error while trying to instantiate a minor rune");
				break;

			default:
				Debug.LogWarning("Error while trying to instantiate a minor rune");
				break;
		}
	}

	private void InstantiateRune(GameObject runePrefab, Transform parent)
	{
		foreach (Transform child in parent) Destroy(child.gameObject);
		Instantiate(runePrefab, parent);
	}

}
