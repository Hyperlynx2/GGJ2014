using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	
	public static GameManager GetInstance()
	{
		return _instance;
	}
	
	public float left;
	public float top;
	public float width = 100;
	public float height = 100;
	
	/// <summary>
	/// Gap between buttons
	/// </summary>
	public float separation;
	
	public string[] menuItemScenes;
	public string[] menuItemLabels;
	public GUISkin skin;
	
	/*TODO: GameManager might as well take over full responsibility for these, rather than the
	player storing them. At the moment, they're just storing the values from OnGameOver so that
	OnGUI can use them.*/
	private float _player1Score;
	private float _player2Score;
	
	private enum STATE
	{
		MENU,
		INGAME,
		GAMEOVER
	}
	
	private STATE _state;
	
	/// <summary>
	/// Call from levels when it's game over to transition to "game over!" screen.
	/// </summary>
	public void OnGameOver(float player1Score, float player2Score)
	{
		Application.LoadLevel("mainScene");		
		_state = STATE.GAMEOVER;
		_player1Score = player1Score;
		_player2Score = player2Score;
	}
	
	// Use this for initialization
	void Start ()
	{
		if(_instance == null)
		{
			_instance = this;
		}
		else
		{
			throw new Exception("More than one GameManager instance! Very bad!");
		}
		
		_state = STATE.MENU;
		
		DontDestroyOnLoad(gameObject);
	}
	
	void OnGUI()
	{
		GUI.skin = skin;

		switch(_state)
		{
		case STATE.MENU:
			//god help you if you haven't got the labels and scenes right sized!
			for(int i = 0; i < menuItemLabels.Length; ++i)
			{
				//TODO: change to be percentage of screen rather than absolute position.
				if(GUI.Button(new Rect(left, top + i*(height + separation), width, height), menuItemLabels[i]))
				{
					_state = STATE.INGAME;
					Application.LoadLevel(menuItemScenes[i]);
				}
				
			}
			break;
		case STATE.INGAME:
			//nothing to do. player GUI and level GUI take over.
			break;
		case STATE.GAMEOVER:
			GUI.Label(new Rect(Screen.width /2, 10, 200, 50), "Game over!");
			
			string victoryText = "Player 1 wins!";
			
			if(_player2Score > _player1Score)
			{
				victoryText = "Player 2 wins!";
			}
			else if(_player1Score == _player2Score)
			{
				victoryText = "Draw!";
			}
			
			GUI.Label(new Rect(Screen.width /2, 70, 200, 50), victoryText);
			
			break;
		}
		
		
	}
	
	
}
