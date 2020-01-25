using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GestureTest : MonoBehaviour
{

	GestureRecognition gr = null;

	public string file_load_gestures = "Assets/GestureRecognition/shapes1.dat";

	GameObject rightController;

	public SteamVR_Action_Boolean Click;
	bool castSpell;
	private GameObject active_controller = null;
	private int record_gesture_id = -1;
	private int stroke_index = 0;
	List<string> stroke = new List<string>();

	public enum spells {
		grow,
		reduce,
		none
	}

	public spells currentSpell;


	// Start is called before the first frame update
	void Start()
	{

		currentSpell = spells.none;

		rightController = GameObject.FindGameObjectWithTag("RIGHTHAND");

		gr = new GestureRecognition();


		bool success = gr.loadFromFile(file_load_gestures);
		Debug.Log((success ? "Gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

	}



    // Update is called once per frame
    void Update()
    {
		bool castingSpell = Click[SteamVR_Input_Sources.RightHand].state;

		//Starting a spell
		if (active_controller == null)
		{
			if (castingSpell)
			{
				//The trigger of the right hand is being clicked
				active_controller = rightController;
			}
			else
			{
				return;
			}

			//If we are here, a button has been pressed
			GameObject hmd = Camera.main.gameObject;
			Vector3 hmd_p = hmd.transform.localPosition;
			Quaternion hmd_q = hmd.transform.localRotation;
			gr.startStroke(hmd_p, hmd_q, record_gesture_id);
			return;
		}

		//Continuing the spell
		if (castingSpell)
		{
			Vector3 p = active_controller.transform.position;
			Quaternion q = active_controller.transform.rotation;
			gr.contdStrokeQ(p, q);
			addToStrokeTrail(p);
			return;
		}

		//Finishing the spell
		active_controller = null;

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
			ChangeSpell(gesture_id);
		}
		return;



	}

	public void ChangeSpell(int gestureId)
	{
		string gestureName = gr.getGestureName(gestureId);

		switch (gestureName)
		{
			case "grow":
				currentSpell = spells.grow;
				break;

			case "reduce":
				currentSpell = spells.reduce;
				break;

			case "none":
				currentSpell = spells.none;
				break;

			default:
				Debug.LogWarning("spell not detected");
			break;
		}

		Debug.Log("changed spell to: " + currentSpell);


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
