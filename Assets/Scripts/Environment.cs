using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : EnergyInteractable
{
    
	[SerializeField] private float startEnergy;
	
	// Start is called before the first frame update
    void Start()
    {
		AddEnergy(startEnergy);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
