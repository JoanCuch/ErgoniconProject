using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesIdealWorld : MonoBehaviour
{
	//Plato <3

	public GameObject prefabBase;
	public GameObject prefabInverse;
	public GameObject prefabObject;
	public GameObject prefabAmbient;
	public GameObject prefabDirect;
	public GameObject prefabHeat;
	public GameObject prefabForce;
	public GameObject prefabGravitational;
	public GameObject prefabAtract;

	public string OpenRuneEditingModeShapeName;
	public string CloseRuneEditingModeShapeName;


	public List<string> minorRunesNames = new List<string>
	{
		"basic",
		"inverse",
		"physicObject",
		"ambient",
		"direct",
		"heat",
		"force",
		"attraction",
		"spring"
	};
	public string failedGestureName = "fail";

	public enum MinorRunesTypes
	{
		basic,
		inverse,
		physicObject,
		ambient,
		direct,
		heat,
		force,
		gravitation,
		attraction
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject GetMinorRune(string runeName)
	{
		GameObject runeToReturn = null;

		switch (runeName)
		{
			case "basic":
				runeToReturn = prefabBase;
				break;

			case "inverse":
				runeToReturn = prefabInverse;
				break;

			case "physicObject":
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

		Debug.Log("Preparing a rune of type:" + runeName);

		return runeToReturn;
	}

	public GameObject GetMinorRune(MinorRunesTypes variation)
	{
		GameObject runeToReturn = null;

		switch (variation)
		{
			case MinorRunesTypes.basic:
				runeToReturn = prefabBase;
				break;

			case MinorRunesTypes.inverse:
				runeToReturn = prefabInverse;
				break;

			case MinorRunesTypes.physicObject:
				runeToReturn = prefabObject;
				break;

			case MinorRunesTypes.ambient:
				runeToReturn = prefabAmbient;
				break;

			case MinorRunesTypes.direct:
				runeToReturn = prefabDirect;
				break;

			case MinorRunesTypes.heat:
				runeToReturn = prefabHeat;
				break;

			case MinorRunesTypes.force:
				runeToReturn = prefabForce;
				break;

			case MinorRunesTypes.attraction:
				runeToReturn = prefabAtract;
				break;

			case MinorRunesTypes.gravitation:
				runeToReturn = prefabGravitational;
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
	}
}
