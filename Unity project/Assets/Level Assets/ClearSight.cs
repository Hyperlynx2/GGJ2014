using UnityEngine;
using System.Collections;
 
public class ClearSight : MonoBehaviour
{
    public float DistanceToPlayer = 20.0f;
    void Update()
    {
        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
		GameObject player = GameObject.Find ("Player");
        hits = Physics.CapsuleCastAll(transform.position - new Vector3(0, 3, 0),
				transform.position + new Vector3(0, 3, 0), 3.0f, transform.forward,
				(transform.position - player.transform.position).magnitude - 15);
		
        foreach(RaycastHit hit in hits)
        {
            Renderer R = hit.collider.renderer;
            if (R == null)
                continue; // no renderer attached? go to next hit
            // TODO: maybe implement here a check for GOs that should not be affected like the player
 
            AutoTransparent AT = R.GetComponent<AutoTransparent>();
            if (AT == null) // if no script is attached, attach one
            {
                AT = R.gameObject.AddComponent<AutoTransparent>();
            }
            AT.BeTransparent(); // get called every frame to reset the falloff
        }
    }
}