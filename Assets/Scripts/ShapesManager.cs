﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telemetry;

public class ShapesManager : MonoBehaviour
{
	//Serializable variables
	[SerializeField] private GameObject starPrefab;
	[SerializeField] private string OneHandedGesturesPath;
	[SerializeField] private string TwoHandedGesturesPath;
	[SerializeField] private string OneHandedGesturesPathBuild;
	[SerializeField] private string TwoHandedGesturesPathBuild;
	//"Assets/GestureRecognition/shapes1.dat"

	//Variables that don't change

	private GestureRecognition gestureRecognition;
	private GestureCombinations gestureCombinations;

	//Variables that changes constantly

	private GlobalBlackboard globalBlackboard;

	private GameObject leftActiveController;
	private GameObject rightActiveController;
	private GameObject oneHandedActiveController;

	private List<string> stroke = new List<string>();
	private int strokeIndex = 0;

	private bool gestureStarted;
	private bool oneHandedGesture;

	private string currentShapeName;

	//Events information
	private double currentShapeSimilarity;
	private double currentShapeScale;
	private Vector3 currentShapePos;
	private Vector3 currentShape0;
	private Vector3 currentShape1;
	private Vector3 currentShape2;
	private float currentShapeStartTime;
	private float currentShapeEndTime;

