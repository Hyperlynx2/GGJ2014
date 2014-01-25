using UnityEngine;
using System.Collections;

public class Candle : MonoBehaviour {

	public Texture2D candle5;
	public Texture2D candle10;
	public Texture2D candle15;
	public Texture2D candle25;
	
	public int candleNum;
	
	
	// Use this for initialization
	void Start () {
		if(candleNum == 5)
		{
			renderer.material.mainTexture = candle5;
		}
		
		if(candleNum == 10)
		{
			renderer.material.mainTexture = candle10;
		}
		
		if(candleNum == 15)
		{
			renderer.material.mainTexture = candle15;
		}
		
		if(candleNum == 25)
		{
			renderer.material.mainTexture = candle25;
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
