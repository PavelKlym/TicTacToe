using UnityEngine;
using System;
using System.Collections;

//Need to rewrite script, too much hard code
public class ScoreManager : MonoBehaviour {

	GameManager _gameManager;

	public static Action OnScoresLoaded;
	public static Action OnScoresUpdated;
	public static Action OnScoresSubmited;

	private const string SCORE_KEY = "TIC_TAC_TOE_SCORE";

	private static int _player1Score = 0;
	private static int _player2Score = 0;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public static int Player1Score {
		get {
			return _player1Score;
		}
	}

	public static int Player2Score {
		get {
			return _player2Score;
		}
	}

	//--------------------------------------------------------------
	//Unity methods
	//--------------------------------------------------------------

	void OnEnable () {
		GameManager.OnGameOver += OnGameOverHandler;
	}

	void Start () {
		LoadScore ();
	}

	void OnDisable () {
		SubmitScore ();

		GameManager.OnGameOver -= OnGameOverHandler;
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public void Init (GameManager gameManager) {
		_gameManager = gameManager;
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private void LoadScore () {
		if (PlayerPrefs.HasKey (SCORE_KEY)) {
			string savedScore = PlayerPrefs.GetString (SCORE_KEY);
			string[] scores = savedScore.Split (new string[] { "," }, System.StringSplitOptions.None);
			try {
				
				_player1Score = Int32.Parse (scores [0]);
				_player2Score = Int32.Parse (scores [1]);
			} catch (Exception e) {
				
				_player1Score = 0;
				_player2Score = 0;

				Debug.Log ("Loading score error! " + e.ToString ());
			}
		}

		if (OnScoresLoaded != null) {
			OnScoresLoaded ();
		}
	}

	private void SubmitScore () {
		PlayerPrefs.SetString (SCORE_KEY, _player1Score.ToString () + "," + _player2Score.ToString ());

		if (OnScoresSubmited != null) {
			OnScoresSubmited ();
		}
	}

	private void OnGameOverHandler (CellState mark) {
		switch (mark) {

		case CellState.Cross:
			_player1Score++;
			break;

		case CellState.Nought:
			_player2Score++;
			break;
		}

		if (OnScoresUpdated != null) {
			OnScoresUpdated ();
		}
	}
}
