using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorRune : EnergyInteractable
{

	public enum RuneClassifications
	{
		source,
		transformation,
		complement,
		root,
		fakeRune
	}

	public enum RuneTypes
	{
		ambient,
		direct,
		thermic,
		kinetic,
		joint,
		range,
		inverse,
		flow,
		center,
		twin,
		destroy
	}

	[SerializeField] private RuneClassifications runeClassification;
	[SerializeField] private RuneTypes runeType;

	[SerializeField] protected MajorRune parentMajorRune;
	//[SerializeField] [ReadOnly] protected bool energyFlowInput;

	[SerializeField] private SpriteRenderer spriteRenderer;

	[SerializeField] private bool isWorkable;

	private float spriteUnitsWidth = 0;

	// Start is called before the first frame update
    protected override void Start()
    {
		base.Start();
		//energyFlowInput = true;
		isWorkable = false;
	}

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();

		if (parentMajorRune == null)
		{
			Debug.LogWarning("Null major rune on: " + this.name);
		}
	
		//EnergyInteractable reverseRune = GetMajorRune().get
	}

	public RuneClassifications GetRuneClassification()
	{
		return runeClassification;
	}
	public RuneTypes GetRuneType()
	{
		return runeType;
	}


	public void SetMajorRune(MajorRune newParent)
	{
		parentMajorRune = newParent;
	}
	public MajorRune GetMajorRune()
	{
		return parentMajorRune;
	}

	/*public void SetEnergyFlow(bool trueForInputEnergy)
	{
		energyFlowInput = trueForInputEnergy;
	}*/


	private void SetSpriteWidth()
	{
		float spriteWith = spriteRenderer.sprite.bounds.size.x;
		//loat pixelPerUni = spriteRenderer.sprite.pixelsPerUnit;
		//float unitsWidth = spriteWith / pixelPerUni * transform.lossyScale.x;
		spriteUnitsWidth = spriteWith * transform.localScale.x;
	}


	public float GetSpriteWidth()
	{
		if (spriteUnitsWidth == 0)
			SetSpriteWidth();

		return spriteUnitsWidth;

	}

	public void SetWorkable(bool _isWorkable) { isWorkable = _isWorkable; }
	public bool GetWorkable () { return isWorkable; }
}

