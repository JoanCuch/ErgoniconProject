﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorRune : EnergyInteractable
{

	public enum RuneClassifications
	{
		source,
		transformation,
		complement,
		basic
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
		force
	}
	[SerializeField]private RuneClassifications runeClassification;
	[SerializeField] private RuneTypes runeType;

	[SerializeField] [ReadOnly]protected MajorRune parentMajorRune;
	[SerializeField] [ReadOnly] protected bool energyFlowInput;

	private float spriteWith;
	[SerializeField] private SpriteRenderer spriteRenderer;


	// Start is called before the first frame update
    protected virtual void Start()
    {
		energyFlowInput = true;
	
		spriteWith = spriteRenderer.sprite.rect.width;
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

	public float GetSpriteWidth()
	{
		spriteWith = spriteRenderer.sprite.rect.width;
		float pixelPerUni = spriteRenderer.sprite.pixelsPerUnit;
		return spriteWith/pixelPerUni * transform.localScale.x;
	}
	


}
