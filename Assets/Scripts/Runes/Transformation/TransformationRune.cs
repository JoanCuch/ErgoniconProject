using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationRune : MinorRune
{

	[SerializeField]  private float flowRate;
	[SerializeField]  private float transformationEfficiency;
	[SerializeField]  private float range;
	[SerializeField]  [ReadOnly]private bool inversed;


	[SerializeField] [ReadOnly] private EnergyInteractable sourceRune;
	[SerializeField] [ReadOnly] private EnergyInteractable target;

	[SerializeField] [ReadOnly] private TransformationRune linkedRune;

    // Start is called before the first frame update
    protected override void Start()
	{
		base.Start();
		SetInversed(false);
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
	public void SetChangeFlowRate(float _flowChange) { flowRate *= _flowChange; }

	public float GetRange() { return range; }
	public void SetChangeRange(float _rangeChange) {  }

	public bool GetInversed() { return inversed; }
	public void SetInversed(bool _newFlow) { inversed = _newFlow; }

	public float GetTransformationEfficiency() { return transformationEfficiency; }

	public TransformationRune GetLinkedRune() { return linkedRune; }
	public void SetLinkedRune(TransformationRune _newLinked) { linkedRune = _newLinked; }

	



}
