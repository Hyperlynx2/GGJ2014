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
			if(obj.name.Contains("Tile-Blank"))
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
				if(WithinRange(tilePosition.x, otherTile.transform.position.x, 0.5f) &&
				   WithinRange(tilePosition.z + 4,otherTile.transform.position.z, 0.5f) &&
					(tilePosition.y + 1.5 >= otherTile.transform.position.y))
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
					(tilePosition.y + 1.5 >= otherTile.transform.position.y))
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
					(tilePosition.y + 1.5 >= otherTile.transform.position.y))
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
					(tilePosition.y + 1.5 >= otherTile.transform.position.y))
				{
					if(t.EastTile == null || (t.EastTile.transform.position.y < otherTile.transform.position.y))
					{
						t.EastTile = otherTile;
					}
				}
				
				
			}
		}
	}
	
	
	public static bool WithinRange(float valOne, float valTwo, float range)
	{
		return Mathf.Abs (valOne - valTwo) < range;
	}
	
}
