using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the attached object
/// </summary>
public class ObjectMinorRune : MinorRune
{
	[SerializeField] [ReadOnly] EnergyInteractable source;
	[SerializeField] private float energyFlow;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

	}

	// Update is called once per frame
	void Update()
	{
		if (parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

		if (energyFlowInput)
		{
		
			//Get the energy from the object and add it to himself
			if (source == null)
			{
				source = parentMajorRune.GetAttachedObject();
			}
			else
			{
				float newE = source.AbsorbEnergy(energyFlow * Time.deltaTime);

				AddEnergy(newE);
			}
		}
		else
		{
			//Get the energy from himself and add it to the object
			if (source == null)
			{
				source = parentMajorRune.GetAttachedObject();
			}
			else
			{
				float newE = AbsorbEnergy(energyFlow * Time.deltaTime);

				source.AddEnergy(newE);
			}
		}
	}	
}