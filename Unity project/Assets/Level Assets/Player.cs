using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
 {
	/********\
	|settings|
	\********/
	public Tile startTile;
	public float movementSpeed;
	
	
	public int PlayerScore = 0;
	
	/*other stuff*/	
	private const float NORTH = 0f;
	private const float EAST = 90f;
	private const float SOUTH = 180f;
	private const float WEST = 270f;
	
	private float _heading;
	private Tile _currentTile;
	private bool _initialised;
	
	private int _flagsCollected = 0;
	
	/// <summary>
	/// do not accept any movement change commands unless we're finished moving!
	/// </summary>
	private float _movementTimeRemaining;

	// Use this for initi alization
	void Start ()
	{
		gameObject.transform.position = startTile.gameObject.transform.position;

		_currentTile = startTile;
		_heading = NORTH;
		_movementTimeRemaining = 0;
		_initialised = false;
		
		PlayerScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!_initialised)
		{
			_initialised = true;
			_currentTile.SetConnectedTilesHighlighted(true);
		}		
		
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
		if(destination != null)
		{
			/*turn off the lights on the current tile, turn em on on the new one, start movement anim, set
			 * movement timer, etc*/
			
			
			destination.SetPainted(true);
			
			_currentTile.SetConnectedTilesHighlighted(false);
			destination.SetConnectedTilesHighlighted(true);
			
			//Testing
			
			_flagsCollected += destination.CollectTileFlags();
			
			if(destination.FlagGoalIsHere)
			{
				ScoreFlagsCollected();
			}
			
			_currentTile = destination;
			
			//TODO: replace with smooth transition, animation, etc:
			gameObject.transform.position = _currentTile.gameObject.transform.position;
		
			_movementTimeRemaining = movementSpeed;
		} 
	}
	
	public void AddCollectedFlags(int iNumberFlags)
	{
		_flagsCollected = iNumberFlags;
	}
	
	public void ScoreFlagsCollected()
	{
		PlayerScore = _flagsCollected * 5;
		_flagsCollected = 0;
	}
	
}
