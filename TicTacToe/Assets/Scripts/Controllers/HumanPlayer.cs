using UnityEngine;
using System.Collections;

public class HumanPlayer : Player {

	//--------------------------------------------------------------
	//Constructors
	//--------------------------------------------------------------

	public HumanPlayer (int ID, CellState mark, GameManager manager) : base (ID, mark, manager) {
		Subscribe ();
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private void Subscribe () {
		GameManager.OnGameOver += OnGameOverHandler;

		GameFieldUIController.OnCellClicked += OnCellClickedHandler;
	}

	private void Unsubscribe () {
		GameManager.OnGameOver -= OnGameOverHandler;

		GameFieldUIController.OnCellClicked -= OnCellClickedHandler;
	}
		

	private void OnGameOverHandler (CellState winner) {
		Unsubscribe ();
	}

	private void OnCellClickedHandler (Vector2 position) {
		if (_isMyTurn == false)
			return;

		_manager.TrySet (position, _mark);
	}
}
