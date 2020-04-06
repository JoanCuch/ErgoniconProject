using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the attached object
/// </summary>
public class ObjectMinorRune : MinorRune
{
	[SerializeField] [ReadOnly] EnergyInteractable source;
	[Space]
	[SerializeField] private float energyFlow;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{




		if (parentMajorRune == null)
		{
			Debug.LogWarning("null major rune, aaaaaaarh! Kaos!");
		}

		//Get the energy from the source and add it to himself
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
}