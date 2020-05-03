using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMinorRune : ComplementMinorRune
{
	protected override void Start()
	{
		base.Start();
		
	}

	protected override void Update()
	{
		base.Update();
		ActivateComplement(true);
	}

	private void OnDestroy()
	{
		ActivateComplement(false);
	}

	public override void ActivateComplement(bool _condition)
	{
		MinorRune powered = GetPoweredRune();

		if (powered)
		{
			if (powered.GetRuneClassification() == RuneClassifications.transformation)
			{
				TransformationRune pow = (TransformationRune)powered;
				pow.SetInversed(_condition);
			}
			else if (powered.GetRuneClassification() == RuneClassifications.source)
			{
				SourceMinorRune pow = (SourceMinorRune)powered;
				pow.SetInversed(_condition);
			}
			else
			{
				Debug.LogWarning("The inverse rune is attached to a rune that isn't source or transformation");
			}
		}
	}

	IEnumerator RetardedActivate()
	{
		yield return new WaitUntil(() => GetPoweredRune() != null);
		ActivateComplement(true);

	}



}
