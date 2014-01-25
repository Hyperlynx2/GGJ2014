using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
 {
	/********\
	|settings|
	\********/
	public Tile startTile;
	public float movementSpeed;
	public string player1HorizontalAxis;
	public string player1VerticalAxis;
	public string player2HorizontalAxis;
	public string player2VerticalAxis;
	public string[] playerNames;
	
	/// <summary>
	/// How much time each player gets per turn.
	/// </summary>
	public float turnTime;
	
	
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
	
	/// <summary>
	/// Time remaining before contorl switches to the other player;
	/// </summary>
	private float _playerTurnRemaining;
	
	public enum PLAYER_NUM
	{
		P1,
		P2
	}
	
	private PLAYER_NUM _currentPlayer;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.transform.position = startTile.gameObject.transform.position;
		_currentPlayer = PLAYER_NUM.P2;

		_currentTile = startTile;
		_heading = NORTH;
		_movementTimeRemaining = 0;
		_initialised = false;
		
		PlayerScore = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*no guaruntees that Start will get called only after the tiles have been generated, so
		initialise here on the first frame*/
		if(!_initialised)
		{
			_initialised = true;
			_currentTile.SetConnectedTilesHighlighted(true);
		}
		
		UpdateTurnSwitch();
		UpdateInput();	
	}
	
	void OnGUI()
	{
		//TODO: make time remaining be formatted in minutes:seconds
		GUI.Box (new Rect (500, 200,100,50), playerNames[(int)_currentPlayer] + "\n" + _playerTurnRemaining);
	}

	/// <summary>
	/// Handle turn timer. Decrement time, switch controls if necessary.
	/// </summary>
	private void UpdateTurnSwitch()
	{
		_playerTurnRemaining -= Time.deltaTime;
		
		if(_playerTurnRemaining <= 0)
		{
		
			if(_currentPlayer == PLAYER_NUM.P1)
				_currentPlayer = PLAYER_NUM.P2;
			else
				_currentPlayer = PLAYER_NUM.P1;
			
			_playerTurnRemaining = turnTime;
		}
	}
	
	private void UpdateInput()
	{
		if(_movementTimeRemaining <= 0)
		{
			string horz = player1HorizontalAxis;
			string vert = player1VerticalAxis;
			
			if(_currentPlayer == PLAYER_NUM.P2)
			{
				horz = player2HorizontalAxis;
				vert = player2VerticalAxis;
			}
			
			//TODO: take heading into consideration here		
			if(Input.GetAxis (horz) < 0) //left
			{
				MoveTo(_currentTile.WestTile);		
			}
			else if(Input.GetAxis(horz) > 0) //right
			{
				MoveTo(_currentTile.EastTile);
			}
			else if(Input.GetAxis(vert) < 0) //down
			{
				MoveTo(_currentTile.SouthTile);
			}
			else if(Input.GetAxis(vert) > 0) //up
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
	
	/// <summary>
	/// Handle movement to the destination tile.
	/// </summary>
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
