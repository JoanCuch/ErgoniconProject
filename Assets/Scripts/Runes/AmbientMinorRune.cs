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
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

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
