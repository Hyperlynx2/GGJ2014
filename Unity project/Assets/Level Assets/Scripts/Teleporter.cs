using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
	public string     TeleporterID;
	public Tile		  ConnectedTile;
	public Teleporter ConnectedTeleporter;
	public Color      TeleporterColour;
	
	
	void Start()
	{
		gameObject.renderer.material.color = TeleporterColour;
	}
}
