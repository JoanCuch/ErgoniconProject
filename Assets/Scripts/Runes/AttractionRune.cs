using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionRune : MinorRune
{
	[SerializeField] [ReadOnly] private AttractionRune twinRune;

	[SerializeField] private float detectionRadius;
	[SerializeField] private float impulseMultiplier;
	[SerializeField] private float distancePower;



	private Rigidbody attachedObjectRigidbody;
	
	// Start iscalled before the first frame update
    void Start()
	{

	}


    // Update is called once per frame
    void Update()
    {
       




		if (twinRune == null)
		{
			AttractionRune possibleRune = FindTwinRune();
			if(possibleRune != null)
			{
				Debug.Log("Creating attraction");
				this.SetTwinRune(possibleRune);
				possibleRune.SetTwinRune(this);
				//GameObject attachedObject = GetMajorRune().GetAttachedObject().gameObject;
				//GameObject twinOBject = twinRune.GetMajorRune().GetAttachedObject().gameObject;		
			}



		}
    }

	private void FixedUpdate()
	{

		if (attachedObjectRigidbody == null) attachedObjectRigidbody = GetMajorRune().GetAttachedObject().transform.GetComponent<Rigidbody>();


		if (twinRune != null)
		{
			Vector3 forceDirection = (twinRune.GetMajorRune().transform.position - this.GetMajorRune().transform.position).normalized;
			float distance = Vector3.Distance(twinRune.GetMajorRune().transform.position,this.GetMajorRune().transform.position);

			Vector3 force = forceDirection * (1 / Mathf.Pow(distance, distancePower) * impulseMultiplier);
			Vector3 startPosition = GetMajorRune().transform.position;

			attachedObjectRigidbody.AddForceAtPosition(force, startPosition);
		}
	}


	private AttractionRune FindTwinRune()
	{
		Collider[] hitColliders = Physics.OverlapSphere(parentMajorRune.transform.position, detectionRadius);

		AttractionRune otherRune = null;

		foreach (Collider col in hitColliders)
		{
			if (col.transform != this.transform && col.tag == transform.tag)
			{

				Debug.Log("possible twin");
				AttractionRune current = col.GetComponent<AttractionRune>();

				if (current.GetTwinRune() == null)
				{
					otherRune = current;
					Debug.Log("twin detected");
				}
			}
		}
		return otherRune;
	}

	public void SetTwinRune(AttractionRune twin)
	{
		twinRune = twin;
	}

	public AttractionRune GetTwinRune()
	{
		return twinRune;
	}



}
