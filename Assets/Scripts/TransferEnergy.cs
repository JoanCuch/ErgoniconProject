using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class TransferEnergy : MonoBehaviour
{
	public Transform hand; //This is the HoverPoint of the hand
	public Transform target;

	public float deviationAccepted = 0.6f;

	private Vector3 handForward;
	private Vector3 targetDirection;


	//Inputs

	public SteamVR_Action_Boolean trackpadTouch;
	public SteamVR_Action_Boolean thumbstickTouch;
	public SteamVR_Action_Boolean aTouch;
	public SteamVR_Action_Boolean bTouch;
	public SteamVR_Action_Boolean firefingerTouch;
	public SteamVR_Action_Boolean gripTouch;

	public SteamVR_Action_Boolean addEnergy;
	public SteamVR_Action_Boolean substractEnergy;

	public SteamVR_Input_Sources actionHand;
	public SteamVR_Input_Sources targetHand;

	public float transferedEnergyPerSecond = 0.2f;

    void Start()
    {
		actionHand = SteamVR_Input_Sources.LeftHand;
		targetHand = SteamVR_Input_Sources.RightHand;
    
    }

    void Update()
    {

		bool adding = CheckAddEnergy();
		bool substracting = CheckSubstractEnergy();

		//The hand is open and ready to send at the same time that an action is selected
		if(ReadyToTransfer() && (adding || substracting))
		{
			targetDirection = Vector3.Normalize(hand.position - target.position);
			handForward = Vector3.Normalize(hand.right);

			float dot = Vector3.Dot(targetDirection, handForward);

			if (dot >= deviationAccepted)
			{
				if (adding)
				{
					ChangeSize(true);
				}
				else if (substracting)
				{
					ChangeSize(false);
				}
			}
		}



    }

	private bool ReadyToTransfer()
	{
		//If it is not touching, transfer energy
		bool isTouching = false;

		if (trackpadTouch[targetHand].state) isTouching = true;
		else if (thumbstickTouch[targetHand].state) isTouching = true;
		else if (aTouch[targetHand].state) isTouching = true;
		else if (bTouch[targetHand].state) isTouching = true;
		else if (firefingerTouch[targetHand].state) isTouching = true;
		else if (gripTouch[targetHand].state) isTouching = true;

		Debug.Log(targetHand + ": " + isTouching);

		return !isTouching;

	}

	private bool CheckAddEnergy()
	{
		bool add = addEnergy[actionHand].state;
		Debug.Log("action hand add: " + add);
		return add;
	}

	private bool CheckSubstractEnergy()
	{
		bool add = substractEnergy[actionHand].state;
		Debug.Log("action hand substract: " + add);
		return add;
	}

	private void ChangeSize(bool makeBigger)
	{

		if (makeBigger)
		{
			target.localScale *= 1 + transferedEnergyPerSecond * Time.deltaTime;
		}
		else
		{
			target.localScale *= 1 - transferedEnergyPerSecond * Time.deltaTime;

		}
	}



}
