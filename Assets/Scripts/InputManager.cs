using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class InputManager : MonoBehaviour
{

	private GameManager gameManager;

	public SteamVR_Action_Boolean Click;
	public SteamVR_Action_Boolean A;
	public SteamVR_Action_Boolean B;
	public SteamVR_Action_Boolean TouchPad;

	//Temp and hardcoded
	private GameObject leftController;


	// Start is called before the first frame update
	void Start()
    {
		leftController = GameObject.FindGameObjectWithTag("LEFTHAND");
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


	public bool IsChangingToSelectMode()
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
	}



	
}
