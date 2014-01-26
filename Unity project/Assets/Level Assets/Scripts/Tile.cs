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
	
	public Texture2D PainterHighLight_UnpaintedTexture;
	public Texture2D PainterHighLight_PaintedTexture;
	
	public Texture2D CounterHighLight_UnpaintedTexture;
	public Texture2D CounterHighLight_PaintedTexture;
	
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
	
	public void SetTileHighlighted(Player.PLAYER_ID playerID, bool bHighlighted)
	{
		if(bHighlighted == true)
		{
			if(bIsPainted)
			{
				if(playerID == Player.PLAYER_ID.PAINTER)
				{
					gameObject.renderer.material.mainTexture = PainterHighLight_PaintedTexture;
				}
				else
				{
					gameObject.renderer.material.mainTexture = CounterHighLight_PaintedTexture;
				}
			}
			else
			{
				if(playerID == Player.PLAYER_ID.PAINTER)
				{
					gameObject.renderer.material.mainTexture = PainterHighLight_UnpaintedTexture;
				}
				else
				{
					gameObject.renderer.material.mainTexture = CounterHighLight_UnpaintedTexture;
				}
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
	
	private void SetConnectedTilesHighlighted(Player.PLAYER_ID playerID, bool bHighlighted)
	{
		if(NorthTile != null)
			NorthTile.SetTileHighlighted(playerID, bHighlighted);
		
		if(EastTile != null)
			EastTile.SetTileHighlighted(playerID, bHighlighted);
		
		if(SouthTile != null)
			SouthTile.SetTileHighlighted(playerID, bHighlighted);
		
		if(WestTile != null)
			WestTile.SetTileHighlighted(playerID, bHighlighted);
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
	
	//Returns 0 if no spawner
	//or returns the spawner value if a spawner is available, and has candles to spare
	public int IsSpawnerHere()
	{
		
		if(ConnectedSpawner == null || ConnectedSpawner.NumSpawnedCandles == 0)
			return 0;
		
		return ConnectedSpawner.PointsPerCandle;
	}
	
	//Removes a single candle from the spawner on this tile, if there is one
	//users should call "IsSpawnerHere" to work out if a spawner is availble
	//here first before calling this function
	
	public void CollectTileCandle()
	{
		//Sanity check, as little as it means in this function
		if(IsSpawnerHere() == 0)
			return;
		
		DestroyObject(ConnectedSpawner.CandleInstances[0]);
		ConnectedSpawner.CandleInstances.RemoveAt(0);
		ConnectedSpawner.NumSpawnedCandles--;
		ConnectedSpawner.audio.Play();

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
	public void OnTileEnter(Player.PLAYER_ID playerID)
	{
		SetConnectedTilesHighlighted(playerID, true);
	}
	
	/// <summary>
	/// Play visual effects when walking off this tile.
	/// </summary>
	public void OnTileExit(Player.PLAYER_ID playerID)
	{
		SetConnectedTilesHighlighted(playerID, false);
	}
	
	/// <summary>
	/// Play visual effects when arriving at this via teleport
	/// </summary>
	public void OnTileSpecialEnter(Player.PLAYER_ID playerID)
	{
	}
}
