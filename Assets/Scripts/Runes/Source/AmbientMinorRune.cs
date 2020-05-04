using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the environment
/// </summary>
public class AmbientMinorRune : SourceMinorRune
{
	[SerializeField] [ReadOnly] EnergyInteractable source;
	[SerializeField] private float sourceUpdateDelay;
	
	// [SerializeField] [ReadOnly] EnergyInteractable environment;
	//[SerializeField] private float energyFlow;

	//[SerializeField] private float detectionRadius;
	[SerializeField] [TagSelector] private string environmentTag;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		StartCoroutine(UpdateSource());

	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (!GetWorkable())
			return;

		//Get the energy
		if (source != null)
		{
			float newE = source.AbsorbEnergy(GetFlowRate() * Time.deltaTime);
			AddEnergy(newE);
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
				break;
			}
		}

		return envi;
	}


	IEnumerator UpdateSource()
	{
		while (true)
		{
			if (GetWorkable())
			{
				if (GetInversed())
				{
					//This minor rune has attached a inverse minor rune
					source = parentMajorRune.GetAttachedObject();
				}
				else
				{
					source = FindEnvironmentAround();
				}
			}

			yield return new WaitForSeconds(sourceUpdateDelay);
		}
	}

	/*private bool IsEnvironmentToFar()
	{

		float distance = (transform.position - source.transform.position).magnitude;

		if(distance > GetRange())
		{
			return true;
		}
		else
		{
			return false;
		}
	}*/

}
