using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public int NumSpawnedCandles = 0;
	public int PointsPerCandle = 0;
	
	public List<GameObject> CandleInstances = new List<GameObject>();
}
