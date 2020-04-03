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
	[SerializeField] private float forceExtra;

	// Start is called before the first frame update
	void Start()
	{
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
		float transformedenergy = AbsorbEnergy(energyFlow * Time.deltaTime) * conversionRateEnergyToForce;

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
			Debug.Log(transformedenergy);
			Debug.Log(Vector3.right * transformedenergy * 100);

			targetRigidbody.AddForce(Vector3.right * transformedenergy * forceExtra);
		}
	}
}
