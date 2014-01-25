using UnityEngine;
using System;
using System.Collections;

public class LevelController : MonoBehaviour {
	
	//Level Control Settings
	public float MaxLevelTime = 100;
	public string TransitionSceneName = "EndGame";
	
	public GUISkin HUDSkin;
	
	private float _remainingTime;
	
	void Start()
	{
		//_remainingTime = MaxLevelTime;
		_remainingTime = 4;
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
		
		TimeSpan levelTime = new TimeSpan(0, 0, (int)_remainingTime);
		
		//TODO: do as percentage of screen width, not absolute pixel value.
		GUI.Box (new Rect (400, 20, 100,100), levelTime.Minutes + ":" + levelTime.Seconds);
		//TODO: BUG: seconds is single-digit when it gets under 10 seconds!
	}
}
