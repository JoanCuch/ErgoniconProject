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
	//[SerializeField] private SteamVR_Action_Boolean SelectAction;
	[SerializeField] private SteamVR_Action_Single SelectAction;
	[SerializeField] private float SelectActionSensibility;
	[SerializeField] private SteamVR_Action_Skeleton skeletonRight;
	[SerializeField] private SteamVR_Action_Skeleton skeletonLeft;
	[SerializeField] private float IndexCurlSensibility;

	public enum PlayerActions {
		draw,
		select,
		drawStateDown
	}

	private GameObject leftController;
	private GameObject rightController;
	private GameObject leftIndexFinger;
	private GameObject rightIndexFinger;
	private GameObject leftGlove;
	private GameObject rightGlove;

	private bool isSelecting;

	// Start is called before the first frame update
	void Start()
    {
		isSelecting = false;
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
		if (!isSelecting && IsDoingAction(PlayerActions.select)) SendEvent(DataManager.Actions.select);
		
		isSelecting = IsDoingAction(PlayerActions.select);
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
				if (
					(SelectAction[SteamVR_Input_Sources.RightHand].axis >= SelectActionSensibility &&
						skeletonRight.indexCurl <= SelectActionSensibility)
					||
					(SelectAction[SteamVR_Input_Sources.LeftHand].axis >= SelectActionSensibility &&
						skeletonLeft.indexCurl <= SelectActionSensibility)
						)
				{
					isDoingIt = true;
				
				}


				/*if (SelectAction[SteamVR_Input_Sources.LeftHand].axis >= SelectActionSensibility ||
					SelectAction[SteamVR_Input_Sources.RightHand].axis >= SelectActionSensibility)
					&&
					skeletonRight.indexCurl >= SelectActionSensibility


				{
					isDoingIt = true;
				
				}*/

				/*if (SelectAction[SteamVR_Input_Sources.LeftHand].stateDown ||
					SelectAction[SteamVR_Input_Sources.RightHand].stateDown)
				{
					isDoingIt = true;
				
				}*/
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
					//Debug.Log(leftIndexFinger + " " + rightIndexFinger);
				}


				if (SelectAction[SteamVR_Input_Sources.LeftHand].axis >= SelectActionSensibility &&
						skeletonLeft.indexCurl <= SelectActionSensibility)
				{
					_leftController = leftIndexFinger;
				}

				if (SelectAction[SteamVR_Input_Sources.RightHand].axis >= SelectActionSensibility &&
						skeletonRight.indexCurl <= SelectActionSensibility)
				{
					_rightController = rightIndexFinger;
				}

				/*if (SelectAction[SteamVR_Input_Sources.LeftHand].state)
				{
					_leftController = leftIndexFinger;
				}

				if (SelectAction[SteamVR_Input_Sources.RightHand].state)
				{
					_rightController = rightIndexFinger;
				}*/
				break;

			default:
				break;
		}

		//if (_leftController == null && _rightController == null)
			//Debug.LogWarning("Returning null both controllers from inputManager");

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
			"null",
			Time.time,
			Time.time,
			"null"
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
