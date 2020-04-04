using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Singletone instance
	public static GameManager gameManager;

	public RunesIdealWorld runesIdealWorld;
	public InputManager inputManager;
	public RuneCreator runeCreator;
	public ObjectSelector objectSelector;

	private Transform leftHandIndex;

	public enum GameStates { drawMode, selectMode}
	private GameStates gameState;


	//Target



    // Start is called before the first frame update
    void Start()
    {
		gameState = GameStates.selectMode;

		gameManager = this;
		//leftHandIndex = GameObject.FindGameObjectWithTag("LEFTHANDINDEX").transform;
        
    }

    // Update is called once per frame
    void Update()
    {

		UpdateState();

    }


	private void UpdateState()
	{
		bool _changeStateNecessary = false;
		GameStates _newState = GameStates.selectMode;

		//Check if it is necesary a state change
		switch (gameState)
		{
			case GameStates.drawMode:
				if (inputManager.IsChangingToSelectMode())
				{
					_changeStateNecessary = true;
					_newState = GameStates.selectMode;
				}
				break;

			case GameStates.selectMode:
				if (inputManager.IsChangingToDrawMode())
				{
					_changeStateNecessary = true;
					_newState = GameStates.drawMode;
				}
				break;

			default:
				break;
		}

		//Change the state
		if (_changeStateNecessary)
		{
			_changeStateNecessary = false;
			ChangeState(_newState);
		}
		else { //Or continue checking on the state
			switch (gameState)
			{
				case GameStates.drawMode:
					bool drawing = inputManager.CheckDrawRune();
					runeCreator.SetIsDrawing(drawing);
					break;

				case GameStates.selectMode:
					bool selecting = inputManager.IsSelectingTarget();
					objectSelector.SetIsSelecting(selecting);

					break;

				default:
					break;
			}
		}
	}


	private void ChangeState(GameStates newState)
	{
		
		switch (newState)
		{
			case GameStates.drawMode:
				objectSelector.SetIsSelecting(false);
				break;

			case GameStates.selectMode:
				runeCreator.SetIsDrawing(false);
				break;

			default:
				break;
		}

		gameState = newState;

	}


	public Transform GetSelectedObject()
	{
		Transform hitObject = null;
		Transform indexFinger = gameManager.leftHandIndex;

		RaycastHit hit;
		Ray ray;

		ray = new Ray(indexFinger.position, Vector3.Normalize(-indexFinger.right));

		if (Physics.Raycast(ray, out hit))
		{
			hitObject = hit.transform;
		}

		Debug.Log("Detected object: " + hitObject.name);

		return hitObject;
	}
	public Transform GetHandIndex()
	{
		if (leftHandIndex == null)
			leftHandIndex = GameObject.FindGameObjectWithTag("LEFTHANDINDEX").transform;
		return leftHandIndex;
	}


	public void TryingToSelect() {

	}


}
