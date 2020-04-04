using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : EnergyInteractable
{


	private List<MajorRune> attachedRunes;

	public Material ownMaterial;
	private Renderer renderer;

	private float origianlIntensity;


    // Start is called before the first frame update
    void Start()
    {
		attachedRunes = new List<MajorRune>();
		ownMaterial = GetComponent<MeshRenderer>().material;
    }
	

    // Update is called once per frame
    void Update()
    {
    }

	private void LateUpdate()
	{
		ModifyEmision();
	}

	public bool hasRune()
	{
		bool _hasRune = false;

		if (attachedRunes.Count != 0)
			_hasRune = true;

		return _hasRune;
	}


	public void AttachMajorRune(Transform _newRune)
	{
		_newRune.SetParent(transform);
		attachedRunes.Add(_newRune.GetComponent<MajorRune>());
	}
	public void RemoveMajorRune(Transform _oldRune)
	{
		attachedRunes.Remove(_oldRune.GetComponent<MajorRune>());
		Destroy(_oldRune.gameObject);

	}

	//This function goes on the rune
	private void ModifyEmision()
	{
		Color newColor = ownMaterial.GetColor("_EmissionColor") * (GetHeat()*0.001f+1);
		ownMaterial.SetColor("_EmissionColor", newColor);

	}



}
