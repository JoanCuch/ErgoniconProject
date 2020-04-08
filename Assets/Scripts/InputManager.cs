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


	public enum PlayerActions { draw}

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

				if (Click[SteamVR_Input_Sources.LeftHand].stateDown)
				{
					isDoingIt = true;
				}
				if (Click[SteamVR_Input_Sources.RightHand].stateDown)
				{
					isDoingIt = true;
				}
				break;

			default:
				break;
		}
		return isDoingIt;
	}

	public void GetActiveControllers(PlayerActions _action,out GameObject _leftController, out GameObject _rightController)
	{
		if(leftController == null || rightController == null) { FindControllers(); }


		_leftController = null;
		_rightController = null;

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
