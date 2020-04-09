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

		EnergyInteractable basic = parentRune.GetAttachedRuneOfType(RuneTypes.basic);
		EnergyInteractable complement = parentRune.GetAttachedRuneOfType(RuneTypes.complement);
		EnergyInteractable source = parentRune.GetAttachedRuneOfType(RuneTypes.source);
		EnergyInteractable transformation = parentRune.GetAttachedRuneOfType(RuneTypes.transformation);

		if(basic != null)
			parentRune.GetAttachedRuneOfType(RuneTypes.basic).SetEnergyFlow(condition);

		if(complement != null)
			parentRune.GetAttachedRuneOfType(RuneTypes.complement).SetEnergyFlow(condition);

		if(source != null)
			parentRune.GetAttachedRuneOfType(RuneTypes.source).SetEnergyFlow(condition);

		if(transformation != null)
			parentRune.GetAttachedRuneOfType(RuneTypes.transformation).SetEnergyFlow(condition);



	}


}
