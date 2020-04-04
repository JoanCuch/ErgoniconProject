using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{

	private GameManager gameManager;
	private RuneCreator runeCreator;
	private Transform target;

	private bool isSelecting;


    // Start is called before the first frame update
    void Start()
    {
		gameManager = GameManager.gameManager;
		runeCreator = gameManager.runeCreator;
		isSelecting = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (gameManager == null)
		{
			gameManager = GameManager.gameManager;
			runeCreator = gameManager.runeCreator;
		}


		if (isSelecting)
		{
			RaycastHit hit;
			Transform _newTarget = SelectTarget(out hit);
			if (_newTarget != null)
			{
				
				runeCreator.SetTarget(_newTarget, hit);
				Debug.Log("Detected object: " + _newTarget.name);
			}
			
		}
    }

	public Transform SelectTarget(out RaycastHit externalHit)
	{
		Transform hitObject = null;
		Transform indexFinger = gameManager.GetHandIndex();

		RaycastHit hit;
		Ray ray;

		ray = new Ray(indexFinger.position, Vector3.Normalize(-indexFinger.right));

		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.collider.transform;			
		}

		externalHit = hit;

		return hitObject;
	}

	public void SetIsSelecting(bool _newState) {
		isSelecting = _newState;
	}
}
