using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMinorRune : MinorRune
{

	[SerializeField] [ReadOnly] private TwinMinorRune linkedTwinRune;
	[SerializeField] [ReadOnly] private TransformationRune linkedTransformationRune;
	[SerializeField] [ReadOnly] private TransformationRune ownTransformationRune;

	[SerializeField] private float detectionRadius;


	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		if (!GetWorkable())
			return;
		//Get the transformation rune of the same major rune
		if (ownTransformationRune == null)
		{
			ownTransformationRune = (TransformationRune)GetMajorRune().GetMinorRune(RuneClassifications.transformation);
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
			}
		}
		else
		{
			if(linkedTransformationRune == null)
			{
				linkedTransformationRune = linkedTwinRune.GetOwnTransformationRune();

				//TODO superharcoded. This should be a subclasse
				if(linkedTransformationRune != null &&
					ownTransformationRune != null)
				{
					//ForceMinorRune forceRune = (ForceMinorRune)ownTransformationRune;
					//forceRune.SetForceTarget(linkedTransformationRune.transform);
					ownTransformationRune.SetLinkedRune(linkedTransformationRune);
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
				Debug.Log(possibleTwin.name + " " + possibleTwin);
				bool TempBoolA = possibleTwin.GetLinkedTwinRune() == null;
				bool tempBoolB = GetOwnTransformationRune().GetType() == possibleTwin.GetOwnTransformationRune().GetType();


				if (possibleTwin.GetLinkedTwinRune() == null &&
					GetOwnTransformationRune().GetType() == possibleTwin.GetOwnTransformationRune().GetType())
				{
					linkedRune = possibleTwin;
					Debug.Log("twin selected with the same rune type");
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
			ownTransformationRune.SetLinkedRune(null);


			/*
			if (ownTransformationRune.GetRuneType() == RuneTypes.force)
			{
				ForceMinorRune forceRune = (ForceMinorRune)ownTransformationRune;
				forceRune.SetForceTarget(null);
			}*/
		}
	}


	public TwinMinorRune GetLinkedTwinRune() { return linkedTwinRune; }
	public TransformationRune GetOwnTransformationRune() { return ownTransformationRune; }

	public void SetLinkedTwinRune(TwinMinorRune _newTwin) {
		linkedTwinRune = _newTwin;


	}


}
