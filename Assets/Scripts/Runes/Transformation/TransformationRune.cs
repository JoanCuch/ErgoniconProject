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

	[SerializeField] [ReadOnly] private EnergyInteractable environment;
	[SerializeField] private float sourceUpdateDelay;
	[SerializeField] [TagSelector] private string environmentTag;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		flowRate = initialFlowRate;
		efficiency = initialEfficiency;
		range = initialRange;
		SetInversed(false);

		StartCoroutine(UpdateEnvironment());
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public EnergyInteractable GetSource() { return sourceRune; }
	public EnergyInteractable GetTarget() { return target; }
	public float GetFlowRate() { return flowRate; }
	public float GetRange() { return range; }
	public bool GetInversed() { return inversed; }
	public float GetEfficiency() { return efficiency; }
	public TransformationRune GetLinkedRune() { return linkedRune; }

	public void SetSource(EnergyInteractable _newSource) { sourceRune = _newSource; }	
	public void SetTarget(EnergyInteractable _newTarget) { target = _newTarget; }
	public void SetChangeFlowRate(float _flowChange) { flowRate *= _flowChange; }
	public void SetChangeRange(float _rangeChange) { range *= _rangeChange;  }
	public void SetInversed(bool _newFlow) { inversed = _newFlow; }
	public void SetLinkedRune(TransformationRune _newLinked) { linkedRune = _newLinked; }
	public void SetEfficiency(float _efficiencyChange) { efficiency *= _efficiencyChange; }

	public EnergyInteractable GetAttachedEnvironment() { return environment; }
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
