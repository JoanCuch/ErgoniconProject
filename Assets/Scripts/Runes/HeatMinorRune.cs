﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the source rune
/// 2 Make the conversion
/// 3 Give it to the target
/// </summary>
public class HeatMinorRune : TransformationRune
{

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		if (!GetWorkable())
			return;

		if (GetFlowDirection()) //The energy flows is energy -> heat
		{
			//Get the energy from the source and add it to himself
			if (GetSource() == null)
			{
				SetSource(parentMajorRune.GetMinorRune(RuneClassifications.source));
			}
			else
			{
				AddEnergy(GetSource().AbsorbEnergy(GetFlowRate() * Time.deltaTime));
			}
			//Transform the energy
			float transformedenergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetTransformationEfficiency();			;
			AddHeat(transformedenergy);

			//Give the energy to the target
			if (GetTarget() == null)
			{
				SetTarget(parentMajorRune.GetAttachedObject());
			}
			else
			{
				GetTarget().AddHeat(AbsorbHeat(GetFlowRate() * Time.deltaTime));
			}

		}
		else //The energy flow is heat -> energy
		{
			//Get the heat from the target
			if (GetTarget() == null)
			{
				SetTarget(parentMajorRune.GetAttachedObject());
			}
			else
			{
				AddHeat(GetTarget().AbsorbHeat(GetFlowRate() * Time.deltaTime));
			}

			//Transfrom the heat to energy
			float transformedenergy = AbsorbHeat(GetFlowRate() * Time.deltaTime) * GetTransformationEfficiency();
			AddEnergy(transformedenergy);

			//Give the energy to the source
			if (GetSource() == null)
			{
				SetSource(parentMajorRune.GetMinorRune(RuneClassifications.source));
			}
			else
			{
				GetSource().AddEnergy(AbsorbEnergy(GetFlowRate() * Time.deltaTime));
			}			
		}	
    }
}
