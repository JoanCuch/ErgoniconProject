using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceRune : MinorRune
{

	[SerializeField]  private float range;
	[SerializeField]  private float flowRate;
	[SerializeField] [ReadOnly]  private bool flowDirection;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		flowDirection = true;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public bool GetFlowDirection(){ return flowDirection; }
	public float GetFlowRate() { return flowRate; }
	public float GetRange() { return range; }

	public void ChangeRange(float _rangeChange)
	{
		range *= _rangeChange;
	}

	public void ChangeFlowRate(float _flowChange)
	{
		flowRate *= _flowChange;
	}

	public void SetFlowDirection(bool _newFlow)
	{
		flowDirection = _newFlow;
	}

}
