using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the environment
/// </summary>
public class AmbientMinorRune : SourceMinorRune
{
	[SerializeField] EnergyInteractable source;

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

		//Check the source
		if (GetInversed())
		{
			//This minor rune has attached a inverse minor rune
			source = GetMajorRune().GetAttachedObject();
		}
		else
		{
			source = GetAttachedEnvironment();
		}

		//Get the energy
		if (source != null)
		{
			
			float absorbedEnergy = source.AbsorbEnergy(GetFlowRate() * Time.deltaTime);
			
			float residualEnergy = absorbedEnergy * (1-GetEfficiency());
			absorbedEnergy -= residualEnergy;
			//The residual Energy that goes to the object
			GetMajorRune().GetAttachedObject().AddEnergy(residualEnergy / 2);
			//The residual Energy that goes to the environment
			GetAttachedEnvironment().AddEnergy(residualEnergy / 2);
			//The absorved energy
			AddEnergy(absorbedEnergy);
		}

	}
}
