using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesIdealWorld : MonoBehaviour
{

	public GameObject prefabBase;
	public GameObject prefabInverse;
	public GameObject prefabObject;
	public GameObject prefabAmbient;
	public GameObject prefabDirect;
	public GameObject prefabHeat;
	public GameObject prefabForce;
	public GameObject prefabGravitational;
	public GameObject prefabAtract;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject GetRuneByName(string runeName)
	{
		GameObject runeToReturn = null;

		switch (runeName)
		{
			case "base":
				runeToReturn = prefabBase;
				break;

			case "inverse":
				runeToReturn = prefabInverse;
				break;

			case "object":
				runeToReturn = prefabObject;
				break;

			case "ambient":
				runeToReturn = prefabAmbient;
				break;

			case "direct":
				runeToReturn = prefabDirect;
				break;

			case "heat":
				runeToReturn = prefabHeat;
				break;

			case "force":
				runeToReturn = prefabForce;
				break;

			case "attraction":
				runeToReturn = prefabAtract;
				break;

			case "spring":
				runeToReturn = prefabGravitational;
				break;
		
			default:
				Debug.LogWarning("Couldn't find the minor rune: " + runeName);
				break;
		}

		if (runeToReturn == null)
		{
			Debug.LogWarning("Returning a null rune from the warehouse");
		}

		Debug.Log(runeName);

		return runeToReturn;
	}
}
