﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class duties;
///  - Make the gesture recognition of the minorRunes
///  - Instantiate minor runes and add to his attached major runes
///  - Instantiate major runes and add to his attached physical object
/// </summary>
public class RuneCreator : MonoBehaviour
{
	
	//OtherScripts
	private GameManager gameManager;
	private GlobalBlackboard runeIdealWorld;

	//Gameobjects prefabs
	public GameObject majorRunePrefab;

	//Other
	private Transform targetObject;
	private MajorRune targetMajorRune;
	private RaycastHit targetHit;

	// Start is called before the first frame update
	void Start()
    {
		gameManager = GameManager.gameManager;
		runeIdealWorld = gameManager.globalBlackboard;

		targetObject = null;
		targetMajorRune = null;

	}

    // Update is called once per frame
    void Update()
    {
		if(gameManager == null)
		{
			gameManager = GameManager.gameManager;
		}		
	}

	/// <summary>
	/// Instantiates a minor rune using his name
	/// </summary>
	public void CreateMinorRune(string runeName)
	{		
		if (targetMajorRune == null)
		{
			//If there is no targetobject from to get the rune, return
			if(targetObject == null)
			{
				Debug.Log("No target selected");
				return;
			}
			targetMajorRune = GetRuneFromTarget(targetObject, targetHit);		
		}

		//Getting the prefab of the minorRune to instantiate it
		GameObject minorRunePrefab = runeIdealWorld.GetMinorRune(runeName);

		if(minorRunePrefab == null)
		{
			Debug.Log("No rune prefab");
			return;
		}







		GameObject newMinorRune = Instantiate(minorRunePrefab);
		targetMajorRune.AddMinorRune(newMinorRune.transform);
		Debug.Log("creating minor rune type of: " + runeName);
	}

	/// <summary>
	/// Returns the MajorRune script of a gameObject. If it doesn't have one, it creates a new one.
	/// The gameobject can be a scene prop or a major rune. 
	/// The hit is the position and direction where the major rune will appear.
	/// </summary>
	private MajorRune GetRuneFromTarget(Transform _newTarget, RaycastHit hit)
	{
		MajorRune majorRuneToReturn = null;

		MajorRune runeScript = _newTarget.GetComponent<MajorRune>();
		PhysicObject objectScript = _newTarget.GetComponent<PhysicObject>();

		if (runeScript != null)
		{
			//If the target is a rune then get it.
			majorRuneToReturn = runeScript;
		}
		else if (objectScript != null)
		{
			//If the target is an object, then add a major rune to it.
			GameObject newRune = Instantiate(majorRunePrefab, hit.point, Quaternion.LookRotation(hit.normal));
			objectScript.AttachMajorRune(newRune.transform);
			majorRuneToReturn = newRune.GetComponent<MajorRune>();		
		}
		else
		{
			Debug.LogWarning("The selected object is not magicable");
		}

		if (majorRuneToReturn == null)
			Debug.LogWarning("There is no rune to attach");

		return majorRuneToReturn;		
	}

	public void SetTarget(Transform _newTarget, RaycastHit hit)
	{
		targetObject = _newTarget;
		targetHit = hit;
		targetMajorRune = null;
	}

	public MajorRune GetTargetRune()
	{
		return targetMajorRune;
	}


}
