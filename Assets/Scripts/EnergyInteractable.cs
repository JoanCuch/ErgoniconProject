using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyInteractable : MonoBehaviour
{

	//

	[ReadOnly] [SerializeField] private float _heat;
	[ReadOnly] [SerializeField] private float _energy;
	[ReadOnly] [SerializeField] private Vector3 force;
	[ReadOnly] [SerializeField] private float _fatigue;

	//[SerializeField] private float coeffHeatResistance = 1;
	//[SerializeField] private float coeffForceResistance = 1;
	//[SerializeField] private float coeffEnergyResistance = 1;

	//

	private float coeffFatigueHeat;
	private float coeffFatigueForce;
	private float coeffFatigueEnergy;

	//

    // Start is called before the first frame update
    void Start()
    {
		//Every child will have to assign his propierties from a seriarizable object.

		_heat = 0;
		_energy = 0;
		force = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
        

    }

	public float GetHeat() { return _heat; }
	public float GetEnergy() { return _energy; }
	public float GetFatigue() { return _fatigue; }

	public void AddEnergy(float energyReceibed)
	{
		_energy += energyReceibed;
	}
	public void AddHeat(float heatReceibed)
	{
		_heat += heatReceibed;
	}
	public void AddForce(Vector3 forceReceived)
	{
		force += forceReceived;
	}

	public float AbsorbEnergy(float energyAsked) {

		float returnedEnergy = 0;

		if(energyAsked <= _energy)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = _energy;
		}

		_energy -= returnedEnergy;
		return returnedEnergy;

	}
	public float AbsorbHeat(float energyAsked)
	{

		float returnedEnergy = 0;

		if (energyAsked <= _heat)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = _heat;
		}

		_heat -= returnedEnergy;
		return returnedEnergy;

	}
	public float AbsorbForce(float energyAsked)
	{
		//Todo absorbforce
		float returnedEnergy = 0;

		if (energyAsked <= _energy)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = _energy;
		}

		_energy -= returnedEnergy;
		return 0;

	}

}
