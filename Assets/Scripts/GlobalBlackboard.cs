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
	public GameObject prefabDestroy;

	public string OpenRuneEditingModeShapeName;
	public string CloseRuneEditingModeShapeName;
	public string DestroyRuneShapeName;

	public List<string> minorRunesNames;
	public string failedGestureName = "fail";

	
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



	public GameObject GetLinePrefab()
	{
		return prefabLine;
	}

}


