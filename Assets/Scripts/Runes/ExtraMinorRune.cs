using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMinorRune : MinorRune
{

	private RuneClassifications targetClassifications;

	private MinorRune poweredRune;
	[SerializeField] private float rateFlowMultiplier;

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

		if (poweredRune == null)
		{
			if(targetClassifications == RuneClassifications.source)
			{
				SourceRune newPow = (SourceRune)GetMajorRune().GetMinorRune(targetClassifications);
				if(newPow != null)
				{
					newPow.ChangeFlowRate(rateFlowMultiplier);
					poweredRune = newPow;
				}
			}
			else if (targetClassifications == RuneClassifications.transformation)
			{
				TransformationRune newPow = (TransformationRune)GetMajorRune().GetMinorRune(targetClassifications);
				if (newPow != null)
				{
					newPow.ChangeFlowRate(rateFlowMultiplier);
					poweredRune = newPow;
				}
			}		
		}
	}

	private void OnDestroy()
	{
		if (poweredRune != null)
		{
			if (targetClassifications == RuneClassifications.source)
			{
				SourceRune newPow = (SourceRune)poweredRune;
				if (newPow != null)
				{
					newPow.ChangeFlowRate(1/rateFlowMultiplier);
				}
			}
			else if (targetClassifications == RuneClassifications.transformation)
			{
				TransformationRune newPow = (TransformationRune)poweredRune;
				if (newPow != null)
				{
					newPow.ChangeFlowRate(1/rateFlowMultiplier);
				}
			}
		}
	}

	public void SetTargetClassification (RuneClassifications _classification)
	{
		targetClassifications = _classification;
	}

	public RuneClassifications GetTargetClassification()
	{
		return targetClassifications;
	}


}
