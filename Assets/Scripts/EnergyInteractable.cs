using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyInteractable : MonoBehaviour
{

	[ReadOnly] [SerializeField] private float heat;
	[ReadOnly] [SerializeField] private float energy;
	[ReadOnly] [SerializeField] private Vector3 impulse;
	[ReadOnly] [SerializeField] private float fatigue;

	private float coeffFatigueHeat;
	private float coeffFatigueForce;
	private float coeffFatigueEnergy;


    // Start is called before the first frame update
    void Start()
    {
		//Every child will have to assign his propierties from a seriarizable object.

		heat = 0;
		energy = 0;
		impulse = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
        

    }

	public float GetHeat() { return heat; }
	public float GetEnergy() { return energy; }
	public float GetFatigue() { return fatigue; }

	public void AddEnergy(float energyReceibed)
	{
		energy += energyReceibed;
	}
	public void AddHeat(float heatReceibed)
	{
		heat += heatReceibed;
	}
	public void AddImpulse(Vector3 forceReceived)
	{
		impulse += forceReceived;
	}

	public float AbsorbEnergy(float energyAsked) {

		float returnedEnergy = 0;

		if(energyAsked <= energy)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = energy;
		}

		energy -= returnedEnergy;
		return returnedEnergy;

	}
	public float AbsorbHeat(float energyAsked)
	{

		float returnedEnergy = 0;

		if (energyAsked <= heat)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = heat;
		}

		heat -= returnedEnergy;
		return returnedEnergy;

	}
	public float AbsorbImpulse(float energyAsked)
	{
		//Todo absorbforce
		float returnedEnergy = 0;

		if (energyAsked <= energy)
		{
			returnedEnergy = energyAsked;
		}
		else
		{
			returnedEnergy = energy;
		}

		energy -= returnedEnergy;
		return 0;

	}

}
