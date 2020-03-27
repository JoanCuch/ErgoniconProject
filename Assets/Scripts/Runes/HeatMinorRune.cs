﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the source rune
/// 2 Make the conversion
/// 3 Give it to the target
/// </summary>
public class HeatMinorRune : MinorRune
{
	[SerializeField] [ReadOnly] EnergyInteractable sourceRune;
	[SerializeField] [ReadOnly] EnergyInteractable target;
	[Space]
	[SerializeField] private float energyFlow;
	[SerializeField] private float conversionRateEnergyHeat;
	[SerializeField] private float heatFlow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if(parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

		//Get the energy from the source and add it to himself
		if(sourceRune == null)
		{
			sourceRune = parentMajorRune.GetAttachedRuneOfType(RuneTypes.source);
		}
		else
		{
			AddEnergy(sourceRune.AbsorbEnergy(energyFlow * Time.deltaTime));
		}

		//Transform the energy
		float transformedenergy = AbsorbEnergy(energyFlow * Time.deltaTime) * conversionRateEnergyHeat;
		AddHeat(transformedenergy);

		//Give the energy to the target
		if(target == null)
		{
			target = parentMajorRune.GetAttachedObject();
		}
		else
		{
			target.AddHeat(AbsorbHeat(heatFlow * Time.deltaTime));
		}

		
    }
}
