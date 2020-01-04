using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
	public GameObject bullet;
	public Transform barrelPivot;
	public float shootingSpeed = 1;
	public GameObject muzzleFlash;

	private Animator animator;
	private Interactable interactable;

	public SteamVR_Action_Boolean fireAction;
	


    void Start()
    {
		animator = GetComponent<Animator>();
		//muzzleFlash.SetActive(false);
		interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {

        if(interactable.attachedToHand != null)
		{
			SteamVR_Input_Sources source = interactable.attachedToHand.handType;

			if (fireAction[source].stateDown)
			{
				Fire();
			}
		}
    }


	void Fire()
	{
		Debug.Log("fire");
		Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
		bulletrb.velocity = barrelPivot.forward * shootingSpeed;
		//muzzleFlash.SetActive(true);
	}
}
