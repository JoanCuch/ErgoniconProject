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
			Transform _newTarget = SelectTarget();
			if (_newTarget != null)
			{
				runeCreator.SetTarget(_newTarget);
				Debug.Log("Detected object: " + _newTarget.name);
			}
			
		}
    }

	public Transform SelectTarget()
	{
		Transform hitObject = null;
		Transform indexFinger = gameManager.GetHandIndex();

		RaycastHit hit;
		Ray ray;

		ray = new Ray(indexFinger.position, Vector3.Normalize(-indexFinger.right));

		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.transform;
		}

		return hitObject;
	}

	public void SetIsSelecting(bool _newState) {
		isSelecting = _newState;
	}
}
