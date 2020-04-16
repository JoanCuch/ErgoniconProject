using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationRune : MinorRune
{

	[SerializeField]  private float flowRate;
	[SerializeField]  private float transformationEfficiency;
	[SerializeField]  [ReadOnly]private bool flowDirection;


	[SerializeField] [ReadOnly] private EnergyInteractable sourceRune;
	[SerializeField] [ReadOnly] private EnergyInteractable target;

	[SerializeField] [ReadOnly] private TransformationRune linkedRune;

    // Start is called before the first frame update
    protected override void Start()
	{
		base.Start();
		SetFlowDirection(true);
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}


	public EnergyInteractable GetSource() { return sourceRune; }
	public void SetSource(EnergyInteractable _newSource) { sourceRune = _newSource; }

	public EnergyInteractable GetTarget() { return target; }
	public void SetTarget(EnergyInteractable _newTarget) { target = _newTarget; }

	public float GetFlowRate() { return flowRate; }
	public void ChangeFlowRate(float _flowChange) { flowRate *= _flowChange; }

	public bool GetFlowDirection() { return flowDirection; }
	public void SetFlowDirection(bool _newFlow) { flowDirection = _newFlow; }

	public float GetTransformationEfficiency() { return transformationEfficiency; }

	public TransformationRune GetLinkedRune() { return linkedRune; }
	public void SetLinkedRune(TransformationRune _newLinked) { linkedRune = _newLinked; }

	



}
