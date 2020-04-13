using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMinorRune : MinorRune
{
	[SerializeField] [ReadOnly] EnergyInteractable sourceRune;
	[SerializeField] [ReadOnly] EnergyInteractable target;

	Rigidbody targetRigidbody;

	[SerializeField] private float energyFlow;
	[SerializeField] private float conversionRateEnergyToForce;
	[SerializeField] private float forceFlow;
	[SerializeField] private float impulseMultiplier;
	[SerializeField] private float distanceSQRT;

	private Transform forceTarget;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{



	}


	private void FixedUpdate()
	{
		if (parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

		//Get the energy from the source and add it to himself
		if (sourceRune == null)
		{
			sourceRune = parentMajorRune.GetMinorRune(RuneClassifications.source);
		}
		else
		{
			AddEnergy(sourceRune.AbsorbEnergy(energyFlow * Time.deltaTime));
		}

		//Transform the energy
		//float transformedEnergy = AbsorbEnergy(energyFlow * Time.deltaTime) * conversionRateEnergyToForce;
		float transformedEnergy = energyFlow * Time.deltaTime * conversionRateEnergyToForce;


	
		//MOVEMENT TIME
		Vector3 forceDirection = Vector3.zero;
		float forceImpulse = 0;
		Vector3 forcePoint = Vector3.zero;

		if(forceTarget == null)
		{
			//There is no twin rune, the direction is the normal of the rune plane.
			forceDirection = -GetMajorRune().transform.forward;

			forceImpulse = impulseMultiplier;
		}
		else
		{
			//There is a twin rune, the direction is the twin position.
			forceDirection = (forceTarget.position - this.GetMajorRune().transform.position).normalized;

			if (energyFlowInput == false)
				forceDirection *= -1;


			float distance = Vector3.Distance(forceTarget.position, this.GetMajorRune().transform.position);
			forceImpulse = (1 / Mathf.Pow(distance, distanceSQRT) * impulseMultiplier);
		}
			
		//Set the forcePoint
		forcePoint = GetMajorRune().transform.position;

		//Add foce
		if (targetRigidbody == null)
		{
			if (target == null)
			{
				target = parentMajorRune.GetAttachedObject();
			}
			targetRigidbody = target.GetComponent<Rigidbody>();
		}

		targetRigidbody.AddForceAtPosition(forceDirection * forceImpulse, forcePoint);		
	}

	public void SetForceTarget(Transform _newForceTarget)
	{
		forceTarget = _newForceTarget;
	}

}
