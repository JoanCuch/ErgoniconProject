using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMinorRune : MinorRune
{

	[SerializeField] [ReadOnly] private TwinMinorRune linkedTwinRune;
	[SerializeField] [ReadOnly] private MinorRune linkedTransformationRune;
	[SerializeField] [ReadOnly] private MinorRune ownTransformationRune;

	[SerializeField] private float detectionRadius;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
    {
		//Get the transformation rune of the same major rune
		if(ownTransformationRune == null)
		{
			ownTransformationRune = GetMajorRune().GetMinorRune(RuneClassifications.transformation);
		}

		if (linkedTwinRune == null)
		{
			//If there is no twin rune, search for une
			TwinMinorRune possibleRune = FindTwinRune();
			if (possibleRune != null)
			{
				Debug.Log("Twin linked");
				this.SetLinkedTwinRune(possibleRune);
				possibleRune.SetLinkedTwinRune(this);
				//GameObject attachedObject = GetMajorRune().GetAttachedObject().gameObject;
				//GameObject twinOBject = twinRune.GetMajorRune().GetAttachedObject().gameObject;		
			}
		}
		else
		{
			if(linkedTransformationRune == null)
			{
				linkedTransformationRune = linkedTwinRune.GetOwnTransformationRune();

				//TODO superharcoded. This should be a subclasse
				if(linkedTransformationRune != null &&
					ownTransformationRune != null &&
					ownTransformationRune.GetRuneType() == RuneTypes.force)
				{
					ForceMinorRune forceRune = (ForceMinorRune)ownTransformationRune;
					forceRune.SetForceTarget(linkedTransformationRune.transform);
				}
			}
		}
	}

	private TwinMinorRune FindTwinRune()
	{
		Collider[] hitColliders = Physics.OverlapSphere(GetMajorRune().transform.position, detectionRadius);

		TwinMinorRune linkedRune = null;

		foreach (Collider col in hitColliders)
		{

			if (col.transform != this.transform && col.tag == transform.tag)
			{
			
				TwinMinorRune possibleTwin = col.GetComponent<TwinMinorRune>();

				if (possibleTwin.GetLinkedTwinRune() == null)
				{
					linkedRune = possibleTwin;
					Debug.Log("twin selected");
				}
			}
		}
		return linkedRune;
	}

	private void OnDestroy()
	{
		if (linkedTransformationRune != null)
		{
			//TODO superharcoded. This should be a subclasse
			if (ownTransformationRune.GetRuneType() == RuneTypes.force)
			{
				ForceMinorRune forceRune = (ForceMinorRune)ownTransformationRune;
				forceRune.SetForceTarget(null);
			}
		}
	}


	public MinorRune GetLinkedTwinRune() { return linkedTwinRune; }
	public MinorRune GetOwnTransformationRune() { return ownTransformationRune; }

	public void SetLinkedTwinRune(TwinMinorRune _newTwin) {
		linkedTwinRune = (TwinMinorRune)_newTwin;


	}


}
