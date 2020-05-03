using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringRune : MinorRune
{

	[SerializeField] private float detectionRadius;
	[SerializeField] private float springForce;

	private SpringRune twinRune;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	
  
		if (twinRune == null)
		{
			twinRune = FindTwinRune();

			//If a twin rune has been found, create the spring joint
			if(twinRune != null)
			{
				Debug.Log("creating joint");


				GameObject attachedObject = GetMajorRune().GetAttachedObject().gameObject;
				attachedObject.AddComponent<SpringJoint>();
				attachedObject.GetComponent<SpringJoint>().connectedBody = twinRune.GetMajorRune().GetAttachedObject().GetComponent<Rigidbody>();
				attachedObject.GetComponent<SpringJoint>().spring = springForce;
				twinRune.SetTwinRune(this);
			}
		}
    }



	private void OnJointBreak(float breakForce)
	{
		twinRune = null;
		Debug.Log("breaking joint");
	}

	private SpringRune FindTwinRune()
	{
		Collider[] hitColliders = Physics.OverlapSphere(parentMajorRune.transform.position, detectionRadius);

		EnergyInteractable otherRune = null;

		foreach(Collider col in hitColliders)
		{
			if(col.transform != this.transform && col.tag == "STRINGMINORRUNE")
			{

				Debug.Log("possible twin");
				SpringRune current = col.GetComponent<SpringRune>();

				if (current.GetTwinRune() == null)
				{
					otherRune = current;
					Debug.Log("twin detected");
				}
			}
		}
	
		return (SpringRune)otherRune;
	}

	public void SetTwinRune(SpringRune twin)
	{
		twinRune = twin;
	}

	public SpringRune GetTwinRune()
	{
		return twinRune;
	}



}
