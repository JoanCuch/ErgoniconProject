using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMinorRune : ComplementMinorRune
{
	[SerializeField] private float rangeMultiplier;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ActivateComplement(bool _condition)
	{
		float multiplier = _condition ? (rangeMultiplier) : (1 / rangeMultiplier);
		
		MinorRune powered = GetPoweredRune();

		if (powered)
		{
			if (powered.GetRuneClassification() == RuneClassifications.transformation)
			{
				TransformationRune pow = (TransformationRune)powered;
				pow.SetChangeRange(multiplier);
			}
			else if (powered.GetRuneClassification() == RuneClassifications.source)
			{
				SourceMinorRune pow = (SourceMinorRune)powered;
				pow.SetChangeRange(multiplier);
			}
			else
			{
				Debug.LogWarning("The inverse rune is attached to a rune that isn't source or transformation");
			}
		}
	}
}
