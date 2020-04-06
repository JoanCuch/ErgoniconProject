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
		
		parentRune.GetAttachedRuneOfType(RuneTypes.basic).SetEnergyFlow(false);
		parentRune.GetAttachedRuneOfType(RuneTypes.complement).SetEnergyFlow(false);
		parentRune.GetAttachedRuneOfType(RuneTypes.source).SetEnergyFlow(false);
		parentRune.GetAttachedRuneOfType(RuneTypes.transformation).SetEnergyFlow(false);
	}

	private void OnDestroy()
	{

		if (parentRune == null)
		{
			parentRune = GetMajorRune();
		}
		parentRune.GetAttachedRuneOfType(RuneTypes.basic).SetEnergyFlow(true);
		parentRune.GetAttachedRuneOfType(RuneTypes.complement).SetEnergyFlow(true);
		parentRune.GetAttachedRuneOfType(RuneTypes.source).SetEnergyFlow(true);
		parentRune.GetAttachedRuneOfType(RuneTypes.transformation).SetEnergyFlow(true);
	}


}
