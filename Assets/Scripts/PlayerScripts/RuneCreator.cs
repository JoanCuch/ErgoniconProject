using System.Collections;
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
	private InputManager inputManager;
	private RunesIdealWorld runeIdealWorld;

	//Gameobjects prefabs
	public GameObject starPrefab;
	public GameObject majorRunePrefab;

	//Gesture recognition
	GestureRecognition gr = null;
	public string file_load_gestures = "Assets/GestureRecognition/shapes1.dat";
	
	//Gesture recognition for every unique draw
	private GameObject activeDrawingController = null;
	private List<string> stroke = new List<string>();
	private int recordGestureId = -1;
	private int stroke_index = 0;

	//Other
	private Transform targetObject;
	private MajorRune targetMajorRune;
	private RaycastHit targetHit;

	//State
	private bool isDrawing;

	
	// Start is called before the first frame update
	void Start()
    {
		gr = new GestureRecognition();

		//Load the file gestures file
		bool success = gr.loadFromFile(file_load_gestures);
		Debug.Log((success ? "Gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

		isDrawing = false;


		gameManager = GameManager.gameManager;
		inputManager = gameManager.inputManager;
		runeIdealWorld = gameManager.runesIdealWorld;

		targetObject = null;
		targetMajorRune = null;

	}

    // Update is called once per frame
    void Update()
    {
		if(gameManager == null)
		{
			gameManager = GameManager.gameManager;
			inputManager = gameManager.inputManager;
		}

		GestureRuneUpdate();
		
	}

	/// <summary>
	/// If the player is drawing, manages from start to ending of the drawing and gets the name of the resulting shape.
	/// </summary>
	private void GestureRuneUpdate()
	{
		//Starting a rune
		if (activeDrawingController == null)
		{
			//If there isn't any active controller, it means that the player isn't drawing anything
			if (isDrawing)
			{
				//The draw trigger of the controller is active
				activeDrawingController = inputManager.getDrawingController();
			}
			else
			{
				return;
			}

			//If we are here, a button has been pressed
			GameObject hmd = Camera.main.gameObject;
			Vector3 hmd_p = hmd.transform.localPosition;
			Quaternion hmd_q = hmd.transform.localRotation;
			gr.startStroke(hmd_p, hmd_q, recordGestureId);
			return;
		}

		//Continuing the rune

		if (isDrawing)
		{
			Vector3 p = activeDrawingController.transform.position;
			Quaternion q = activeDrawingController.transform.rotation;
			gr.contdStrokeQ(p, q);
			addToStrokeTrail(p);
			return;
		}

		//Finishing the rune

		activeDrawingController = null;

		foreach (string star in stroke)
		{
			Destroy(GameObject.Find(star));
			stroke_index = 0;
		}

		double similarity = 0; // This will receive the similarity value (0~1)
		Vector3 pos = Vector3.zero; // This will receive the position where the gesture was performed.
		double scale = 0; // This will receive the scale at which the gesture was performed.
		Vector3 dir0 = Vector3.zero; // This will receive the primary direction in which the gesture was performed (greatest expansion).
		Vector3 dir1 = Vector3.zero; // This will receive the secondary direction of the gesture.
		Vector3 dir2 = Vector3.zero; // This will receive the minor direction of the gesture (direction of smallest expansion).
		int gesture_id = gr.endStroke(ref similarity, ref pos, ref scale, ref dir0, ref dir1, ref dir2);

		if(recordGestureId >= 0)
		{
			Debug.Log("i dont know why I am here");
			return;
		}



		if (gesture_id < 0)
		{
			// Error trying to identify any gesture
			Debug.LogWarning("Failed to identify gesture.");
		}
		else
		{
			//Geting the name of the identified rune.
			string gesture_name = gr.getGestureName(gesture_id);
			CreateMinorRune(gesture_name);
		}
		return;
	}

	/// <summary>
	/// Instantiates a minor rune using his name
	/// </summary>
	private void CreateMinorRune(string runeName)
	{		
		//Debug.Log("Identified gesture " + gesture_name + "(" + gesture_id + ")\n(Similarity: " + similarity + ")");

		//Getting the prefab of the minorRune to instantiate it
		GameObject minorRunePrefab = runeIdealWorld.GetMinorRune(runeName);
		GameObject newMinorRune = Instantiate(minorRunePrefab);
		
		//The minor rune has to be added to a major rune script.
		if (targetMajorRune == null)
		{
			targetMajorRune = GetRuneFromTarget(targetObject, targetHit);
		}

		targetMajorRune.AddMinorRune(newMinorRune.transform);		
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

	/// <summary>
	/// Utility function. Add a point to the drawing of the player
	/// </summary>
	private void addToStrokeTrail(Vector3 p)
	{
		GameObject star_instance = Instantiate(starPrefab);
		GameObject star = new GameObject("stroke_" + stroke_index++);
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


	public void SetIsDrawing(bool draw) {
		isDrawing = draw;
	}
	public void SetTarget(Transform _newTarget, RaycastHit hit)
	{
		targetObject = _newTarget;
		targetHit = hit;
		targetMajorRune = null;
	}


}
