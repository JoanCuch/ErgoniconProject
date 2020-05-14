using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplementMinorRune : MinorRune
{

	private RuneClassifications targetClassifications;
	[SerializeField] private MinorRune poweredRune;


	//private MinorRune poweredRune;
	//[SerializeField] private float rateFlowMultiplier;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		StartCoroutine(RetardedActivate());
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();

		if (!GetWorkable())
			return;

		if (poweredRune == null)
		{
			poweredRune = GetMajorRune().GetMinorRune(targetClassifications);
		}





		/*if (poweredRune == null)
		{




			if (targetClassifications == RuneClassifications.source)
			{
				SourceMinorRune newPow = (SourceMinorRune)GetMajorRune().GetMinorRune(targetClassifications);
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
		}*/
	}

	/*private void OnDestroy()
	{
		if (poweredRune != null)
		{
			if (targetClassifications == RuneClassifications.source)
			{
				SourceMinorRune newPow = (SourceMinorRune)poweredRune;
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
	}*/
	private void OnDestroy()
	{
		ActivateComplement(false);
	}


	public void SetTargetClassification (RuneClassifications _classification) { targetClassifications = _classification; }
	public RuneClassifications GetTargetClassification() { return targetClassifications; }

	public MinorRune GetPoweredRune() { return poweredRune; }
	
	public virtual void ActivateComplement(bool _condition)
	{

	}

	IEnumerator RetardedActivate()
	{
		while(GetPoweredRune() == null)
		{
			yield return null;
		}

		ActivateComplement(true);
	}

}
