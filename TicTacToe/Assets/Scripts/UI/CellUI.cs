using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CellUI : MonoBehaviour {

	public Action<Vector2> OnCellClicked;

	[SerializeField]
	Vector2 _position;

	[SerializeField]
	CellState _state;

	[SerializeField]
	Image _stateImage;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public Vector2 Position {
		get {
			return _position;
		}
	}

	public CellState State {
		get {
			return _state;
		}
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public void SetState (CellState state) {
		_state = state;

		switch (_state) {

		case CellState.EMPTY:
			_stateImage.sprite = null;
			break;

		case CellState.CROSS:
			_stateImage.sprite = GameFieldUIController.GetCrossSprite;
			break;

		case CellState.NOUGHT:
			_stateImage.sprite = GameFieldUIController.GetNoughtSprite;
			break;
		}
	}

	public void OnCellClick () {
		if (OnCellClicked != null) {
			OnCellClicked (_position);
		}
	}
}
