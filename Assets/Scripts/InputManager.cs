using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class InputManager : MonoBehaviour
{


	public SteamVR_Action_Boolean Click;



	//Temp and hardcoded
	private GameObject leftController;


	// Start is called before the first frame update
	void Start()
    {
		leftController = GameObject.FindGameObjectWithTag("LEFTHAND");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public bool CheckActivateDrawMode()
	{
		//TODO checkactivatedrawmode()
		return true;
	}

	public bool CheckDrawRune()
	{
		//TODO remove hardcoded on lefthand
		return Click[SteamVR_Input_Sources.LeftHand].state;
	}

	public GameObject getDrawingController()
	{
		//Todo remove hardcoded on left hand
		return leftController;
	}



	
}
