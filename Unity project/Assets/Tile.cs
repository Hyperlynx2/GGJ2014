using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	
	//Exit List
	public Tile NorthTile;
	public Tile EastTile;
	public Tile WestTile;
	public Tile SouthTile;

	public Texture2D UnpaintedTexture;
	public Texture2D PaintedTexture;
	
	public Texture2D UnPaintedHighlightTexture;
	public Texture2D PaintedHighlightTexture;
	
	private bool bIsPainted = false;
	
	void Start()
	{
		SetPainted(true);
	}
	
	public void SetPainted(bool bPainted)
	{
		bIsPainted = bPainted;
		
		if(bIsPainted)
		{
			gameObject.renderer.material.mainTexture = PaintedTexture;
		}
		else
		{
			gameObject.renderer.material.mainTexture = UnpaintedTexture;
		}
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
	
	public void SetConnectedTilesHighlighted(bool bHighlighted)
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
