using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
	//Deprecated script
	public SteamVR_Action_Vector2 input;
	public float speed = 1;

	private CharacterController characterController;


	private Vector3 gravity;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();

		gravity = new Vector3(0, -9.81f, 0);
	}


	void Update()
    {

		if (input.axis.magnitude > 0.1f)
		{
			Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
			characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
		}

    }
}
