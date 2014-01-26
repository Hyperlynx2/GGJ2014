using UnityEngine;
using System;
using System.Collections;

public class LevelController : MonoBehaviour {
	
	//Level Control Settings
	public float MaxLevelTime = 100;
	public string TransitionSceneName = "EndGame";
	
	public GUISkin HUDSkin;
	public GUISkin player1HUDSkin;
	
	float timerBoxWidth = 200;
	float timerBoxHeight = 55;
	float hudMargin = 10;
	float scoreBoxWidth = 170;
	float scoreBoxHeight = 30;
	
	private float _remainingTime;
	
	void Start()
	{
		_remainingTime = MaxLevelTime;
		//_remainingTime = 2;
	}
	
	void Update()
	{
		_remainingTime -= Time.deltaTime;
		
		if(_remainingTime <= 0.0f || IsAllFlagsCollected() || IsAllTilesPainted())
		{
			GameManager.GetInstance().OnGameOver();
			//Application.LoadLevel(TransitionSceneName);
		}
	}
	
	private bool IsAllFlagsCollected()
	{
		Spawner[] allSpawners = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
		
		int iNumberCandlesRemaining = 0;
		foreach(Spawner sp in allSpawners)
		{
			iNumberCandlesRemaining += sp.NumSpawnedCandles;
		}
		
		return iNumberCandlesRemaining == 0;
	}
	
	private bool IsAllTilesPainted()
	{
		Tile[] allTiles = GameObject.Find("Level").GetComponentsInChildren<Tile>();
	
		foreach(Tile t in allTiles)
		{
			if(!t.IsPainted())
			{
				return false;
			}
		}
		
		return true;
	}
	
	void OnGUI()
	{
		GUI.skin = HUDSkin;

		//level timer:
		TimeSpan levelTime = new TimeSpan(0, 0, (int)_remainingTime);
		GUI.Label(new Rect (Screen.width /2 - timerBoxWidth/2, hudMargin, timerBoxWidth, timerBoxHeight), levelTime.Minutes + ":" + levelTime.Seconds.ToString().PadLeft(2,'0'));
		
		//P1 score:
		GUI.Box(new Rect (Screen.width /3 - scoreBoxWidth/2, hudMargin + timerBoxHeight + hudMargin, scoreBoxWidth, scoreBoxHeight),
			"P1: " + GameManager.GetInstance().Player1Score,
			GUI.skin.GetStyle("P1Score"));
		
		//P2 score:
		GUI.Box(new Rect (Screen.width/3 * 2 - scoreBoxWidth/2, hudMargin + timerBoxHeight + hudMargin, scoreBoxWidth, scoreBoxHeight),
			"P2: " + GameManager.GetInstance().Player2Score,
			GUI.skin.GetStyle("P2Score"));
	}
}
