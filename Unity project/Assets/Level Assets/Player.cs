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
	public string player1RotateAxis;
	public string player2RotateAxis;
	public string[] playerNames;
	public int flagScoreValue = 5;
	
	public AnimationCurve jumpCurve;
	
	/// <summary>
	/// How much time each player gets per turn.
	/// </summary>
	public float turnTime;
	
	private int _heading;
	private Tile _currentTile;
	private Tile _destination;
	private bool _initialised;
	
	private float _currentMoveSpeed;
	private Vector3 _velocity;
	
	private bool _travelledThroughTeleporter;
	
	private Animator _playerAnimator;
	private Renderer _renderer;
	
	
	private Camera _camera;
	private Quaternion _cameraTargetRotation;
	private Transform _cameraPivot;
	
	
	private AudioSource _playerPainterLoop;
	private AudioSource _playerCounterLoop;
	private AudioSource _playerChangeSound;
	
	private bool _rotatedThisFrame;
	
	
	private bool _teleporting;
	
	/// <summary>
	/// The _flags currently carried (not total or player score!)
	/// </summary>
	private int _flagsCarried = 0;
	
	private int[] _playerScores = {0,0};
	
	/// <summary>
	/// do not accept any movement change commands unless we're finished moving!
	/// </summary>
	private float _movementTimeRemaining;
	
	/// <summary>
	/// Time remaining before contorl switches to the other player;
	/// </summary>
	private float _playerTurnRemaining;
	
	public enum PLAYER_ID
	{
		PAINTER,
		COLLECTOR
	}
	
	private PLAYER_ID _currentPlayer;
	
	// Use this for initialization
	void Start ()
	{
		_teleporting = false;
		
		gameObject.transform.position = startTile.gameObject.transform.position;
		_currentPlayer = PLAYER_ID.COLLECTOR;

		_currentTile = startTile;
		_heading = 0;
		_movementTimeRemaining = 0;
		_initialised = false;
		_travelledThroughTeleporter = false;
		
		_heading = 0;
		
		_playerAnimator = GetComponentInChildren<Animator>();
		_camera = GetComponentInChildren<Camera>();
		_renderer = GetComponentInChildren<Renderer>();
		
		_cameraPivot = gameObject.transform.FindChild("CameraPivot");
		
		_playerPainterLoop = transform.FindChild ("Sounds").transform.FindChild("PlayerOneLoop").GetComponent<AudioSource>();
		_playerCounterLoop = transform.FindChild ("Sounds").transform.FindChild("PlayerTwoLoop").GetComponent<AudioSource>();
		_playerChangeSound = transform.FindChild ("Sounds").transform.FindChild("PlayerChange").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*no guaruntees that Start will get called only after the tiles have been generated, so
		initialise here on the first frame*/
		
		if(!_initialised)
		{
			_initialised = true;
			_currentTile.OnTileEnter(_currentPlayer);
		}
		
		_cameraTargetRotation = Quaternion.AngleAxis(  _heading, Vector3.up);
		_cameraPivot.rotation = Quaternion.Slerp(_cameraPivot.rotation, _cameraTargetRotation, Time.deltaTime * 2.5f);

		UpdateTurnSwitch();
		
		if(_teleporting) return;
		
		UpdateInput();	
		UpdateMovement();
	}
	
	void OnGUI()
	{
		//whose turn it is, time remaining:
		//TODO: make time remaining be formatted in minutes:seconds
		GUI.Box (new Rect (500, 200,100,50), playerNames[(int)_currentPlayer] + "\n" + _playerTurnRemaining);
		
		string scoreText = "";
		
		for(int i = 0; i < playerNames.Length; ++i)
		{
			scoreText += playerNames[i] + ": " + _playerScores[i] + "\n";
		}		
		
		//score:
		GUI.Box (new Rect (500, 400,100,50), scoreText);
	}

	/// <summary>
	/// Handle turn timer. Decrement time, switch controls if necessary.
	/// </summary>
	private void UpdateTurnSwitch()
	{
		_playerTurnRemaining -= Time.deltaTime;
		
		if(_playerTurnRemaining <= 0)
		{
			_playerChangeSound.Play();
		
			if(_currentPlayer == PLAYER_ID.PAINTER)
			{
				_currentPlayer = PLAYER_ID.COLLECTOR;
				
				float fMusicTime = _playerPainterLoop.time;
				_playerPainterLoop.Stop();
				_playerCounterLoop.time = fMusicTime;
				_playerCounterLoop.Play();
			}
			else
			{
				_currentPlayer = PLAYER_ID.PAINTER;
				
				float fMusicTime = _playerCounterLoop.time;
				_playerCounterLoop.Stop();
				_playerPainterLoop.time = fMusicTime;
				_playerPainterLoop.Play();
			}
			
			_playerTurnRemaining = turnTime;
			
			if(_movementTimeRemaining <= 0)
				_currentTile.OnTileEnter(_currentPlayer);
		}
	}
	
	private void UpdateInput()
	{
		//do not accept new movement command if we're currently in the process of moving!
		if(_movementTimeRemaining <= 0)
		{
			string horz = player1HorizontalAxis;
			string vert = player1VerticalAxis;
			string rotate = player1RotateAxis;
			
			if(_currentPlayer == PLAYER_ID.COLLECTOR)
			{
				horz = player2HorizontalAxis;
				vert = player2VerticalAxis;
				rotate = player2RotateAxis;
			}
			
			Tile[] exits = {_currentTile.NorthTile,
							_currentTile.EastTile,
							_currentTile.SouthTile,
							_currentTile.WestTile};
			
			int headingOffset = _heading / 90;
			
			if(Input.GetAxis(vert) > 0) //up
			{
				StartMovingTo(exits[headingOffset]);
			}
			else if(Input.GetAxis(horz) > 0) //right
			{
				StartMovingTo(exits[(headingOffset + 1) % 4]);
			}
			else if(Input.GetAxis(vert) < 0) //down
			{
				StartMovingTo(exits[(headingOffset + 2) % 4]);
			}
			else if(Input.GetAxis(horz) < 0) //left
			{
				StartMovingTo(exits[(headingOffset + 3) % 4]);
			}
			else
			{
				_playerAnimator.SetBool("bHaveDestination", false);
			}
			
			
			
			
			/*for camera rotation, use transform.rotatearound to make it look nice.
			(http://docs.unity3d.com/Documentation/ScriptReference/Transform.RotateAround.html)
			
			From a mechanical point of view, I think the change of control orientation should be
			instantaneous, ie before the camera has finished rotating.
			
			Can do the rotating in a script on the camera, in its update()... but might as well do it
			here.
			*/
			
			/*TODO: regarding the camera rotation, cheat the same way I did with the player movement.
			find and store the 90 degree points at start, have a setting for time to rotate 90
			degrees, while rotating rotate that amount * deltaTime and decrement deltaTime from
			current rotation time remaining, when rotation time <=0 translate straight to the
			precalculated point. That way it won't go out of synch.*/
			
			
				
			if(Input.GetAxis(rotate) < 0)
			{
				if(!_rotatedThisFrame)
				{
					//_camera.transform.RotateAround(gameObject.transform.position, Vector3.up,  -90);
					_rotatedThisFrame = true;
					
					_heading -= 90;
					
					if(_heading < 0)
						_heading = 270;
				}
			}
			else if(Input.GetAxis(rotate) > 0)
			{
				if(!_rotatedThisFrame)
				{
					//_camera.transform.RotateAround(gameObject.transform.position, Vector3.up,  90);
					_rotatedThisFrame = true;
					
					_heading += 90;
					if(_heading >= 360)
						_heading = 0;
				}
			}
			else
			{
				_rotatedThisFrame = false;
			}
			
		}
	}
	
	/// <summary>
	/// Abstract movement, in case we want to do fancy things with trajectory later.
	/// </summary>
	private void UpdateMovement()
	{
		if(_destination != null)
		{
			
			Vector3 toDestination = (_destination.gameObject.transform.position - _currentTile.gameObject.transform.position);
			
			float speed = toDestination.magnitude / _currentMoveSpeed;
			
			gameObject.transform.position += toDestination.normalized * speed * Time.deltaTime;
			
			float yMod = Mathf.Abs (_destination.gameObject.transform.position.y - _currentTile.gameObject.transform.position.y);
			//float xMod = Mathf.Abs (_destination.gameObject.transform.position.x - _currentTile.gameObject.transform.position.x);
			float fEvalutateValue = 1 - (_movementTimeRemaining / _currentMoveSpeed);
			float fYOffset = jumpCurve.Evaluate(fEvalutateValue ) 
				* (yMod);
			gameObject.transform.position += new Vector3(0, fYOffset, 0);
			
			_movementTimeRemaining -= Time.deltaTime;
			//if(fYOffset > 0)
			//{
			//	Debug.Log (fEvalutateValue);
			//	Debug.Log (yMod);
			//	//Debug.Log (xMod);
			//	Debug.Log (fYOffset);
			//	
			//}
			if(_movementTimeRemaining < 0)
			{
				_movementTimeRemaining = 0;
				
				//cheating, just in case things don't line up:
				gameObject.transform.position = _destination.gameObject.transform.position;
				ArriveAtDestination();				
			}
			
		}
	}
	
	/// <summary>
	/// Begin movement to destination tile
	/// </summary>
	private void StartMovingTo(Tile destination)
	{
		if(destination != null)
		{
			//TODO: start playing movement anim
			
			_currentTile.OnTileExit(_currentPlayer);
			_destination = destination;
				
			Vector3 toDestination = (_destination.gameObject.transform.position - _currentTile.gameObject.transform.position);
			_currentMoveSpeed = movementSpeed * Mathf.Pow((toDestination.magnitude / 4), 0.5f);

			
			_movementTimeRemaining = _currentMoveSpeed;
			
			_playerAnimator.SetBool("bHaveDestination", true);
			
			if(_destination.transform.position.y < _currentTile.transform.position.y)
			{
				_playerAnimator.SetBool("bHaveDestination", false);
				_playerAnimator.SetBool("bFalling", true);
			}
		} 
	}
		
	private void ArriveAtDestination()
	{
		
		_renderer.enabled = true;
		_playerAnimator.SetBool("bJumping", false);
		_playerAnimator.SetBool("bFalling", false);
		
		HandleScoring(_destination);
		
		_currentTile = _destination;
		_destination = null;
		_currentTile.OnTileEnter(_currentPlayer); //if the current tile is a teleporter, this should start the TP anim
		
		//now cope with teleporters
		Tile specialDest = _currentTile.GetConnectedTeleporterTile();
		if(specialDest != null && _travelledThroughTeleporter == false)
		{
			//gameObject.transform.position = specialDest.gameObject.transform.position;
			///*NB: DO NOT treat this as arriving at a destination. otherwise you'll infinitely
			//teleport between the two, and overflow stack space.
			//
			//a jump pad CANNOT occupuy the same space as a teleporter (which you might want
			//with the idea of teleporting onto a jump pad), because then if you were to ordinarily
			//walk onto that jump pad/teleporter what would the correct action be? to jump or to
			//teleport?*/
			//
			//_currentTile = specialDest;
			//_currentTile.OnTileSpecialEnter(_currentPlayer);
			//HandleScoring(_currentTile);
			_playerAnimator.SetBool("bHaveDestination", false);
			StartCoroutine(DoTeleport(specialDest));
			_travelledThroughTeleporter = true;
		}
		else
		{
			
			_travelledThroughTeleporter = false;
			
			
			specialDest = _currentTile.GetConnectedJumperTile();
			
			if(specialDest != null)
			{
				StartMovingTo(specialDest);
				_playerAnimator.SetBool("bJumping", true);
			}
		}
		
		
	}
	
	/// <summary>
	/// Paint the tile/pick up flags/score flags etc.
	/// </summary>
	private void HandleScoring(Tile scoreThisTile)
	{
		if(_currentPlayer == PLAYER_ID.PAINTER)
		{
			if(scoreThisTile.PaintTile())
			{
				++_playerScores[(int)_currentPlayer];
			}
		}
		else
		{
			_flagsCarried += scoreThisTile.CollectTileFlags();
			
			if(scoreThisTile.FlagGoalIsHere)
			{
				_playerScores[(int)_currentPlayer] += _flagsCarried * flagScoreValue;
				_flagsCarried = 0;
			}
		}
	}
	
	private IEnumerator DoTeleport(Tile destination)
	{
		_teleporting = true;
		
		_playerAnimator.SetBool("bTeleporting", true);
		yield return new WaitForSeconds(0.25f);
		_renderer.enabled = false;
		
		yield return new WaitForSeconds(0.5f);
		gameObject.transform.position = destination.gameObject.transform.position;
		
		//float fDistFromTarget = (destination.transform.position - transform.position).magnitude;
		//while(fDistFromTarget > 1)
		//{
		//	
		//	gameObject.transform.position = Mathf.Lerp(
		//	
		//	yield return new WaitForSeconds(1.0f);
		//}
		
		_currentTile = destination;
		_currentTile.OnTileSpecialEnter(_currentPlayer);
		yield return new WaitForSeconds(0.25f);
		_renderer.enabled = true;
		_playerAnimator.SetBool("bTeleporting", false);
		
		_teleporting = false;
	}
		
}
