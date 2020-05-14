using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceMinorRune : MinorRune
{
	[SerializeField] float initialEfficiency;
	[SerializeField] float initialFlowRate;
	[SerializeField] float initialRange;

	[SerializeField] private bool inversed;
	[SerializeField] private float flowRate;
	[SerializeField] private float efficiency;
	[SerializeField] private float range;

	[SerializeField] private EnergyInteractable environment;
	[SerializeField] private float sourceUpdateDelay;
	[SerializeField] private string environmentTag;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		inversed = false;
		flowRate = initialFlowRate;
		efficiency = initialEfficiency;
		range = initialRange;

		StartCoroutine(UpdateEnvironment());
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public bool GetInversed() { return inversed; }
	public float GetFlowRate() { return flowRate; }
	public float GetRange() { return range; }
	public float GetEfficiency() { return efficiency; }

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
	public void SetEfficienty(float _efficiencyChange)
	{
		efficiency *= _efficiencyChange;
	}

	public EnergyInteractable GetAttachedEnvironment(){ return environment; }
	protected EnergyInteractable FindEnvironmentAround()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetRange());

		EnergyInteractable envi = null;


		foreach (Collider col in hitColliders)
		{
			if (col.transform.tag == environmentTag)
			{
				envi = col.GetComponent<EnergyInteractable>();
				break;
			}
		}

		return envi;
	}

	IEnumerator UpdateEnvironment()
	{
		while (true)
		{
			if (GetWorkable())
			{
				environment = FindEnvironmentAround();
			}
			yield return new WaitForSeconds(sourceUpdateDelay);
		}
	}

}

