using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesIdealWorld : MonoBehaviour
{
	//Plato <3

	public GameObject prefabBase;

	public GameObject prefabPhysicalObject;
	public GameObject prefabAmbient;
	public GameObject prefabDirect;

	public GameObject prefabExtra;
	public GameObject prefabTwin;
	public GameObject prefabInverse;
	
	public GameObject prefabHeat;
	public GameObject prefabForce;

	public GameObject prefabLine;

	//public GameObject prefabGravitational;
	//public GameObject prefabAtract;

	public string OpenRuneEditingModeShapeName;
	public string CloseRuneEditingModeShapeName;


	public List<string> minorRunesNames = new List<string>
	{
		"basic",
		"physicalObject",
		"ambient",
		"direct",
		"extra",
		"twin",
		"inverse",
		"heat",
		"force"
	};
	public string failedGestureName = "fail";

	public enum MinorRunesTypes
	{
		basic,
		inverse,
		physicalObject,
		ambient,
		direct,
		extra,
		twin,
		heat,
		force
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
	public GameObject GetLinePrefab()
	{
		return prefabLine;
	}

}
