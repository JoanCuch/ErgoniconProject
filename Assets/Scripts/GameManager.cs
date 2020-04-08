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
	public ShapesManager shapesManager;

	private Transform leftHandIndex;

	public enum GameStates { inital, interactionWaiting, interactionDrawing, runeWaiting, runeDrawing}
	private GameStates currentState;

    // Start is called before the first frame update
    void Start()
    {
		currentState = GameStates.inital;
		gameManager = this;	      
    }

    // Update is called once per frame
    void Update()
    {
		UpdateState();
    }

	#region FSM

	private void UpdateState()
	{
		//Change to newState if necessary
		switch (currentState)
		{
			case GameStates.inital:
				ChangeState(GameStates.interactionWaiting);
				break;

			case GameStates.interactionWaiting:
				if (inputManager.IsDoingAction(InputManager.PlayerActions.draw))
				{
					ChangeState(GameStates.interactionDrawing);
				}					
				break;

			case GameStates.interactionDrawing:
				UpdateShapesManager();

				if (shapesManager.GetLastShapeName() == null)
				{
					
					//the rune is being drawn, do nothing
				}
				else if (shapesManager.GetLastShapeName() == runesIdealWorld.OpenRuneEditingModeShapeName)
				{
					//the player made the gesture to edit a rune
					ChangeState(GameStates.runeWaiting);
				}
				else
				{
					//The player ended drawing but not any shape he can use now. Return to wait for more shape input.
					ChangeState(newState: GameStates.interactionWaiting);				
				}
				break;

			case GameStates.runeWaiting:
				if (inputManager.IsDoingAction(InputManager.PlayerActions.draw))
				{
					ChangeState(GameStates.runeDrawing);
				}
				break;

			case GameStates.runeDrawing:
				UpdateShapesManager();

				if (shapesManager.GetLastShapeName() == null)
				{
					//the rune is being drawn		
				}
				else if (shapesManager.GetLastShapeName() == runesIdealWorld.CloseRuneEditingModeShapeName)
				{
					//the player made the gesture to close the rune editing
					ChangeState(GameStates.interactionWaiting);
				}
				else
				{
					//The player ended drawing but not any shape he can use now. Return to wait for more shape input.
					ChangeState(newState: GameStates.runeWaiting);
				}
				break;

			default:
				Debug.LogWarning("null game state");
				break;
		}
	}


	private void ChangeState(GameStates newState)
	{
		OnExitState(currentState);
		OnEnterState(newState);
		currentState = newState;

		Debug.Log("Change state to: " + newState);

	}

	private void OnExitState(GameStates _currentState)
	{
		switch (_currentState)
		{
			case GameStates.inital:
				break;
			case GameStates.interactionWaiting:
				break;
			case GameStates.interactionDrawing:
				break;
			case GameStates.runeWaiting:
				break;
			case GameStates.runeDrawing:
				break;
			default:
				break;
		}
	}

	private void OnEnterState(GameStates _nextState)
	{
		switch (_nextState)
		{
			case GameStates.inital:
				break;
			case GameStates.interactionWaiting:
				break;
			case GameStates.interactionDrawing:
				break;
			case GameStates.runeWaiting:
				break;
			case GameStates.runeDrawing:
				break;
			default:
				break;
		}
	}

	#endregion

	#region otherFunctions

	private void UpdateShapesManager()
	{
		GameObject leftController;
		GameObject righController;

		inputManager.GetActiveControllers(InputManager.PlayerActions.draw, out leftController, out righController);

		shapesManager.SetActiveController(leftController, righController);

		shapesManager.GestureUpdate();
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

	#endregion

}
