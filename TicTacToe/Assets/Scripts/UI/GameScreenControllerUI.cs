using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScreenControllerUI : MonoBehaviour {

	GameManager _gameManager;

	[SerializeField]
	Text _scoreText;

	[SerializeField]
	Text _messageText;

	//--------------------------------------------------------------
	//Unity methods
	//--------------------------------------------------------------

	private void OnEnable () {
		GameManager.OnGameStarted += OnGameStartedHandler;
		GameManager.OnGameOver += OnGameOverHandler;
		GameManager.OnMoveStarted += OnMoveStartedHandler;

		ScoreManager.OnScoresLoaded += OnScoresUpdatedHandler;
		ScoreManager.OnScoresUpdated += OnScoresUpdatedHandler;
	}

	private void OnDisable () {
		GameManager.OnGameStarted -= OnGameStartedHandler;
		GameManager.OnGameOver -= OnGameOverHandler;
		GameManager.OnMoveStarted -= OnMoveStartedHandler;

		ScoreManager.OnScoresLoaded -= OnScoresUpdatedHandler;
		ScoreManager.OnScoresUpdated -= OnScoresUpdatedHandler;
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public void Init (GameManager gameManager) {
		_gameManager = gameManager;
	}

	public void OnExitButtonClick () {
		Application.Quit ();
	}

	public void OnNewGameButtonClick () {
		_gameManager.StartNewGame ();
	}

	//--------------------------------------------------------------
	//Handlers
	//--------------------------------------------------------------

	private void OnGameStartedHandler () {
		//Show game start popup
	}

	private void OnGameOverHandler (CellState winner) {
		if (winner == CellState.NONE) {
			_messageText.text = "DRAW";
		} else {
			_messageText.text = _gameManager.CurrentPlayer.Mark.ToString () + " WIN";
		}

	}

	private void OnScoresUpdatedHandler () {
		_scoreText.text = ScoreManager.Player1Score.ToString () + " : " + ScoreManager.Player2Score.ToString ();
	}

	private void OnMoveStartedHandler () {
		_messageText.text = "TURN: " + _gameManager.CurrentPlayer.Mark.ToString ();
	}
}
