using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Telemetry;

public class InputManager : MonoBehaviour
{

	private GameManager gameManager;


	[SerializeField] private string leftControllerTag;
	[SerializeField] private string rightControllerTag;
	[SerializeField] private string leftIndexFingerTag;
	[SerializeField] private string rightIndexFingerTag;
	[SerializeField] private string leftGloveTag;
	[SerializeField] private string rightGloveTag;


	[SerializeField] private SteamVR_Action_Boolean DrawAction;
	//[SerializeField] private SteamVR_Action_Boolean A;
	//[SerializeField] private SteamVR_Action_Boolean B;
	[SerializeField] private SteamVR_Action_Boolean SelectAction;


	public enum PlayerActions {
		draw,
		select,
		drawStateDown
	}

	//Temp and hardcoded
	private GameObject leftController;
	private GameObject rightController;
	private GameObject leftIndexFinger;
	private GameObject rightIndexFinger;
	private GameObject leftGlove;
	private GameObject rightGlove;

	// Start is called before the first frame update
	void Start()
    {
		FindControllers();
		gameManager = GameManager.gameManager;
    }
    // Update is called once per frame
    void Update()
    {

		/*if (TouchPad[SteamVR_Input_Sources.LeftHand].stateDown)
		{
			gameManager.TryingToSelect();
		}*/

		if (IsDoingAction(PlayerActions.drawStateDown)) SendEvent(DataManager.Actions.draw);
		if (IsDoingAction(PlayerActions.select)) SendEvent(DataManager.Actions.select);

    }

	public bool IsDoingAction(PlayerActions _action)
	{
		bool isDoingIt = false;

		switch (_action)
		{
			case PlayerActions.draw:

				if (DrawAction[SteamVR_Input_Sources.LeftHand].state)
				{
					isDoingIt = true;
				}
				if (DrawAction[SteamVR_Input_Sources.RightHand].state)
				{
					isDoingIt = true;
				}
				break;

			case PlayerActions.select:
				if (SelectAction[SteamVR_Input_Sources.LeftHand].stateDown ||
					SelectAction[SteamVR_Input_Sources.RightHand].stateDown)
				{
					isDoingIt = true;
				
				}
				break;

			case PlayerActions.drawStateDown:

				if (DrawAction[SteamVR_Input_Sources.LeftHand].stateDown)
				{
					isDoingIt = true;
				}
				if (DrawAction[SteamVR_Input_Sources.RightHand].stateDown)
				{
					isDoingIt = true;
				}
				break;

			default:
				break;
		}
		return isDoingIt;
	}

	public (GameObject left, GameObject right) GetActiveControllers(PlayerActions _action)
	{
		GameObject _leftController = null;
		GameObject _rightController = null;

		switch (_action)
		{
			case PlayerActions.draw:

				if (leftController == null || rightController == null)
				{
					FindControllers();
				}

				if (DrawAction[SteamVR_Input_Sources.LeftHand].state)
				{
					_leftController = leftController;
				}

				if (DrawAction[SteamVR_Input_Sources.RightHand].state)
				{
					_rightController = rightController;
				}
				break;

			case PlayerActions.select:

				if(leftIndexFinger == null || rightIndexFinger == null)
				{
					FindIndexFingers();
					Debug.Log(leftIndexFinger + " " + rightIndexFinger);
				}


				if (SelectAction[SteamVR_Input_Sources.LeftHand].state)
				{
					_leftController = leftIndexFinger;
				}

				if (SelectAction[SteamVR_Input_Sources.RightHand].state)
				{
					_rightController = rightIndexFinger;
				}
				break;

			default:
				break;
		}

		if (_leftController == null && _rightController == null)
			Debug.LogWarning("Returning null both controllers from inputManager");

		return (_leftController, _rightController);
	}

	private void FindControllers()
	{
		leftController = GameObject.FindGameObjectWithTag(leftControllerTag);
		rightController = GameObject.FindGameObjectWithTag(rightControllerTag);
	}

	private void FindIndexFingers()
	{
		leftIndexFinger = GameObject.FindGameObjectWithTag(leftIndexFingerTag);
		rightIndexFinger = GameObject.FindGameObjectWithTag(rightIndexFingerTag);
	}

	private void FindGloves()
	{
		leftGlove = GameObject.FindGameObjectWithTag(leftGloveTag);
		rightGlove = GameObject.FindGameObjectWithTag(rightGloveTag);
	}

	public void ChangeGlovesLayer(int _newLayer)
	{
		if(leftGlove == null || rightGlove == null)
		{
			FindGloves();
		}

		if (leftGlove != null && rightGlove != null)
		{
			leftGlove.gameObject.layer = _newLayer;

			rightGlove.gameObject.layer = _newLayer;

		}
	}


	private void SendEvent(DataManager.Actions _action)
	{
		DataManager.dataManager.AddAction(
			DataManager.Actors.player,
			_action,
			"",
			Time.time,
			Time.time,
			null
			);
	}



	/*public bool IsChangingToSelectMode()
	{
		return A[SteamVR_Input_Sources.LeftHand].state;
	}

	public bool IsChangingToDrawMode()
	{
		return B[SteamVR_Input_Sources.LeftHand].state; ;
	}

	public bool CheckDrawRune()
	{
		//TODO remove hardcoded on lefthand
		return Click[SteamVR_Input_Sources.LeftHand].state;
	}

	public bool IsSelectingTarget()
	{
		return TouchPad[SteamVR_Input_Sources.LeftHand].stateDown;
	}

	public GameObject getDrawingController()
	{
		//Todo remove hardcoded on left hand
		return leftController;
	}*/



	
}
