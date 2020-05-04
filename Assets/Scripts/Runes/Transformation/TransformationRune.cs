using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationRune : MinorRune
{


	[SerializeField] float initialEfficiency;
	[SerializeField] float initialFlowRate;
	[SerializeField] float initialRange;

	[SerializeField] [ReadOnly] private bool inversed;
	[SerializeField] [ReadOnly] private float flowRate;
	[SerializeField] [ReadOnly] private float efficiency;
	[SerializeField] [ReadOnly] private float range;

	[SerializeField] [ReadOnly] private EnergyInteractable sourceRune;
	[SerializeField] [ReadOnly] private EnergyInteractable target;

	[SerializeField] [ReadOnly] private TransformationRune linkedRune;

    // Start is called before the first frame update
    protected override void Start()
	{
		base.Start();
		flowRate = initialFlowRate;
		efficiency = initialEfficiency;
		range = initialRange;
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
	public void SetChangeRange(float _rangeChange) { range *= _rangeChange;  }

	public bool GetInversed() { return inversed; }
	public void SetInversed(bool _newFlow) { inversed = _newFlow; }

	public float GetTransformationEfficiency() { return efficiency; }

	public TransformationRune GetLinkedRune() { return linkedRune; }
	public void SetLinkedRune(TransformationRune _newLinked) { linkedRune = _newLinked; }

	



}
