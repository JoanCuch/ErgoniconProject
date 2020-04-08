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
			sourceRune = parentMajorRune.GetAttachedRuneOfType(RuneTypes.source);
		}
		else
		{
			AddEnergy(sourceRune.AbsorbEnergy(energyFlow * Time.deltaTime));
		}

		//Transform the energy
		//float transformedEnergy = AbsorbEnergy(energyFlow * Time.deltaTime) * conversionRateEnergyToForce;
		float transformedEnergy = energyFlow * Time.deltaTime * conversionRateEnergyToForce;


		//Give the energy to the target
		if (targetRigidbody == null)
		{
			if (target == null)
			{
				target = parentMajorRune.GetAttachedObject();
			}
			targetRigidbody = target.GetComponent<Rigidbody>();
		}
		else
		{
		
			Vector3 direction = -GetMajorRune().transform.forward;
			float impulse = transformedEnergy * impulseMultiplier;

			targetRigidbody.AddForceAtPosition(direction * transformedEnergy * impulseMultiplier, GetMajorRune().transform.position);

		}
	}
}
