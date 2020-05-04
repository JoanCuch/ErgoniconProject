using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointMinorRune : TransformationRune
{

	[SerializeField] float jointForce;
	private FixedJoint joint;
	[SerializeField] private float jointForceMultiplier;



	// Start is called before the first frame update
	protected override void Start()
	{
		joint = null;
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	private void LateUpdate()
	{
		
		if (joint == null && GetLinkedRune())
		{
			if (GetSource() == null)
			{
				SetSource(GetMajorRune().GetMinorRune(RuneClassifications.source));
			}
			else
			{
				AddEnergy(GetSource().AbsorbEnergy(GetFlowRate() * Time.deltaTime));
				float transformedEnergy = 0;
				transformedEnergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetTransformationEfficiency();

				if (transformedEnergy > 0)
				{
					CreateJoint();
				}
			}
		}

		if(joint != null && GetLinkedRune() == false)
		{
			DestroyJoint();
		}

		if (joint != null)
		{
			if(GetSource() == null)
			{
				SetSource(GetMajorRune().GetMinorRune(RuneClassifications.source));
			}
			else
			{
				AddEnergy(GetSource().AbsorbEnergy(GetFlowRate() * Time.deltaTime));
				float transformedEnergy = 0;
				transformedEnergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetTransformationEfficiency();

				//The transformed energy defines the current break force. Considering that the jointForce maximum gets when the transformed energy equals the maxFlowRate
				joint.breakForce = transformedEnergy * jointForce / GetFlowRate();

				if(transformedEnergy == 0)
				{ DestroyJoint(); }
			}
		}

	}

	private void OnJointBreak(float breakForce)
	{
		Debug.Log("joint broke");
	}

	private void OnDestroy()
	{
		DestroyJoint();
	}

	private void CreateJoint()
	{
		GameObject attachedObject = GetMajorRune().GetAttachedObject().gameObject;
		joint = attachedObject.AddComponent<FixedJoint>() as FixedJoint;
		joint.connectedBody = GetLinkedRune().GetMajorRune().GetAttachedObject().GetComponent<Rigidbody>();
		joint.breakForce = jointForce;
		Debug.Log("joint created");
	}

	public void DestroyJoint()
	{
		Destroy(joint);
		Debug.Log("joint broke");
	}


}
