using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR;
using Telemetry;

public class GameManager : MonoBehaviour
{
	//Singletone instance
	public static GameManager gameManager;

	public GlobalBlackboard globalBlackboard;
	public InputManager inputManager;
	public RuneCreator runeCreator;
	public ObjectSelector objectSelector;
	public ShapesManager shapesManager;

	public RuneEditingPostProcessing runeEditingEffect;

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
		string currentShape = null;

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
					
					//Getting the controllers that are activating the correct input and setting the target.
					var fingers = inputManager.GetActiveControllers(InputManager.PlayerActions.select);
					GameObject finger = fingers.left == null ? fingers.right : fingers.left;

					var newTarget = objectSelector.SelectTarget(finger);
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

				currentShape = shapesManager.GetCurrentShapeName();

				if (currentShape == null)
				{	
					//the rune is being drawn, do nothing
				}
				else if (currentShape == globalBlackboard.OpenRuneEditingModeShapeName)
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

				currentShape = shapesManager.GetCurrentShapeName();

				if (currentShape == null)
				{
					//the rune is being drawn		
				}
				else if (currentShape == globalBlackboard.CloseRuneEditingModeShapeName)
				{
					//the player made the gesture to close the rune editing
					ChangeState(GameStates.interactionWaiting);
				}
				else
				{
					Debug.Log("searching rune");
					bool shapeIsMinorRune = false;

					foreach (string s in globalBlackboard.minorRunesNames)
					{
						if (s == currentShape)
						{
							shapeIsMinorRune = true;
							Debug.Log(currentShape +" is a minor rune shape");
						}
					}

					if (shapeIsMinorRune)
					{
						runeCreator.CreateMinorRune(currentShape);
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
		SendEvent(DataManager.Actions.changeState, _nextState);
		switch (_nextState)
		{
			case GameStates.inital:
				break;
			case GameStates.interactionWaiting:
				runeEditingEffect.RuneEditingEffectSetActive(false);
				runeCreator.ChangeTargetLayer(globalBlackboard.defaultLayer);
				inputManager.ChangeGlovesLayer(globalBlackboard.defaultLayer);		
				break;
			case GameStates.interactionDrawing:
				break;
			case GameStates.runeWaiting:
				runeEditingEffect.RuneEditingEffectSetActive(true);
				runeCreator.ChangeTargetLayer(globalBlackboard.targetLayer);
				inputManager.ChangeGlovesLayer(globalBlackboard.targetLayer);

				break;
			case GameStates.runeDrawing:
				break;
			default:
				break;
		}
	}

	#endregion

	#region otherFunctions


	private void SendEvent(DataManager.Actions _action, GameStates _result )
	{
		DataManager.dataManager.AddAction(
			DataManager.Actors.game,
			_action,
			_result.ToString("g"),
			Time.time,
			Time.time,
			"null"
			);
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
