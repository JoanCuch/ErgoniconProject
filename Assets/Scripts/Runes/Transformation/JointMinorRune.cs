using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointMinorRune : TransformationRune
{

	[SerializeField] float jointForce;
	private Vector3 allowedAxisRotation;
	private Joint joint;

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
		
		if (joint == null)
		{
			//If there isn't any joint but there is a linked twin --> Create joint if there is energy
			if (GetLinkedRune())
			{
				if (GetSource() == null)
				{
					SetSource(GetMajorRune().GetMinorRune(RuneClassifications.source));
				}
				else
				{
					AddEnergy(GetSource().AbsorbEnergy(GetFlowRate() * Time.deltaTime));
					float transformedEnergy = 0;
					transformedEnergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetEfficiency();

					if (transformedEnergy > 0)
					{
						CreateJoint();
					}
				}
			}
		}
		else	
		{
			if (GetInversed() == true  && joint.GetType() == typeof(FixedJoint) ||
				GetInversed() == false && joint.GetType() == typeof(HingeJoint) )
			{
				//If the joint is incorrect, reinstall
				DestroyJoint();
				CreateJoint();
			}
			else if (GetLinkedRune() == false)
			{
				//If there is a joint but not a twin anymore --> destroy joint
				DestroyJoint();
			}
			else
			{
				//If the joint still exists update the break force.
				if (GetSource() == null)
				{
					SetSource(GetMajorRune().GetMinorRune(RuneClassifications.source));
				}
				else
				{
					AddEnergy(GetSource().AbsorbEnergy(GetFlowRate() * Time.deltaTime));
					float transformedEnergy = 0;
					transformedEnergy = AbsorbEnergy(GetFlowRate() * Time.deltaTime) * GetEfficiency();

					float residualEnergy = transformedEnergy * (1 - GetEfficiency());
					transformedEnergy -= residualEnergy;

					if (GetAttachedEnvironment() == null)
					{
						GetMajorRune().GetAttachedObject().AddEnergy(residualEnergy);
					}
					else
					{
						//The residual Energy that goes to the object
						GetMajorRune().GetAttachedObject().AddEnergy(residualEnergy / 2);
						//The residual Energy that goes to the environment
						GetAttachedEnvironment().AddEnergy(residualEnergy / 2);
					}


					//The transformed energy defines the current break force. Considering that the jointForce maximum gets when the transformed energy equals the maxFlowRate
					joint.breakForce = transformedEnergy * jointForce / GetFlowRate();

					if (transformedEnergy == 0)
					{ DestroyJoint(); }
				}
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

		if (GetInversed())
		{
			//If there is an inverse rune, create hinge joint with Y axis rotation
			HingeJoint tempJoint = attachedObject.AddComponent<HingeJoint>() as HingeJoint;
			tempJoint.axis = (GetMajorRune().transform.position - GetLinkedRune().GetMajorRune().transform.position).normalized;
			joint = tempJoint;
			Debug.Log("Hinge joint created. Rotation allowed");

		}
		else
		{
			//By default, create a fixerd joint with no movement or rotation allowed
			joint = attachedObject.AddComponent<FixedJoint>() as FixedJoint;	
			Debug.Log("Fixed joint created");
		}

		joint.connectedBody = GetLinkedRune().GetMajorRune().GetAttachedObject().GetComponent<Rigidbody>();
		joint.breakForce = jointForce;


	}

	private void DestroyJoint()
	{
		Destroy(joint);
		Debug.Log("joint broke");
	}


}
