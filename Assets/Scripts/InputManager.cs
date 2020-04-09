using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class InputManager : MonoBehaviour
{

	private GameManager gameManager;


	[SerializeField] private string leftControllerTag;
	[SerializeField] private string rightControllerTag;

	[SerializeField] private SteamVR_Action_Boolean Click;
	[SerializeField] private SteamVR_Action_Boolean A;
	[SerializeField] private SteamVR_Action_Boolean B;
	[SerializeField] private SteamVR_Action_Boolean TouchPad;


	public enum PlayerActions { draw, select}

	//Temp and hardcoded
	private GameObject leftController;
	private GameObject rightController;

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



    }

	public bool IsDoingAction(PlayerActions _action)
	{
		bool isDoingIt = false;

		switch (_action)
		{
			case PlayerActions.draw:

				if (Click[SteamVR_Input_Sources.LeftHand].state)
				{
					isDoingIt = true;
				}
				if (Click[SteamVR_Input_Sources.RightHand].state)
				{
					isDoingIt = true;
				}
				break;

			case PlayerActions.select:
				if (TouchPad[SteamVR_Input_Sources.LeftHand].stateDown)
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
		if(leftController == null || rightController == null) { FindControllers(); }

		GameObject _leftController = null;
		GameObject _rightController = null;

		switch (_action)
		{
			case PlayerActions.draw:

				if (Click[SteamVR_Input_Sources.LeftHand].state)
				{
					_leftController = leftController;
				}

				if (Click[SteamVR_Input_Sources.RightHand].state)
				{
					_rightController = rightController;
				}
				break;

			default:
				break;
		}

		return (_leftController, _rightController);
	}

	private void FindControllers()
	{
		leftController = GameObject.FindGameObjectWithTag(leftControllerTag);
		rightController = GameObject.FindGameObjectWithTag(rightControllerTag);

		Debug.Log("searching for controllers: " + leftController + " " + rightController);


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
