using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the environment
/// </summary>
public class AmbientMinorRune : SourceRune
{
    [SerializeField] [ReadOnly] EnergyInteractable environment;
	//[SerializeField] private float energyFlow;

	//[SerializeField] private float detectionRadius;
	[SerializeField] [TagSelector] private string environmentTag;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		//Starting energy. temp
		AddEnergy(30);


	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (!GetWorkable())
			return;


		if(environment != null && IsEnvironmentToFar())
		{
			environment = null;
		}

		if(environment == null)
		{
			environment = FindEnvironmentAround();
		}
		else
		{
			if (GetFlowDirection())
			{
				//Get the energy from the source and add it to himself
				float newE = environment.AbsorbEnergy(GetFlowRate() * Time.deltaTime);

				AddEnergy(newE);
			}
			else
			{
				//Get the energy from himself and add it to the source
				float newE = AbsorbEnergy(GetFlowRate() * Time.deltaTime);

				environment.AddEnergy(newE);
			}
		}		
	}


	private EnergyInteractable FindEnvironmentAround()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetRange());

		EnergyInteractable envi = null;


		foreach (Collider col in hitColliders)
		{
			if(col.transform.tag == environmentTag)
			{
				envi = col.GetComponent<EnergyInteractable>();
			}
		}

		return envi;
	}
	private bool IsEnvironmentToFar()
	{
		float distance = (transform.position - environment.transform.position).magnitude;

		if(distance > GetRange())
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
