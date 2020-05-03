using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMinorRune : ComplementMinorRune
{

	[SerializeField] private float flowMultiplier;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	private void OnDestroy()
	{
		ActivateComplement(false);
	}

	public override void ActivateComplement(bool _condition)
	{
		float multiplier = _condition ? (flowMultiplier) : (1 / flowMultiplier);

		MinorRune powered = GetPoweredRune();

		if (powered && powered.GetRuneClassification() == RuneClassifications.transformation)
		{
			TransformationRune pow = (TransformationRune)powered;
			pow.SetChangeFlowRate(multiplier);
		}
		else if (powered && powered.GetRuneClassification() == RuneClassifications.source)
		{
			SourceMinorRune pow = (SourceMinorRune)powered;
			pow.SetChangeFlowRate(multiplier);
		}
		else
		{
			Debug.LogWarning("The inverse rune is attached to a rune that isn't source or transformation");
		}
	}
}
