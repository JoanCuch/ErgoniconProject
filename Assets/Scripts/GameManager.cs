using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Singletone instance
	public static GameManager gameManager;

	public RunesWarehouse runesWarehouse;
	public InputManager inputManager;
	public RuneCreator runeCreator;



	public enum GameStates { drawMode, selectMode}
	private GameStates gameState;


    // Start is called before the first frame update
    void Start()
    {
		gameState = GameStates.drawMode;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void UpdateState()
	{
		switch (gameState)
		{
			case GameStates.drawMode:
				runeCreator.SetIsDrawing(inputManager.CheckDrawRune());
				break;

			case GameStates.selectMode:
				break;

			default:
				break;
		}
	}


	private void ChangeState()
	{

	}


}
