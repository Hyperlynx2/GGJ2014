using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	
	//Exit List
	public Tile NorthTile;
	public Tile EastTile;
	public Tile WestTile;
	public Tile SouthTile;
	
	
	public void OnDrawGizmosSelected()
	{
		if(NorthTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = new Color(0, 252, 255, 255);
			Gizmos.DrawWireCube(NorthTile.transform.position + new Vector3(0, 1, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(SouthTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(SouthTile.transform.position + new Vector3(0, 1, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(EastTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = new Color(1.0f, 0.8f, 0.8f, 255);
			Gizmos.DrawWireCube(EastTile.transform.position + new Vector3(0, 1, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(WestTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(WestTile.transform.position + new Vector3(0, 1, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
	}
}
