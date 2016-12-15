using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static Action OnGameStarted;
	public static Action<CellState> OnGameOver;

	public static Action<CellState> OnMoveStarted;
	public static Action OnMoveEnded;

	//Model
	GameField _gameField;

	//View
	[SerializeField]
	GameScreenControllerUI _screenController;
	[SerializeField]
	ScoreManager _scoreManager;

	//Players
	Player _player1;
	Player _player2;

	Player _currentPlayer;
	Player _lastGameFirstTurn;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public Player Player1 {
		get {
			return _player1;
		}
	}

	public Player Player2 {
		get {
			return _player2;
		}
	}

	public Player CurrentPlayer {
		get {
			return _currentPlayer;
		}
	}

	public bool IsGameOver {
		get {
			return _gameField.IsGameOver;
		}
	}

	public CellState[,] GetCells {
		get {
			/*int size = _gameField.Cells.GetLength ();
			CellState[,] cells = new CellState[,];
			return Array.Copy (_cells, 0, cells, 0, size);*/

			return (CellState[,])_gameField.Cells;
		}
	}

	//--------------------------------------------------------------
	//Unity methods
	//--------------------------------------------------------------

	void Awake () {
		_scoreManager.Init (this);

		StartNewGame ();
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public void StartNewGame () {
		Debug.Log ("---------------------------NEW GAME---------------------------");

		Init ();

		if (_lastGameFirstTurn == null) {
			_currentPlayer = _player1;
		} else {
			if (_lastGameFirstTurn.ID == _player1.ID) {
				_currentPlayer = _player2;
			} else {
				_currentPlayer = _player1;
			}
		}
		_lastGameFirstTurn = _currentPlayer;

		if (OnGameStarted != null) {
			OnGameStarted ();
		}

		StartNewMove ();

		Debug.Log ("Player1 " + _player1.ToString ());
		Debug.Log ("Player2 " + _player2.ToString ());
	}

	public void Restart () {
		GameOver (CellState.NONE);

		StartNewGame ();
	}

	public bool TrySet (Vector2 position, CellState mark) {
		if (_gameField.TrySet ((int)position.x, (int)position.y, mark)) {
			_currentPlayer.EndMove ();

			if (OnMoveEnded != null)
				OnMoveEnded ();

			if (_gameField.IsGameOver == false) {
				SwitchPlayer ();
				StartNewMove ();
			} 

			return true;
		}

		return false;
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private void Init () {
		Subscribe ();

		//Model
		_gameField = new GameField ();

		//View
		_screenController.Init (this);

		//Controller
		int player1ID = 123;
		_player1 = new HumanPlayer (player1ID, CellState.CROSS, this);
		int player2ID = 321;
		//_player2 = new HumanPlayer (player2ID, CellState.NOUGHT, this);
		int size = _gameField.Cells.GetLength (0);
		//CellState[,] cells = _gameField.Cells;

		_player2 = new AIPlayer (player2ID, CellState.NOUGHT, this, 4);
	}

	private void StartNewMove () {
		Debug.Log ("New move");

		if (OnMoveStarted != null) {
			OnMoveStarted (_currentPlayer.Mark);
		}

		_currentPlayer.GiveTurn ();
	}

	private void SwitchPlayer () {
		if (_currentPlayer == null) {
			_currentPlayer = _player1;
			return;
		}

		if (_currentPlayer.ID == _player1.ID) {
			_currentPlayer = _player2;
		} else {
			_currentPlayer = _player1;
		}

		Debug.Log ("Switch player to " + _currentPlayer.Mark);
	}

	private void Subscribe () {
		GameField.OnGameOver += OnGameOverHandler;
	}

	private void Unsubscribe () {
		GameField.OnGameOver -= OnGameOverHandler;
	}

	private void GameOver (CellState winner) {
		Unsubscribe ();

		if (OnGameOver != null) {
			OnGameOver (winner);
		}
		Debug.Log ("---------------------------GAME OVER " + winner.ToString () + "---------------------------");
	}

	//--------------------------------------------------------------
	//Handlers
	//--------------------------------------------------------------


	private void OnGameOverHandler (CellState winner) {
		GameOver (winner);

		Invoke ("StartNewGame", 2.0f);
	}
}
