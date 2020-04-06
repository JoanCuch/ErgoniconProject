using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorRune : EnergyInteractable
{

	public enum RuneTypes { source, transformation, complement, basic}
	[SerializeField]private RuneTypes runeType;

	[SerializeField] [ReadOnly]protected MajorRune parentMajorRune;
	[SerializeField] [ReadOnly] protected bool energyFlowInput;



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

	


}
