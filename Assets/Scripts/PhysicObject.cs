using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicObject : EnergyInteractable
{
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private Material highlightMaterial;

	private List<MajorRune> attachedRunes;

	public Material ownMaterial;
	private Renderer render;

	private float origianlIntensity;
	private float lastHeat;

	[SerializeField] private float minHeat;
	[SerializeField] private float maxHeat;

	[SerializeField] private Color minHeatColor;
	[SerializeField] private Color maxHeatColor;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		meshRenderer = GetComponent<MeshRenderer>();
		attachedRunes = new List<MajorRune>();
		ownMaterial = GetComponent<MeshRenderer>().material;
		lastHeat = GetHeat();


		//temp hardcoded
		AddEnergy(10000);
    }


	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	private void LateUpdate()
	{
		if (lastHeat != GetHeat()) ModifyEmision();
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
		//The object has a max heat that can not give more light. The color lerp needs a color between 0 and 1
		//float colorHeatNumber = Mathf.Clamp(GetHeat(), minHeat, maxHeat);
		float colorHeatNumber = Mathf.InverseLerp(minHeat, maxHeat, GetHeat());
		Color newColor = Color.Lerp(minHeatColor, maxHeatColor, colorHeatNumber);
		ownMaterial.SetColor("_EmissionColor", newColor);

		//Color newColor = ownMaterial.GetColor("_EmissionColor") * (GetHeat()*0.001f+1);
		//ownMaterial.SetColor("_EmissionColor", newColor);
	}

	public void SetActiveHightlight(bool _active)
	{
		if (_active)
		{
			List<Material> mats = new List<Material>{ meshRenderer.material, highlightMaterial };
			meshRenderer.materials = mats.ToArray();
		}
		else
		{
			List<Material> mats = new List<Material> { ownMaterial };
			meshRenderer.materials = mats.ToArray();
		}



	}

}