	// Start is called before the first frame update
	void Start()
	{
		currentShapeStartTime = Time.time;
		currentShapeEndTime = Time.time;

		globalBlackboard = GameManager.gameManager.globalBlackboard;

		gestureStarted = false;

		gestureRecognition = new GestureRecognition();
		gestureCombinations = new GestureCombinations(2);

		//Load the file gestures file
#if UNITY_EDITOR
		bool success = gestureRecognition.loadFromFile(OneHandedGesturesPath);
		Debug.LogWarning((success ? "One-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

		success = gestureCombinations.loadFromFile(TwoHandedGesturesPath);
		Debug.LogWarning((success ? "Two-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));
#else	
		bool success = gestureRecognition.loadFromFile(Application.streamingAssetsPath + OneHandedGesturesPathBuild);
		Debug.LogWarning((success ? "One-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

		success = gestureCombinations.loadFromFile(Application.streamingAssetsPath + TwoHandedGesturesPathBuild);
		Debug.LogWarning((success ? "Two-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));
#endif



		//bool success = gestureRecognition.loadFromFile(OneHandedGesturesPath);
		//Debug.LogWarning((success ? "One-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

		//success = gestureCombinations.loadFromFile(TwoHandedGesturesPath);
		//Debug.LogWarning((success ? "Two-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));
	}

	// Update is called once per frame
	void Update()
	{


	}



	public void GestureUpdate()
	{
		//There is no gesture started, start one
		if (gestureStarted == false)
		{
			StartGesture();
		}
		else
		{
			//There is a gesture started, update or finish it.
			if (oneHandedGesture)
			{
				if(oneHandedActiveController == null)
				{
					//End one-handed gesture
					int gestureId = gestureRecognition.endStroke(
						ref currentShapeSimilarity,
						ref currentShapePos,
						ref currentShapeScale,
						ref currentShape0,
						ref currentShape1,
						ref currentShape2
						);

					currentShapeEndTime = Time.time;


					if (gestureId < 0)
					{
						// Error trying to identify any gesture
						Debug.LogWarning("Failed to identify gesture.");
						currentShapeName = globalBlackboard.failedGestureName;
					}
					else
					{
						//Geting the name of the identified rune.
						currentShapeName = gestureRecognition.getGestureName(gestureId);
						Debug.Log("## one-handed gesture: " + currentShapeName + " ##");
					}

					foreach (string star in stroke)
					{
						Destroy(GameObject.Find(star));
						strokeIndex = 0;
					}

					gestureStarted = false;
					SendEvent();
				}
				else
				{
					//Update one-handed gesture
					Vector3 p = oneHandedActiveController.transform.position;
					Quaternion q = oneHandedActiveController.transform.rotation;
					gestureRecognition.contdStrokeQ(p, q);
					addToStrokeTrail(p);
				}
			}
			else
			{
				if (leftActiveController == null  || rightActiveController == null)
				{
					//End two-handed gesture
					gestureCombinations.endStroke(0);
					gestureCombinations.endStroke(1);
					int itendifiedGestureCombo = gestureCombinations.identifyGestureCombination(ref currentShapeSimilarity);
					currentShapePos = Vector3.zero;
					currentShapeScale = 0;
					currentShape0 = Vector3.zero;
					currentShape1 = Vector3.zero;
					currentShape2 = Vector3.zero;
					currentShapeEndTime = Time.time;

					if (itendifiedGestureCombo < 0)
					{
						// Error trying to identify any gesture
						currentShapeName = globalBlackboard.failedGestureName;
						Debug.LogWarning("Failed to identify gesture.");
					}
					else
					{
						//Geting the name of the identified rune.
						currentShapeName = gestureCombinations.getGestureCombinationName(itendifiedGestureCombo);
						Debug.Log("## two-handed gesture: " + currentShapeName + " ##");

					}

					foreach (string star in stroke)
					{
						Destroy(GameObject.Find(star));
						strokeIndex = 0;
					}

					gestureStarted = false;
					SendEvent();
				}
				else
				{
					//Update two-handed gesture
					Vector3 p_left = leftActiveController.transform.position;
					Quaternion q_left = leftActiveController.transform.rotation;
					gestureCombinations.contdStrokeQ(0, p_left, q_left);
					addToStrokeTrail(p_left);

					Vector3 p_right = rightActiveController.transform.position;
					Quaternion q_right = rightActiveController.transform.rotation;
					gestureCombinations.contdStrokeQ(1, p_right, q_right);
					addToStrokeTrail(p_right);
				}
			}		
		}
	}

	private void StartGesture()
	{
		//The active controllers are given by the GameMager
		Debug.Log("start gesture: " + leftActiveController + " " + rightActiveController);
		currentShapeStartTime = Time.time;

		if (leftActiveController == null && rightActiveController == null)
		{
			//Debug.LogWarning("Starting a gesture with no activeControllers");
		}
		else if (leftActiveController == null || rightActiveController == null)
		{
			//Start one-handed gesture
			GameObject hmd = Camera.main.gameObject;
			Vector3 hmd_p = hmd.transform.localPosition;
			Quaternion hmd_q = hmd.transform.localRotation;
			gestureRecognition.startStroke(hmd_p, hmd_q);
			gestureStarted = true;
			oneHandedGesture = true;
			//Debug.Log("#### Start one-handed gesture");
		}
		else
		{
			//Start two-handed gesture
			GameObject hmd = Camera.main.gameObject;
			Vector3 hmd_p = hmd.transform.localPosition;
			Quaternion hmd_q = hmd.transform.localRotation;
			gestureCombinations.startStroke(0, hmd_p, hmd_q); //leff hand
			gestureCombinations.startStroke(1, hmd_p, hmd_q); //right hand
			gestureStarted = true;
			oneHandedGesture = false;
			//Debug.Log("#### Start two-handed gesture");

		}
	}

	public string GetCurrentShapeName()
	{
		string name = currentShapeName;
		currentShapeName = null;
		return name;
	}

	public void SetActiveController(GameObject _leftController, GameObject _rightController)
	{
		if (gestureStarted)
		{
			if (oneHandedGesture && (_leftController != null && _rightController != null))
			{
				//If the player is activating both controlers in a one-handed gesture. Restart
				finishCurentGesture();
			}
			/*else if (!oneHandedGesture && (_leftController == null || _rightController == null))
			{
				//If the player is activating only one controler in a two-handed gesture. Restart
				finishCurentGesture();
			}*/
		}


		//##############



		leftActiveController = _leftController;
		rightActiveController = _rightController;

		if(leftActiveController == null && rightActiveController != null)
		{
			oneHandedActiveController = _rightController;
		}
		else if(leftActiveController != null && rightActiveController == null)
		{
			oneHandedActiveController = _leftController;
		}
		else
		{
			oneHandedActiveController = null;
		}
	}

	private void finishCurentGesture()
	{
		print("hard reset gesture");
		gestureCombinations.endStroke(0);
		gestureCombinations.endStroke(1);
		gestureRecognition.endStroke();

		foreach (string star in stroke)
		{
			Destroy(GameObject.Find(star));
			strokeIndex = 0;
		}

		gestureStarted = false;
		currentShapeName = "restart";
	}


	/// <summary>
	/// Utility function. Add a point to the drawing of the player
	/// </summary>
	private void addToStrokeTrail(Vector3 p)
	{
		GameObject star_instance = Instantiate(starPrefab);
		GameObject star = new GameObject("stroke_" + strokeIndex++);
		star_instance.name = star.name + "_instance";
		star_instance.transform.SetParent(star.transform, false);
		System.Random random = new System.Random();
		star.transform.localPosition = new Vector3((float)random.NextDouble() / 80, (float)random.NextDouble() / 80, (float)random.NextDouble() / 80) + p;
		star.transform.localRotation = new Quaternion((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f).normalized;
		//star.transform.localRotation.Normalize();
		float star_scale = (float)random.NextDouble() + 0.3f;
		star.transform.localScale = new Vector3(star_scale, star_scale, star_scale);
		stroke.Add(star.name);
	}



	private void SendEvent()
	{
		/*List<KeyValuePair<string, object>> extraInfo = new List<KeyValuePair<string, object>>
		{
			new KeyValuePair<string, object>("similarity", currentShapeSimilarity),
			new KeyValuePair<string, object>("scale", currentShapeScale),
			new KeyValuePair<string, object>("position", currentShapePos),
			new KeyValuePair<string, object>("dir0", currentShape0),
			new KeyValuePair<string, object>("dir1", currentShape1),
			new KeyValuePair<string, object>("dir2", currentShape2)
		};*/
	
		
		string extra =
			"similarity: " + currentShapeSimilarity + "/ " +
			"scale: " + currentShapeScale + "/ " +
			"position: " + currentShapePos + "/ " +
			"dir0: " + currentShape0 + "/ " +
			"dir1: " + currentShape1 + "/ " +
			"dir2: " + currentShape2 + "/ ";
			

		DataManager.dataManager.AddAction(
			DataManager.Actors.player,
			DataManager.Actions.draw,
			currentShapeName,
			currentShapeStartTime,
			currentShapeEndTime,
			extra);
	}
}
