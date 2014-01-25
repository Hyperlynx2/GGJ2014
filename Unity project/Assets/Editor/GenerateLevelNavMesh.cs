using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class GenerateLevelNavMesh : Editor 
{
	private static List<Color> TeleporterColourList;
	private static int _TeleportColourIndex;
	
	[MenuItem("Helper Functionality/Generate Level Mesh")]
	public static void GenerateLevelMesh()
	{	
		TeleporterColourList = new List<Color>();
		TeleporterColourList.Add(new Color(1.0f, 0, 0));
		TeleporterColourList.Add(new Color(0, 1.0f, 0));
		TeleporterColourList.Add (new Color(0, 0.0f, 1.0f));
		TeleporterColourList.Add (new Color(0, 1.0f, 1.0f));
		TeleporterColourList.Add (new Color(1.0f, 0.0f, 1.0f));
		TeleporterColourList.Add (new Color(1.0f, 1.0f, 0.0f));
		TeleporterColourList.Add (new Color(0.7f, 0.1f, 0.5f));
		TeleporterColourList.Add (new Color(0.2f, 0.9f, 0.3f));
		
		_TeleportColourIndex = 0;
		
		ReplaceObjectsWithPrefabs ();
		SetupTileConnections ();
		FindAttachedSpawnersAndGoal();
		
		FindAttachedTeleporters();
		
		FindJumpers();
	}
	
	static void SetupTileConnections ()
	{
		Tile[] tiles = GameObject.FindObjectsOfType(typeof(Tile)) as Tile[];
		Debug.Log ("Found " + tiles.Length + " tiles!");
		foreach(Tile t in tiles)
		{
			t.NorthTile = null;
			t.SouthTile = null;
			t.EastTile = null;
			t.WestTile = null;
			
			
			Vector3 tilePosition = t.transform.position;
			//Find tiles "north" of us..which is less on the z axis
			foreach(Tile otherTile in tiles)
			{
				if(WithinRange(tilePosition.x, otherTile.transform.position.x, 0.5f) &&
				   WithinRange(tilePosition.z + 4,otherTile.transform.position.z, 0.5f) &&
					(tilePosition.y + 3.5 >= otherTile.transform.position.y))
				{
					if(t.NorthTile == null || (t.NorthTile.transform.position.y < otherTile.transform.position.y))
					{
						t.NorthTile = otherTile;
					}
				}
				
			}
			//Find tiles "south" of us..which is less on the z axis
			foreach(Tile otherTile in tiles)
			{
				if(WithinRange(tilePosition.x, otherTile.transform.position.x, 0.5f) &&
				   WithinRange(tilePosition.z - 4,otherTile.transform.position.z, 0.5f) &&
					(tilePosition.y + 3.5 >= otherTile.transform.position.y))
				{
					if(t.SouthTile == null || (t.SouthTile.transform.position.y < otherTile.transform.position.y))
					{
						t.SouthTile = otherTile;
					}
				}
				
				
			}
			//Find tiles "west" of us..which is less on the x axis
			foreach(Tile otherTile in tiles)
			{
				if(WithinRange(tilePosition.x - 4, otherTile.transform.position.x, 0.5f) &&
				   WithinRange(tilePosition.z, otherTile.transform.position.z, 0.5f) &&
					(tilePosition.y + 3.5 >= otherTile.transform.position.y))
				{
					if(t.WestTile == null || (t.WestTile.transform.position.y < otherTile.transform.position.y))
					{
						t.WestTile = otherTile;
					}
				}
				
				
			}
			
			//Find tiles "east" of us..which is less on the x axis
			foreach(Tile otherTile in tiles)
			{
				if(WithinRange(tilePosition.x + 4, otherTile.transform.position.x, 0.5f) &&
				   WithinRange(tilePosition.z,otherTile.transform.position.z, 0.5f) &&
					(tilePosition.y + 3.5 >= otherTile.transform.position.y))
				{
					if(t.EastTile == null || (t.EastTile.transform.position.y < otherTile.transform.position.y))
					{
						t.EastTile = otherTile;
					}
				}
				
				
			}
	
		}
	}

	static void ReplaceObjectsWithPrefabs ()
	{
		GameObject levelObj = null;
		GameObject spawnerObj = null;
		//Check to make sure there is a "Level" node here - it'll be the parent of everything will be in the letter
		if(GameObject.Find ("Level") == null)
		{
			levelObj = new GameObject("Level");
		}
		
		if(GameObject.Find ("Spawners") == null)
		{
			spawnerObj = new GameObject("Spawners");
		}
		
		if(GameObject.Find("LevelController") == null)
		{
			Object prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/LevelController.prefab", typeof(GameObject));
			GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			clone.name = "LevelController";
			//clone.transform.parent = levelObj.transform;
			//clone.transform.position = obj.transform.position;
			//clone.transform.rotation = obj.transform.rotation;	
		}
		
		
		
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject obj in allObjects)
		{
			if(obj.name.Contains(" "))
			{
				obj.name = obj.name.Substring(0, obj.name.IndexOf(" "));
			}
			
			Object prefab = null;
			if(obj.name.Contains("Tile-Blank"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Tile.prefab", typeof(GameObject));
			}
			
			if(obj.name.Contains("Flag-Goal"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Flag-Goal.prefab", typeof(GameObject));	
			}
			
			if(obj.name.Contains("Flag-Spawner"))
			{
				int number = int.Parse(obj.name.Substring(15));
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Flag-Spawner-" + number.ToString() + ".prefab", typeof(GameObject));	
			}
			
			if(obj.name.Contains("Player-Spawner"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Player-Spawner.prefab", typeof(GameObject));
			}
			
			if(obj.name.Contains("Teleporter"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Teleporter.prefab", typeof(GameObject));
			}
			
			if(obj.name.Contains("Jumper"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Jumper.prefab", typeof(GameObject));
			}
			if(obj.name.Contains("Jumper-Target"))
			{
				prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Jumper-Target.prefab", typeof(GameObject));
			}
			
			
			if(prefab != null)
			{
				GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				clone.transform.parent = levelObj.transform;
				clone.transform.position = obj.transform.position;
				clone.transform.rotation = obj.transform.rotation;
				
				
				if(obj.name.Contains("Tile-Blank"))
				{
					clone.name = "Tile";
				}
				
				if(obj.name.Contains("Flag-Goal"))
				{
					clone.name = "Flag-Goal";	
				}
				
				if(obj.name.Contains("Flag-Spawner"))
				{
					clone.name = "Flag-Spawner";
					int number = int.Parse(obj.name.Substring(13, 1));
					clone.GetComponent<Spawner>().NumSpawnedCandles = number;
					clone.transform.parent = spawnerObj.transform;
				}
				
				if(obj.name.Contains("Player-Spawner"))
				{
					clone.name = "Player-Spawner";	
				}
				
				if(obj.name.Contains("Teleporter"))
				{
					string teleporterID = obj.name.Substring(11, 1);
					clone.name = "Teleporter";	
					clone.GetComponent<Teleporter>().TeleporterID = teleporterID;
				}
				
				if(obj.name.Contains("Jumper") && !obj.name.Contains("Jumper-Target"))
				{
					string JumperID = obj.name.Substring(7, 1);
					clone.name = "Jumper";	
					clone.GetComponent<Jumper>().JumperID = JumperID;
				}
				
				if(obj.name.Contains("Jumper-Target"))
				{
					string JumperID = obj.name.Substring(14, 1);
					clone.name = "Jumper-Target";	
					clone.GetComponent<JumperTarget>().JumperID = JumperID;
				}
				
				DestroyImmediate(obj);
			}			
		}
	}
	
	static void FindAttachedSpawnersAndGoal()
	{
		Tile[] tiles = GameObject.FindObjectsOfType(typeof(Tile)) as Tile[];
		
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		foreach(GameObject obj in allObjects)
		{
			if(obj.name.Contains("Flag-Goal"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{
					connectedTile.FlagGoalIsHere = true;
				}
			}
			
			if(obj.name.Contains("Flag-Spawner"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{
					connectedTile.ConnectedSpawner = obj.GetComponent<Spawner>();
					
					//Create the flags to go here
					
					float fRotationOffset = 360 / connectedTile.ConnectedSpawner.NumSpawnedCandles;
					
					float fCurRotate = 0;
					for(int i = 0;i < connectedTile.ConnectedSpawner.NumSpawnedCandles;++i)
					{
						Object prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Flag.prefab", typeof(GameObject));	
						GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;	
						clone.GetComponent<Candle>().candleNum = obj.GetComponent<Spawner>().PointsPerCandle;
						
						clone.transform.position = obj.transform.position;
						
						
						
						clone.transform.rotation = obj.transform.rotation * Quaternion.Euler (0, 0, fCurRotate);
						fCurRotate += fRotationOffset;
						
						
						connectedTile.ConnectedSpawner.CandleInstances.Add(clone);
					}
				}
			}
			
			if(obj.name.Contains("Player-Spawner"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{
						Object prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Player.prefab", typeof(GameObject));	
						GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
						clone.transform.position = obj.transform.position;
						clone.transform.rotation = obj.transform.rotation;
					
						clone.GetComponent<Player>().startTile = connectedTile;
				}
			}
		}
	}
	
	static void FindAttachedTeleporters()
	{
		Tile[] tiles = GameObject.FindObjectsOfType(typeof(Tile)) as Tile[];
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		for(int i = 0;i < allObjects.Length;++i)
		{
			GameObject obj = allObjects[i];
			
			
			
			if(obj.name.Contains("Teleporter"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{
					connectedTile.ConnectedTeleporter = obj.GetComponent<Teleporter>();
					connectedTile.ConnectedTeleporter.ConnectedTile = connectedTile;
					
					for(int j = i + 1;j < allObjects.Length;++j)
					{
						Teleporter otherTeleporter = allObjects[j].GetComponent<Teleporter>();
						if(otherTeleporter  && otherTeleporter.TeleporterID == connectedTile.ConnectedTeleporter.TeleporterID)
						{
							connectedTile.ConnectedTeleporter.ConnectedTeleporter = otherTeleporter;
							otherTeleporter.ConnectedTeleporter = connectedTile.ConnectedTeleporter;
							
							connectedTile.ConnectedTeleporter.TeleporterColour = TeleporterColourList[_TeleportColourIndex];
							otherTeleporter.TeleporterColour = TeleporterColourList[_TeleportColourIndex];
							
							++_TeleportColourIndex;
							if(_TeleportColourIndex >= TeleporterColourList.Count) _TeleportColourIndex = 0;
						}
					}
					
				}
			}
		}
	}
	
	static void FindJumpers()
	{
		Tile[] tiles = GameObject.FindObjectsOfType(typeof(Tile)) as Tile[];
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		foreach(GameObject obj in allObjects)
		{
			
			if(obj.name.Contains("Jumper") && !obj.name.Contains("Jumper-Target"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{
					connectedTile.ConnectedJumper = obj.GetComponent<Jumper>();
				}
			}
			
			if(obj.name.Contains("Jumper-Target"))
			{
				Tile connectedTile = FindConnectedTile(tiles, obj);
				if(connectedTile)
				{	
					
					foreach(GameObject otherObject in allObjects)
					{
						Jumper jump = otherObject.GetComponent<Jumper>();
						if(jump && jump.JumperID == obj.GetComponent<JumperTarget>().JumperID)
						{
							jump.ConnectedTile = connectedTile;
						}
					}
					
				}
			}
		}
	}
	
	static bool WithinRange(float valOne, float valTwo, float range)
	{
		return Mathf.Abs (valOne - valTwo) < range;
	}
	static Tile FindConnectedTile(Tile[] tiles, GameObject obj)
	{
		foreach(Tile t in tiles)
		{
			if(WithinRange(t.transform.position.x, obj.transform.position.x, 0.25f) &&
				WithinRange(t.transform.position.y, obj.transform.position.y, 0.25f) &&
				WithinRange(t.transform.position.z, obj.transform.position.z, 0.25f) )
			{
				return t;
			}
		}
		
		return null;
	}
}
