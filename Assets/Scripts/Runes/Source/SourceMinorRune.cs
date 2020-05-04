using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceMinorRune : MinorRune
{

	[SerializeField]  private float range;
	[SerializeField]  private float flowRate;
	[SerializeField] [ReadOnly]  private bool inversed;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		inversed = false;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public bool GetInversed() { return inversed; }
	public float GetFlowRate() { return flowRate; }
	public float GetRange() { return range; }

	public void SetChangeRange(float _rangeChange)
	{
		range *= _rangeChange;
	}
	public void SetChangeFlowRate(float _flowChange)
	{
		flowRate *= _flowChange;
	}
	public void SetInversed(bool _newFlow)
	{
		inversed = _newFlow;
	}

}
