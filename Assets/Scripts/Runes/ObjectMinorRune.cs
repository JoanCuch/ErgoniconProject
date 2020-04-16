using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. Get the energy from the attached object
/// </summary>
public class ObjectMinorRune : SourceRune
{
	[SerializeField] [ReadOnly] EnergyInteractable source;
	//[SerializeField] private float energyFlow;

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

		if (source == null)
		{
			source = parentMajorRune.GetAttachedObject();
		}
		else
		{
			if (GetFlowDirection())
			{
				float newE = source.AbsorbEnergy(GetFlowRate() * Time.deltaTime);

				AddEnergy(newE);
			}
			else
			{
				float newE = AbsorbEnergy(GetFlowRate() * Time.deltaTime);

				source.AddEnergy(newE);
			}
		}		
	}	
}