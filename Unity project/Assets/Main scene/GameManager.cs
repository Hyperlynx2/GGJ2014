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
	
	public GameManager()
	{
		if(_instance == null)
		{
			_instance = this;
			_state = STATE.MENU;
		}
		/*TODO: this is not the right way to do this, and it won't work. it complains that it can't do the
		comparator. if I don't put this in, though, it will create more than one GameManager, and it'll set the
		state to menu, so it'll render the menu GUI stuff and ALSO the "game over" GUI stuff.
		
		Halp, Matt! Brain no worky no more.
		 */
	}
	
	// Use this for initialization
	void Start ()
	{
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
