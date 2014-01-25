using UnityEngine;
using System.Collections;

public class DestroyObjectOnParticleFinish : MonoBehaviour {
	
	private ParticleSystem _ps;
	
	// Use this for initialization
	void Start () {
		_ps = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_ps.isStopped)
		{
			DestroyObject(gameObject);
		}
	}
}
