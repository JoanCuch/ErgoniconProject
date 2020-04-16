using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : EnergyInteractable
{
    
	[SerializeField] private float startEnergy;
	
	// Start is called before the first frame update
    protected override void Start()
    {
		AddEnergy(startEnergy);
    }

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

}
