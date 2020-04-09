using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : MonoBehaviour
{
	//Serializable variables
	[SerializeField] private GameObject starPrefab;
	[SerializeField] private string OneHandedGesturesPath;
	[SerializeField] private string TwoHandedGesturesPath;
	//"Assets/GestureRecognition/shapes1.dat"

	//Variables that don't change

	private GestureRecognition gestureRecognition;
	private GestureCombinations gestureCombinations;

	//Variables that changes constantly

	private RunesIdealWorld runesIdealWorld;

	private GameObject leftActiveController;
	private GameObject rightActiveController;
	private GameObject oneHandedActiveController;

	private List<string> stroke = new List<string>();
	private int strokeIndex = 0;

	private bool gestureStarted;
	private bool oneHandedGesture;

	private string lastShapeName;



	// Start is called before the first frame update
	void Start()
	{

		runesIdealWorld = GameManager.gameManager.runesIdealWorld;

		gestureStarted = false;

		gestureRecognition = new GestureRecognition();
		gestureCombinations = new GestureCombinations(2);

		//Load the file gestures file
		bool success = gestureRecognition.loadFromFile(OneHandedGesturesPath);
		Debug.LogWarning((success ? "One-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));

		success = gestureCombinations.loadFromFile(TwoHandedGesturesPath);
		Debug.LogWarning((success ? "Two-Handed gesture file loaded successfully" : "[ERROR] Failed to load gesture file."));
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
					int gestureId = gestureRecognition.endStroke();

					if(gestureId < 0)
					{
						// Error trying to identify any gesture
						Debug.LogWarning("Failed to identify gesture.");
						lastShapeName = runesIdealWorld.failedGestureName;
					}
					else
					{
						//Geting the name of the identified rune.
						lastShapeName = gestureRecognition.getGestureName(gestureId);
						print("@@@@ finished one-handed gesture: " + lastShapeName);
					}

					foreach (string star in stroke)
					{
						Destroy(GameObject.Find(star));
						strokeIndex = 0;
					}

					gestureStarted = false;
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
					int itendifiedGestureCombo = gestureCombinations.identifyGestureCombination();

					if (itendifiedGestureCombo < 0)
					{
						// Error trying to identify any gesture
						lastShapeName = runesIdealWorld.failedGestureName;
						Debug.LogWarning("Failed to identify gesture.");
					}
					else
					{
						//Geting the name of the identified rune.
						lastShapeName = gestureCombinations.getGestureCombinationName(itendifiedGestureCombo);
						print("@@@@ finished two-handed: " + lastShapeName);
					}

					foreach (string star in stroke)
					{
						Destroy(GameObject.Find(star));
						strokeIndex = 0;
					}

					gestureStarted = false;
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
			Debug.Log("#### Start one-handed gesture");
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
			Debug.Log("#### Start two-handed gesture");

		}
	}

	public string GetLastShapeName()
	{
		string name = lastShapeName;
		lastShapeName = null;
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
		lastShapeName = "restart";
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


	//Deprecated

	/*



		public void setActiveController()
		{

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

			if (recordGestureId >= 0)
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




		*/



}
