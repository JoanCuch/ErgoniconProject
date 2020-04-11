using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Singletone instance
	public static GameManager gameManager;

	public GlobalBlackboard runesIdealWorld;
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
		string lastShape = null;

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
				else if (inputManager.IsDoingAction(InputManager.PlayerActions.select))
				{			
					var newTarget = objectSelector.SelectTarget();
					runeCreator.SetTarget(newTarget.transform, newTarget.hit);
				}					
				break;

			case GameStates.interactionDrawing:

				var activeControllers = inputManager.GetActiveControllers(InputManager.PlayerActions.draw);

				if(activeControllers.left == null && activeControllers.right == null)
				{
					//If there are no active controllers, there is no possible action
					ChangeState(GameStates.interactionWaiting);
				}

				shapesManager.SetActiveController(activeControllers.left, activeControllers.right);
				shapesManager.GestureUpdate();

				lastShape = shapesManager.GetLastShapeName();

				if (lastShape == null)
				{	
					//the rune is being drawn, do nothing
				}
				else if (lastShape == runesIdealWorld.OpenRuneEditingModeShapeName)
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

				var activeController = inputManager.GetActiveControllers(InputManager.PlayerActions.draw);

				if (activeController.left == null && activeController.right == null)
				{
					//If there are no active controllers, there is no possible action
					ChangeState(GameStates.runeWaiting);
				}

				shapesManager.SetActiveController(activeController.left, activeController.right);
				shapesManager.GestureUpdate();

				lastShape = shapesManager.GetLastShapeName();

				if (lastShape == null)
				{
					//the rune is being drawn		
				}
				else if (lastShape == runesIdealWorld.CloseRuneEditingModeShapeName)
				{
					//the player made the gesture to close the rune editing
					ChangeState(GameStates.interactionWaiting);
				}
				else
				{
					bool shapeIsMinorRune = false;

					foreach (string s in runesIdealWorld.minorRunesNames)
					{
						if (s == lastShape)
						{
							shapeIsMinorRune = true;
							Debug.Log(lastShape +" is a minor rune shape");
						}
					}

					if (shapeIsMinorRune)
					{
						runeCreator.CreateMinorRune(lastShape);
					}
				
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

		Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(Color.green.r * 255f), (byte)(Color.green.g * 255f), (byte)(Color.green.b * 255f), "Change state: " + newState));

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
