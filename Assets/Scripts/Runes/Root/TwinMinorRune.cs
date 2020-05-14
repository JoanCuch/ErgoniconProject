using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMinorRune : MinorRune
{

	[SerializeField] private TwinMinorRune linkedTwinRune;
	[SerializeField] private TransformationRune linkedTransformationRune;
	[SerializeField] private TransformationRune ownTransformationRune;

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
		else if (linkedTransformationRune != null &&
			GetOwnTransformationRune() != null &&
			linkedTransformationRune.GetType() != GetOwnTransformationRune().GetType()
			)
		{
			linkedTwinRune.RemoveLink();
			RemoveLink();
		}
		else
		{
			if(linkedTransformationRune == null)
			{
				linkedTransformationRune = linkedTwinRune.GetOwnTransformationRune();

				if(linkedTransformationRune != null &&
					ownTransformationRune != null)
				{
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

				//The two twins runes needs to be attached to a transformation rune
				if (!(possibleTwin == null ||
					possibleTwin.GetOwnTransformationRune() == null ||
					this.GetOwnTransformationRune() == null))
				{
					//bool TempBoolA = possibleTwin.GetLinkedTwinRune() == null;
					//bool tempBoolB = GetOwnTransformationRune().GetType() == possibleTwin.GetOwnTransformationRune().GetType();

					//If the other twin rune is not linked yet and they are the same type
					if (possibleTwin.GetLinkedTwinRune() == null &&
						GetOwnTransformationRune().GetType() == possibleTwin.GetOwnTransformationRune().GetType())
					{
						linkedRune = possibleTwin;
						Debug.Log("twin selected with the same rune type");
					}
				}
			}
		}
		return linkedRune;
	}

	private void OnDestroy()
	{
		linkedTwinRune.RemoveLink();
		RemoveLink();
	}


	public TwinMinorRune GetLinkedTwinRune() { return linkedTwinRune; }
	public TransformationRune GetOwnTransformationRune() { return ownTransformationRune; }

	public void SetLinkedTwinRune(TwinMinorRune _newTwin) {
		linkedTwinRune = _newTwin;


	}


	public void RemoveLink()
	{
		if (linkedTransformationRune != null)
		{
			linkedTransformationRune = null;
			ownTransformationRune.SetLinkedRune(null);		
			linkedTwinRune = null;
			Debug.Log("removing twin link");
		}
	}
}
