using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static Action OnGameStarted;
	public static Action<CellState> OnGameOver;

	public static Action OnMoveStarted;
	public static Action OnMoveEnded;

	private bool _isGameOver = false;

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
			return _isGameOver;
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

		//StartNewMove ();

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

		StartNewMove ();

		if (OnGameStarted != null) {
			OnGameStarted ();
		}
	}

	public bool TrySet (Vector2 position, CellState mark) {
		if (_gameField.TrySet ((int)position.x, (int)position.y, mark)) {
			_currentPlayer.EndMove ();

			if (OnMoveEnded != null)
				OnMoveEnded ();

			if (_isGameOver == false) {
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
		_isGameOver = false;

		Subscribe ();

		//Model
		_gameField = new GameField ();

		//View
		_screenController.Init (this);

		//Controller
		int player1ID = 123;
		_player1 = new HumanPlayer (player1ID, CellState.CROSS, this);
		int player2ID = 321;
		_player2 = new HumanPlayer (player2ID, CellState.NOUGHT, this);

	}

	private void StartNewMove () {
		Debug.Log ("New move");

		_currentPlayer.GiveTurn ();

		if (OnMoveStarted != null) {
			OnMoveStarted ();
		}
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

	public void Subscribe () {
		GameField.OnGameOver += OnGameOverHandler;
	}

	public void Unsubscribe () {
		GameField.OnGameOver -= OnGameOverHandler;
	}

	private void OnGameOverHandler (CellState winner) {
		_isGameOver = true;

		Unsubscribe ();

		if (OnGameOver != null) {
			OnGameOver (winner);
		}
		Debug.Log ("---------------------------GAME OVER " + winner.ToString () + "---------------------------");

		Invoke ("StartNewGame", 2.0f);
	}
}
