using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
 {
	/********\
	|settings|
	\********/
	public Tile startTile;
	public float movementSpeed;
	
	/*other stuff*/	
	private const float NORTH = 0f;
	private const float EAST = 90f;
	private const float SOUTH = 180f;
	private const float WEST = 270f;
	
	private float _heading;
	private Tile _currentTile;
	
	/// <summary>
	/// do not accept any movement change commands unless we're finished moving!
	/// </summary>
	private float _movementTimeRemaining;

	// Use this for initialization
	void Start ()
	{
		gameObject.transform.position = startTile.gameObject.transform.position + new Vector3(0, 0.5f, 0);

		_currentTile = startTile;
		_heading = NORTH;
		_movementTimeRemaining = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_movementTimeRemaining <= 0)
		{
			//TODO: take heading into consideration here		
			if(Input.GetAxis ("Horizontal") < 0) //left
			{
				MoveTo(_currentTile.WestTile);		
			}
			else if(Input.GetAxis("Horizontal") > 0) //right
			{
				MoveTo(_currentTile.EastTile);
			}
			else if(Input.GetAxis("Vertical") < 0) //down
			{
				MoveTo(_currentTile.SouthTile);
			}
			else if(Input.GetAxis("Vertical") > 0) //up
			{
				MoveTo(_currentTile.NorthTile);
			}
			
			
			//TODO: input for changing heading
		}
		else
		{
			_movementTimeRemaining -= Time.deltaTime;
			if(_movementTimeRemaining < 0)
				_movementTimeRemaining = 0;
		}
	
	}
	
	private void MoveTo(Tile destination)
	{
		/*turn off the lights on the current tile, turn em on on the new one, start movement anim, set
		 * movement timer, etc*/
		
		_currentTile.SetConnectedTilesHighlighted(false);
		destination.SetConnectedTilesHighlighted(true);
		
		_currentTile = destination;
		
		//TODO: replace with smooth transition, animation, etc:
		gameObject.transform.position = _currentTile.gameObject.transform.position + new Vector3(0, 0.5f, 0);
	
		_movementTimeRemaining = movementSpeed;
	}
}
