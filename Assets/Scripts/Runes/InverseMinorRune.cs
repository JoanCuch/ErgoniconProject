using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMinorRune : MinorRune
{

	MajorRune parentRune;
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		parentRune = GetMajorRune();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if(parentRune == null)
		{
			parentRune = GetMajorRune();
		}

		ChangeEnergyFlowDirection(false);
		
	}

	private void OnDestroy()
	{

		if (parentRune == null)
		{
			parentRune = GetMajorRune();
		}

		ChangeEnergyFlowDirection(true);
	}

	private void ChangeEnergyFlowDirection(bool condition)
	{
		//If the condition is true, the energy flow direction will be from source to the target.
		//If the condition is false, the energy flow direction will be from targe to source.

		if (parentRune == null)
		{
			GetMajorRune();
		}
		else
		{
			//EnergyInteractable basic = parentRune.GetMinorRune(RuneClassifications.basic);
			//EnergyInteractable complement = parentRune.GetMinorRune(RuneClassifications.complement);
			SourceRune source = (SourceRune)parentRune.GetMinorRune(RuneClassifications.source);
			TransformationRune transformation = (TransformationRune)parentRune.GetMinorRune(RuneClassifications.transformation);

			//if (basic != null)
			//	parentRune.GetMinorRune(RuneClassifications.basic).SetEnergyFlow(condition);

			//if (complement != null)
			//parentRune.GetMinorRune(RuneClassifications.complement).SetEnergyFlow(condition);
			
			//TODO HARDCODED this is a very big design problem
			if(source != null)
			{
				if(transformation.GetRuneType() == RuneTypes.force)
				{
					source.SetFlowDirection(!condition);
				}
				else
				{
					source.SetFlowDirection(condition);
				}
			}
			
				
			//parentRune.GetMinorRune(RuneClassifications.source).(condition);

			if (transformation != null)
				//parentRune.GetMinorRune(RuneClassifications.transformation).SetEnergyFlow(condition);
				transformation.SetFlowDirection(condition);

		}

	}


}
