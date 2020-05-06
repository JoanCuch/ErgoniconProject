using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticMinorRune : TransformationRune
{
	Rigidbody targetRigidbody;
	[SerializeField] private float impulseMultiplier;
	[SerializeField] private float distanceSQRT;

	//private Transform forceTarget;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (!GetWorkable())
			return;
	}


	private void FixedUpdate()
	{
		if (!GetWorkable())
			return;

		//Get the energy from the source and add it to himself
		if (GetSource() == null)
		{
			SetSource(parentMajorRune.GetMinorRune(RuneClassifications.source));
		}
		else
		{
			AddEnergy(GetSource().AbsorbEnergy(GetFlowRate()* Time.deltaTime));
		}

		//Transform the energy
		float transformedEnergy = 0;
		transformedEnergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetEfficiency();

		float residualEnergy = transformedEnergy * (1 - GetEfficiency());
		transformedEnergy -= residualEnergy;

		if (GetAttachedEnvironment() == null)
		{
			GetMajorRune().GetAttachedObject().AddEnergy(residualEnergy);
		}
		else
		{
			//The residual Energy that goes to the object
			GetMajorRune().GetAttachedObject().AddEnergy(residualEnergy / 2);
			//The residual Energy that goes to the environment
			GetAttachedEnvironment().AddEnergy(residualEnergy / 2);
		}

		//MOVEMENT TIME
		Vector3 forceDirection = Vector3.zero;
		float forceImpulse = 0;
		Vector3 forcePoint = Vector3.zero;

		if(GetLinkedRune() == null)
		{
			//There is no twin rune, the direction is the normal of the rune plane.
			forceDirection = -GetMajorRune().transform.forward;

			if (GetInversed() == false)
				forceDirection *= -1;

			forceImpulse = impulseMultiplier * transformedEnergy;
		}
		else
		{
			Transform linked = GetLinkedRune().transform;

			//There is a twin rune, the direction is the twin position.
			forceDirection = (linked.position - this.GetMajorRune().transform.position).normalized;

			if (GetInversed())
				forceDirection *= -1;


			float distance = Vector3.Distance(linked.position, this.GetMajorRune().transform.position);
			forceImpulse = (GetRange() / Mathf.Pow(distance, distanceSQRT) * impulseMultiplier * transformedEnergy);
		}
			
		//Set the forcePoint
		forcePoint = GetMajorRune().transform.position;

		//Add foce
		if (targetRigidbody == null)
		{
			if (GetTarget() == null)
			{
				SetTarget(parentMajorRune.GetAttachedObject());
			}
			targetRigidbody = GetTarget().GetComponent<Rigidbody>();
		}

		targetRigidbody.AddForceAtPosition(forceDirection * forceImpulse, forcePoint);		
	}

	/*public void SetForceTarget(Transform _newForceTarget)
	{
		forceTarget = _newForceTarget;
	}*/

}
