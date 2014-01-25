using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	//Tile Modifiers
	public bool FlagGoalIsHere = false;
	public Spawner ConnectedSpawner = null;
	public Teleporter ConnectedTeleporter = null;
	public Jumper ConnectedJumper = null;
	
	//Exit List
	public Tile NorthTile;
	public Tile EastTile;
	public Tile WestTile;
	public Tile SouthTile;

	public Texture2D UnpaintedTexture;
	public Texture2D PaintedTexture;
	
	public Texture2D UnPaintedHighlightTexture;
	public Texture2D PaintedHighlightTexture;
	
	private bool   bIsPainted = false;
	
	void Start()
	{
		gameObject.renderer.material.mainTexture = UnpaintedTexture;
	}
	
	/// <summary>
	/// Paints the tile, if unpainted.
	/// </summary>
	/// <returns>
	/// True if the tile became painted, false if it was already painted.
	/// </returns>
	public bool PaintTile()
	{
		bool tileWasPainted = false;
		if(!bIsPainted)
		{
			bIsPainted = true;
			tileWasPainted = true;
			gameObject.renderer.material.mainTexture = PaintedTexture;
		}
		
		return tileWasPainted;
	}
	
	public void SetTileHighlighted(bool bHighlighted)
	{
		if(bHighlighted == true)
		{
			if(bIsPainted)
			{
				gameObject.renderer.material.mainTexture = PaintedHighlightTexture;
			}
			else
			{
				gameObject.renderer.material.mainTexture = UnPaintedHighlightTexture;
			}
		}
		else
		{
			if(bIsPainted)
			{
				gameObject.renderer.material.mainTexture = PaintedTexture;
			}
			else
			{
				gameObject.renderer.material.mainTexture = UnpaintedTexture;
			}
		}
	}
	
	private void SetConnectedTilesHighlighted(bool bHighlighted)
	{
		if(NorthTile != null)
			NorthTile.SetTileHighlighted(bHighlighted);
		
		if(EastTile != null)
			EastTile.SetTileHighlighted(bHighlighted);
		
		if(SouthTile != null)
			SouthTile.SetTileHighlighted(bHighlighted);
		
		if(WestTile != null)
			WestTile.SetTileHighlighted(bHighlighted);
	}
	
	public void OnDrawGizmosSelected()
	{
		if(NorthTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = new Color(0, 252, 255, 255);
			Gizmos.DrawWireCube(NorthTile.transform.position + new Vector3(0, 0.25f, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(SouthTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(SouthTile.transform.position + new Vector3(0, 0.25f, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(EastTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = new Color(1.0f, 0.8f, 0.8f, 255);
			Gizmos.DrawWireCube(EastTile.transform.position + new Vector3(0, 0.25f, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
		if(WestTile != null)
		{
			Color prevColour = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(WestTile.transform.position + new Vector3(0, 0.25f, 0), new Vector3(4, 1, 4));
			Gizmos.color = prevColour;
		}
	}
	
	public bool IsPainted() { return bIsPainted; }
	
	public int CollectTileFlags()
	{
		if(ConnectedSpawner == null)
			return 0;
		
		int iNumFlags = ConnectedSpawner.NumSpawnedFlags;
		foreach(GameObject f in ConnectedSpawner.FlagInstances)
		{
			DestroyObject(f);
		}
		ConnectedSpawner.FlagInstances.Clear();
		ConnectedSpawner.NumSpawnedFlags = 0;
		
		return iNumFlags;
	}
	
	public Tile GetConnectedTeleporterTile()
	{
		if(ConnectedTeleporter == null)
		{
			return null;
		}
		else
		{
			return ConnectedTeleporter.ConnectedTeleporter.ConnectedTile;
		}
	}
	
	public Tile GetConnectedJumperTile()
	{
		if(ConnectedJumper == null)
		{
			return null;
		}
		else
		{
			return ConnectedJumper.ConnectedTile;
		}
	}
	
	/// <summary>
	/// Play visual effects when walking or jumping onto this tile.
	/// </summary>
	public void OnTileEnter()
	{
		SetConnectedTilesHighlighted(true);
	}
	
	/// <summary>
	/// Play visual effects when walking off this tile.
	/// </summary>
	public void OnTileExit()
	{
		SetConnectedTilesHighlighted(false);
	}
	
	/// <summary>
	/// Play visual effects when arriving at this via teleport
	/// </summary>
	public void OnTileSpecialEnter()
	{
		//TODO: OnTileSpecialEnter
	}
}
