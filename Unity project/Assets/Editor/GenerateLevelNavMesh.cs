using UnityEngine;
using System.Collections;
using UnityEditor;

public class GenerateLevelNavMesh : Editor 
{
	[MenuItem("Helper Functionality/Generate Level Mesh")]
	public static void GenerateLevelMesh()
	{
		
		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach(GameObject obj in allObjects)
		{
			if(obj.name == "TileBlank")
			{
				Object prefab = AssetDatabase.LoadAssetAtPath("Assets/LevelPrefabs/Tile.prefab", typeof(GameObject));
				GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
				clone.transform.position = obj.transform.position;
				clone.transform.rotation = obj.transform.rotation;
				clone.name = "Tile";
				DestroyImmediate(obj);
			}
		}
		
		
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
				if(tilePosition.x == otherTile.transform.position.x &&
				   tilePosition.z - 4 == otherTile.transform.position.z &&
					(Mathf.Abs(tilePosition.y - otherTile.transform.position.y) <= 1))
				{
					t.NorthTile = otherTile;
				}
				
			}
			//Find tiles "south" of us..which is less on the z axis
			foreach(Tile otherTile in tiles)
			{
				if(tilePosition.x == otherTile.transform.position.x &&
				   tilePosition.z + 4 == otherTile.transform.position.z &&
					(Mathf.Abs(tilePosition.y - otherTile.transform.position.y) <= 1))
				{
					t.SouthTile = otherTile;
				}
				
			}
			//Find tiles "west" of us..which is less on the x axis
			foreach(Tile otherTile in tiles)
			{
				if(tilePosition.x - 4 == otherTile.transform.position.x &&
				   tilePosition.z == otherTile.transform.position.z &&
					(Mathf.Abs(tilePosition.y - otherTile.transform.position.y) <= 1))
				{
					t.WestTile = otherTile;
				}
				
			}
			
			//Find tiles "east" of us..which is less on the x axis
			foreach(Tile otherTile in tiles)
			{
				if(tilePosition.x + 4 == otherTile.transform.position.x &&
				   tilePosition.z == otherTile.transform.position.z &&
					(Mathf.Abs(tilePosition.y - otherTile.transform.position.y) <= 1))
				{
					t.EastTile = otherTile;
				}
				
			}
		}
	}
	
}
