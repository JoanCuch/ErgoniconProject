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
		basic,
		noRune
	}

	public enum RuneTypes
	{
		basic,
		inverse,
		physicalObject,
		ambient,
		direct,
		extra,
		twin,
		heat,
		force,
		destroy
	}

	[SerializeField]private RuneClassifications runeClassification;
	[SerializeField] private RuneTypes runeType;

	[SerializeField] [ReadOnly]protected MajorRune parentMajorRune;
	[SerializeField] [ReadOnly] protected bool energyFlowInput;

	[SerializeField] private SpriteRenderer spriteRenderer;

	private float spriteUnitsWidth = 0;

	// Start is called before the first frame update
    protected virtual void Start()
    {
		energyFlowInput = true;
	}

    // Update is called once per frame
    void Update()
    {
        
		//EnergyInteractable reverseRune = GetMajorRune().get



    }


	public virtual void UpdateEnergy()
	{
		Debug.LogWarning("This minor rune is doing nothing");
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

	public void SetEnergyFlow(bool trueForInputEnergy)
	{
		energyFlowInput = trueForInputEnergy;
	}


	private void SetSpriteWidth()
	{
		float spriteWith = spriteRenderer.sprite.bounds.size.x;
		//loat pixelPerUni = spriteRenderer.sprite.pixelsPerUnit;
		//float unitsWidth = spriteWith / pixelPerUni * transform.lossyScale.x;
		spriteUnitsWidth = spriteWith * transform.localScale.x;

		Debug.Log(this.name + " --> spriteWith: " + spriteUnitsWidth + ", localScale: " + transform.localScale.x);
	}


	public float GetSpriteWidth()
	{
		if (spriteUnitsWidth == 0)
			SetSpriteWidth();

		return spriteUnitsWidth;

	}
}

