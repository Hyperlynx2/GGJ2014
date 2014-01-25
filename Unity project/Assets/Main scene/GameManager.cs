using UnityEngine;
using System;

public class GameManager
{
	private static GameManager _instance = null;
	
	public static GameManager GetInstance()
	{
		if(_instance == null)
			_instance = new GameManager();
		
		return _instance;
	}
		
	/// <summary>
	/// Gap between buttons
	/// </summary>
	public float separation = 5;
	
	public string[] menuItemScenes;
	public string[] menuItemLabels;
	public GUISkin skin;
	
	//scores are properties so that the inspector can't get at them. they're otherwise public.
	public float Player1Score
	{
		get { return _player1Score;}
		
		set {_player1Score = value;}
	}
	
	public float Player2Score
	{
		get { return _player2Score;}
		
		set {_player2Score = value;}
	}
	
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
	public void OnGameOver()
	{
		Application.LoadLevel("mainScene");		
		_state = STATE.GAMEOVER;
	}
	
	private GameManager()
	{
		_state = STATE.MENU;
	}
	
	public void OnGUI()
	{
		GUI.skin = skin;
		
		float heightOffset = 10;
		const float height = 50;
		const float width = 200;

		switch(_state)
		{
		case STATE.MENU:
			//god help you if you haven't got the labels and scenes right sized!
			for(int i = 0; i < menuItemLabels.Length; ++i)
			{
				//TODO: change to be percentage of screen rather than absolute position.
				if(GUI.Button(new Rect(Screen.width /2 - width/2, 10 + i*(height + separation), width, height), menuItemLabels[i]))
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
			GUI.Box(new Rect(Screen.width /2 - width/2, heightOffset, width, height), "Game over!");
			heightOffset += height;
			
			string victoryText = "Player 1 wins!";
			
			if(_player2Score > _player1Score)
			{
				victoryText = "Player 2 wins!";
			}
			else if(_player1Score == _player2Score)
			{
				victoryText = "Draw!";
			}
			
			GUI.Box(new Rect(Screen.width /2 - width/2, heightOffset, width, height), victoryText);
			heightOffset += height;
			
			GUI.Box(new Rect(Screen.width /2 - width/2, heightOffset, width, height), _player1Score + " vs " + _player2Score);
			heightOffset += height;
			
			GUI.Box(new Rect(Screen.width /2 - width/2, Screen.height - (height+10), width, height), "Press start");
			
			
			break;
		}
	}
	
	public void Update()
	{
		if(_state == STATE.GAMEOVER)
		{
			if(Input.GetAxis("Start") > 0)
			{
				_state = STATE.MENU;
			}
		}		
	}
	
}
