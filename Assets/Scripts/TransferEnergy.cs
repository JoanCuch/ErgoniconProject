using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class TransferEnergy : MonoBehaviour
{
	public float deviationAccepted = 0.6f;

	private Vector3 handForward;
	private Vector3 targetDirection;

	public GestureTest spell;

	//Inputs

	public SteamVR_Action_Boolean trackpadTouch;
	public SteamVR_Action_Boolean thumbstickTouch;
	public SteamVR_Action_Boolean aTouch;
	public SteamVR_Action_Boolean bTouch;
	public SteamVR_Action_Boolean firefingerTouch;
	public SteamVR_Action_Boolean gripTouch;

	public SteamVR_Action_Boolean addEnergy;
	public SteamVR_Action_Boolean substractEnergy;

	public SteamVR_Input_Sources actionHandSource;
	public SteamVR_Input_Sources targetHandSource;

	public float transferedEnergyPerSecond = 0.2f;

	//

	private Transform targetHand;
	private Transform hoverPoint;

	private Transform target;



	void Start()
	{
		GetTargetHand();
	}

	void Update()
	{

		//bool adding = CheckAddEnergy();
		//bool substracting = CheckSubstractEnergy();

		//The hand is open and ready to send at the same time that an action is selected
		if (ReadyToTransfer() && spell.currentSpell != GestureTest.spells.none)
		{
			//Check if the hand is targeting an object
			target = SearchTarget();

			/*if (adding)
			{
				ChangeSize(target, true);
			}
			else if (substracting)
			{
				ChangeSize(target, false);
			}*/
			if (target != null)
			{
				if (spell.currentSpell == GestureTest.spells.grow)
				{
					ChangeSize(target, true);
				}
				else if (spell.currentSpell == GestureTest.spells.reduce)
				{
					ChangeSize(target, false);
				}
			}
		}

	}

	private bool ReadyToTransfer()
	{
		//If it is not touching, transfer energy
		bool isTouching = false;

		if (trackpadTouch[targetHandSource].state) isTouching = true;
		else if (thumbstickTouch[targetHandSource].state) isTouching = true;
		else if (aTouch[targetHandSource].state) isTouching = true;
		else if (bTouch[targetHandSource].state) isTouching = true;
		else if (firefingerTouch[targetHandSource].state) isTouching = true;
		else if (gripTouch[targetHandSource].state) isTouching = true;



		return !isTouching;

	}

	private bool CheckAddEnergy()
	{
		bool add = addEnergy[actionHandSource].state;
		return add;
	}

	private bool CheckSubstractEnergy()
	{
		bool add = substractEnergy[actionHandSource].state;
		return add;
	}

	private void ChangeSize(Transform objective, bool makeBigger)
	{

		if (makeBigger)
		{
			objective.localScale *= 1 + transferedEnergyPerSecond * Time.deltaTime;
		}
		else
		{
			objective.localScale *= 1 - transferedEnergyPerSecond * Time.deltaTime;

		}
	}

	private void GetTargetHand()
	{
		if (targetHandSource == SteamVR_Input_Sources.RightHand)
		{
			targetHand = GameObject.FindGameObjectWithTag("RIGHTHAND").transform;
		}
		else if (targetHandSource == SteamVR_Input_Sources.LeftHand)
		{
			targetHand = GameObject.FindGameObjectWithTag("LEFTHAND").transform;
		}
		else
		{
			Debug.LogError("Hand object transform not found");
		}

		if (targetHand == null)
		{
			Debug.LogError("Target hand is null");
		}
		else
		{
			//Get the hoverPoint
			foreach(Transform child in targetHand)
			{
				if(child.tag == "HOVERPOINT")
				{
					hoverPoint = child;
				}
			}

			if (hoverPoint == null)
				Debug.LogError("Hover point is null");
 		}

		Debug.Log("Target hand: " + targetHand.name);
		Debug.Log("Hoverpoint: " + hoverPoint.name);
	}

	private Transform SearchTarget()
	{
		Transform hitObject = null;

		if (targetHand == null)
			GetTargetHand();

		RaycastHit hit;
		Ray ray;

		ray = new Ray(hoverPoint.position,Vector3.Normalize(-hoverPoint.right));

		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.transform;
		}
		return hitObject;
	}

}
