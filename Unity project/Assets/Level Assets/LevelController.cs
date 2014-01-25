using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	
	//Level Control Settings
	public float MaxLevelTime = 100;
	public string TransitionSceneName = "EndGame";
	
	private float _remainingTime;
	
	
	void Start()
	{
		_remainingTime = MaxLevelTime;
	}
	
	void Update()
	{
		_remainingTime -= Time.deltaTime;
		
		if(_remainingTime <= 0.0f || IsAllFlagsCollected() || IsAllTilesPainted())
		{
			Debug.Log ("The Game is finished!");
			//Application.LoadLevel(TransitionSceneName);
		}
	}
	
	
	private bool IsAllFlagsCollected()
	{
		Spawner[] allSpawners = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
		
		int iNumberFlagsRemaining = 0;
		foreach(Spawner sp in allSpawners)
		{
			iNumberFlagsRemaining += sp.NumSpawnedFlags;
		}
		
		return iNumberFlagsRemaining == 0;
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
}
