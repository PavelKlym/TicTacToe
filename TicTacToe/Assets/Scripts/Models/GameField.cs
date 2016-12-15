using UnityEngine;
using System;
using System.Collections;

public class GameField {

	public static Action<Vector2, CellState> OnCellUpdated;
	public static Action OnFieldUpdated;
	public static Action<CellState> OnGameOver;


	private CellState[,] _gameField = null;
	private bool _isFieldInited = false;
	private bool _isGameOver = false;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public CellState[,] Cells {
		get {
			return (CellState[,])_gameField.Clone ();
		}
	}

	public bool IsGameOver {
		get {
			return _isGameOver;
		}
	}

	//--------------------------------------------------------------
	//Constructors
	//--------------------------------------------------------------

	public GameField () {
		_gameField = new CellState[3, 3];
		_isFieldInited = true;
		_isGameOver = false;

		for (int x = 0; x < _gameField.GetLength (0); x++) {
			for (int y = 0; y < _gameField.GetLength (0); y++) {
				_gameField [x, y] = CellState.EMPTY;
			}
		}
	}

	public bool TrySet (int x, int y, CellState mark) {
		//Check init
		if (_isFieldInited == false) {
			return false;
		}

		//Check game field size
		if (x < 0 || x > _gameField.GetLength (0) || y < 0 || y > _gameField.GetLength (1)) {
			return false;
		}

		//Check game field filling
		if (_gameField [x, y] != CellState.EMPTY) {
			return false;
		}

		_gameField [x, y] = mark;

		if (OnCellUpdated != null)
			OnCellUpdated (new Vector2(x, y), mark);

		if (OnFieldUpdated != null)
			OnFieldUpdated ();

		if (CheckGameOver ()) {
			_isGameOver = true;
		}

		return true;
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private bool CheckGameOver () {
		CellState winner = CellState.NONE;
		//bool is
		//Check who win or noone

		//Horizontal
		if (_gameField [0, 0] != CellState.EMPTY 
			&& _gameField [0, 0] == _gameField [0, 1] 
			&& _gameField [0, 1] == _gameField [0, 2]) {

			winner = _gameField [0, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		if (_gameField [1, 0] != CellState.EMPTY 
			&& _gameField [1, 0] == _gameField [1, 1] 
			&& _gameField [1, 1] == _gameField [1, 2]) {

			winner = _gameField [1, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		if (_gameField [2, 0] != CellState.EMPTY 
			&& _gameField [2, 0] == _gameField [2, 1] 
			&& _gameField [2, 1] == _gameField [2, 2]) {

			winner = _gameField [2, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		//Vertical
		if (_gameField [0, 0] != CellState.EMPTY 
			&& _gameField [0, 0] == _gameField [1, 0] 
			&& _gameField [1, 0] == _gameField [2, 0]) {

			winner = _gameField [0, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		if (_gameField [0, 1] != CellState.EMPTY 
			&& _gameField [0, 1] == _gameField [1, 1] 
			&& _gameField [1, 1] == _gameField [2, 1]) {

			winner = _gameField [0, 1];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		if (_gameField [0, 2] != CellState.EMPTY 
			&& _gameField [0, 2] == _gameField [1, 2] 
			&& _gameField [1, 2] == _gameField [2, 2]) {

			winner = _gameField [0, 2];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		//Cross
		if (_gameField [0, 0] != CellState.EMPTY 
			&& _gameField [0, 0] == _gameField [1, 1] 
			&& _gameField [1, 1] == _gameField [2, 2]) {

			winner = _gameField [0, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		if (_gameField [2, 0] != CellState.EMPTY 
			&& _gameField [2, 0] == _gameField [1, 1] 
			&& _gameField [1, 1] == _gameField [0, 2]) {

			winner = _gameField [2, 0];
			if (OnGameOver != null)
				OnGameOver (winner);
			
			return true;
		}

		bool isEmptyCellPresent = false;
		for (int h = 0; h < _gameField.GetLength (0); h++) {
			for (int v = 0; v < _gameField.GetLength (1); v++) {
				if (_gameField [h, v] == CellState.EMPTY) {
					isEmptyCellPresent = true;
				}
			}
		}

		if (isEmptyCellPresent == false) {
			if (OnGameOver != null)
				OnGameOver (CellState.NONE);

			return true;
		}
		
		return false;
	}
}
