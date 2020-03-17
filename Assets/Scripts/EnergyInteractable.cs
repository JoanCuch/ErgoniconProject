using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyInteractable : MonoBehaviour
{

	//

	private float _heat;
	private float _energy;
	private Vector3 force;

	private float coeffHeatResistance;
	private float coeffForceResistance;
	private float coeffEnergyResistance;

	//

	private float _fatigue;

	private float coeffFatigueHeat;
	private float coeffFatigueForce;
	private float coeffFatigueEnergy;

	//

    // Start is called before the first frame update
    void Start()
    {
		//Every child will have to assign his propierties from a seriarizable object.
	
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
		_energy += energyReceibed * coeffEnergyResistance;
	}
	public void AddHeat(float heatReceibed)
	{
		_heat += heatReceibed * coeffHeatResistance;
	}
	public void AddForce(Vector3 forceReceived)
	{
		force += forceReceived * coeffForceResistance;
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
