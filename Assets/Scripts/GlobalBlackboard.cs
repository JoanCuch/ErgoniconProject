using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBlackboard : MonoBehaviour
{
	//Plato <3
	[System.Serializable]
	public struct RunePrefab
	{
		public string name;
		public GameObject prefab;
	}




	public List<RunePrefab> runePrefabs;

	public GameObject prefabLine;



	/*public GameObject prefabBase;

	public GameObject prefabPhysicalObject;
	public GameObject prefabAmbient;
	public GameObject prefabDirect;

	public GameObject prefabExtra;
	public GameObject prefabTwin;
	public GameObject prefabInverse;
	
	public GameObject prefabHeat;
	public GameObject prefabForce;*/


	public GameObject prefabDestroy;

	//public GameObject prefabGravitational;
	//public GameObject prefabAtract;

	public string OpenRuneEditingModeShapeName;
	public string CloseRuneEditingModeShapeName;
	public string DestroyRuneShapeName;


	public List<string> minorRunesNames;
	public string failedGestureName = "fail";

	/*public enum MinorRunesTypes
	{
	"center",
		"inverse",
		"ambient",
		"thermic",
		"twin",
		"direct",
		"kinetic",
		"joint",
		"range",
		"flow"



		basic,
		inverse,
		physicalObject,
		ambient,
		direct,
		extra,
		twin,
		heat,
		force,
		destroy
	}*/

	// Start is called before the first frame update
	void Start()
	{
		minorRunesNames = new List<string>();

		foreach (RunePrefab pref in runePrefabs)
		{
			minorRunesNames.Add(pref.name);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}


	public GameObject GetMinorRune(string runeName)
	{
		GameObject runeToReturn = null;

		foreach(RunePrefab pref in runePrefabs)
		{
			if(pref.name == runeName)
			{
				runeToReturn = pref.prefab;
				break;
			}
		}

		if (runeToReturn == null)
		{
			Debug.LogWarning("Returning a null rune from the globalBlackboard");
		}
		else
		{
			Debug.Log("Blackboard is returning a rune type of:" + runeName);
		}

		return runeToReturn;
	}


	/*public GameObject GetMinorRune(string runeName)
	{
		GameObject runeToReturn = null;

		switch (runeName)
		{
			case "basic":
				runeToReturn = prefabBase;
				break;

			case "physicalObject":
				runeToReturn = prefabPhysicalObject;
				break;

			case "ambient":
				runeToReturn = prefabAmbient;
				break;

			case "direct":
				runeToReturn = prefabDirect;
				break;

			case "extra":
				runeToReturn = prefabExtra;
				break;

			case "twin":
				runeToReturn = prefabTwin;
				break;

			case "inverse":
				runeToReturn = prefabInverse;
				break;

			case "heat":
				runeToReturn = prefabHeat;
				break;

			case "force":
				runeToReturn = prefabForce;
				break;

			case "destroy":
				runeToReturn = prefabDestroy;
				break;
		
			default:
				Debug.LogWarning("Couldn't find the minor rune: " + runeName);
				break;
		}

		if (runeToReturn == null)
		{
			Debug.LogWarning("Returning a null rune from the globalBlackboard");
		}

		Debug.Log("Blackboard is returning a rune type of:" + runeName);

		return runeToReturn;
	}*/

	/*public GameObject GetMinorRune(MinorRunesTypes variation)
	{
		GameObject runeToReturn = null;

		switch (variation)
		{
			case MinorRunesTypes.basic:
				runeToReturn = prefabBase;
				break;

			case MinorRunesTypes.physicalObject:
				runeToReturn = prefabPhysicalObject;
				break;

			case MinorRunesTypes.ambient:
				runeToReturn = prefabAmbient;
				break;

			case MinorRunesTypes.direct:
				runeToReturn = prefabDirect;
				break;

			case MinorRunesTypes.extra:
				runeToReturn = prefabExtra;
				break;

			case MinorRunesTypes.twin:
				runeToReturn = prefabTwin;
				break;

			case MinorRunesTypes.inverse:
				runeToReturn = prefabExtra;
				break;

			case MinorRunesTypes.heat:
				runeToReturn = prefabHeat;
				break;

			case MinorRunesTypes.force:
				runeToReturn = prefabForce;
				break;

			case MinorRunesTypes.destroy:
				runeToReturn = prefabDestroy;
				break;

			default:
				Debug.LogWarning("Couldn't find the minor rune: " + variation);
				break;
		}

		if (runeToReturn == null)
		{
			Debug.LogWarning("Returning a null rune from the warehouse");
		}

		return runeToReturn;
	}*/
	public GameObject GetLinePrefab()
	{
		return prefabLine;
	}

}


