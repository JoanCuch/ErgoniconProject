using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMinorRune : MinorRune
{

	MajorRune parentRune;
    // Start is called before the first frame update
    void Start()
    {
		parentRune = GetMajorRune();
	}

    // Update is called once per frame
    void Update()
    {
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
			EnergyInteractable basic = parentRune.GetAttachedRuneOfType(RuneClassifications.basic);
			EnergyInteractable complement = parentRune.GetAttachedRuneOfType(RuneClassifications.complement);
			EnergyInteractable source = parentRune.GetAttachedRuneOfType(RuneClassifications.source);
			EnergyInteractable transformation = parentRune.GetAttachedRuneOfType(RuneClassifications.transformation);

			if (basic != null)
				parentRune.GetAttachedRuneOfType(RuneClassifications.basic).SetEnergyFlow(condition);

			if (complement != null)
				parentRune.GetAttachedRuneOfType(RuneClassifications.complement).SetEnergyFlow(condition);

			if (source != null)
				parentRune.GetAttachedRuneOfType(RuneClassifications.source).SetEnergyFlow(condition);

			if (transformation != null)
				parentRune.GetAttachedRuneOfType(RuneClassifications.transformation).SetEnergyFlow(condition);

		}

	}


}
