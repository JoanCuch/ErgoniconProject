using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{

	private GameManager gameManager;
	private Transform target;

    // Start is called before the first frame update
    void Start()
    {
		gameManager = GameManager.gameManager;
    }

    // Update is called once per frame
    void Update()
    {
		if (gameManager == null)
		{
			gameManager = GameManager.gameManager;
		}	
    }

	public (Transform transform, RaycastHit hit) SelectTarget()
	{
		Transform hitObject = null;
		Transform indexFinger = gameManager.GetHandIndex();

		RaycastHit hit;
		Ray ray;

		ray = new Ray(indexFinger.position, Vector3.Normalize(-indexFinger.right));

		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.collider.transform;
			Debug.Log("Detected object: " + hitObject.name);
		}

		return (hitObject, hit);
	}
}
