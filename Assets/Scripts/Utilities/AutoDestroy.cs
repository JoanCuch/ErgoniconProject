using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

	public int secondsToDestroy;

    void Start()
    {
		StartCoroutine("TimedDetroy");
    }

	private IEnumerator TimedDetroy()
	{
		yield return new WaitForSeconds(secondsToDestroy);
		Debug.Log("I don't want to life anymore");
		Destroy(gameObject);
	}
}
