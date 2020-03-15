using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCreator : MonoBehaviour
{

	//OtherScripts
	public InputManager inputManager;


	//Gesture recognition
	GestureRecognition gr = null;
	public string file_load_gestures = "Assets/GestureRecognition/shapes1.dat";
	
	//Gesture recognition for every unique draw
	private GameObject activeDrawingController = null;
	private List<string> stroke = new List<string>();
	private int recordGestureId = -1;
	private int stroke_index = 0;

	//Hardcoded
	public GameObject attachedObject;
	public GameObject modifiedRune;







	// Start is called before the first frame update
	void Start()
    {
		gr = new GestureRecognition();

		//Load the file gestures file
		bool success = gr.loadFromFile(file_load_gestures);
		Debug.Log((success ? "Gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));
	}

    // Update is called once per frame
    void Update()
    {

		bool isDrawing = inputManager.CheckDrawRune();


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

		if (gesture_id < 0)
		{
			// Error trying to identify any gesture
			Debug.LogWarning("Failed to identify gesture.");
		}
		else
		{
			string gesture_name = gr.getGestureName(gesture_id);
			Debug.LogWarning("Identified gesture " + gesture_name + "(" + gesture_id + ")\n(Similarity: " + similarity + ")");
		}
		return;

	}

	public void addToStrokeTrail(Vector3 p)
	{
		GameObject star_instance = Instantiate(GameObject.Find("star"));
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
}
