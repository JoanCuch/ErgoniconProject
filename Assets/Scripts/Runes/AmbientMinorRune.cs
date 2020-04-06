using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the environment
/// </summary>
public class AmbientMinorRune : MinorRune
{
    [SerializeField] [ReadOnly] EnergyInteractable environment;
	[Space]
	[SerializeField] private float energyFlow;

	[SerializeField] private float detectionRadius;
	[SerializeField] private string environmentTag;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		//Starting energy. temp
		AddEnergy(30);


	}

	// Update is called once per frame
	void Update()
	{
		if (parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

		if (energyFlowInput)
		{
		
			//Get the energy from the source and add it to himself
			if (environment == null)
			{
				environment = FindEnvironmentAround();
			}
			else
			{
				float newE = environment.AbsorbEnergy(energyFlow * Time.deltaTime);

				AddEnergy(newE);
			}
		}
		else
		{
			//Get the energy from himself and add it to the source
			if (environment == null)
			{
				environment = FindEnvironmentAround();
			}
			else
			{
				float newE = AbsorbEnergy(energyFlow * Time.deltaTime);

				environment.AddEnergy(newE);
			}
		}
	}


	private EnergyInteractable FindEnvironmentAround()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

		EnergyInteractable envi = null;


		foreach (Collider col in hitColliders)
		{
			if(col.transform.tag == environmentTag)
			{
				envi = col.GetComponent<EnergyInteractable>();
			}
		}

		if (envi == null)
		{
			Debug.LogWarning("Error 404: Environment not found");
		}

		return envi;
	}
}
