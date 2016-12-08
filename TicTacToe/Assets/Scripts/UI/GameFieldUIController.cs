using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameFieldUIController : MonoBehaviour {

	public static Action<Vector2> OnCellClicked;

	[SerializeField]
	int _paddingPercent;
	[SerializeField]
	int _spacingPercent;

	[SerializeField]
	GridLayoutGroup _gridLayoutGroup;
	[SerializeField]
	RectTransform _rectTransform;

	[SerializeField]
	List<CellUI> _cells;

	static Sprite _noughtSprite;
	static Sprite _crossSprite;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public static Sprite GetNoughtSprite {
		get {
			return _noughtSprite;
		}
	}

	public static Sprite GetCrossSprite {
		get {
			return _crossSprite;
		}
	}

	//--------------------------------------------------------------
	//Unity methods
	//--------------------------------------------------------------

	private void Awake () {
		if (_noughtSprite == null || _crossSprite == null) {
			_noughtSprite = Resources.Load<Sprite> ("Nought") as Sprite;
			_crossSprite = Resources.Load<Sprite> ("Cross") as Sprite;
		}
	}

	private void OnEnable () {
		GameField.OnCellUpdated += OnGameFieldCellUpdatedHandler;
		GameManager.OnGameStarted += OnGameStartedHandler;

		for (int c = 0; c < _cells.Count; c++) {
			_cells [c].OnCellClicked += OnCellClickedHandler;
		}


	}

	private void OnDisable () {
		GameField.OnCellUpdated -= OnGameFieldCellUpdatedHandler;
		GameManager.OnGameStarted += OnGameStartedHandler;

		for (int c = 0; c < _cells.Count; c++) {
			_cells [c].OnCellClicked -= OnCellClickedHandler;
		}
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public void Init () {
		int padding = (int) (_rectTransform.rect.width * _paddingPercent / 100f);
		int spacing = (int) (_rectTransform.rect.width * _spacingPercent / 100f);
		_gridLayoutGroup.padding = new RectOffset (padding, padding, padding, padding);
		_gridLayoutGroup.spacing = new Vector2 (spacing, spacing);
		_gridLayoutGroup.cellSize = new Vector2 ((_rectTransform.rect.width - 2 * padding - 2 * spacing) / 3f, (_rectTransform.rect.height - 2 * padding - 2 * spacing) / 3f);
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private void ClearField () {
		for (int c = 0; c < _cells.Count; c++) {
			_cells [c].SetState (CellState.Empty);
		}
	}
		
	//--------------------------------------------------------------
	//Handlers
	//--------------------------------------------------------------

	private void OnGameStartedHandler () {
		ClearField ();
	}

	private void OnCellClickedHandler (Vector2 position) {
		if (OnCellClicked != null) {
			OnCellClicked (position);
		}
	}

	private void OnGameFieldCellUpdatedHandler (Vector2 position, CellState mark) {
		foreach (CellUI cell in _cells) {
			if (cell.Position == position) {
				cell.SetState (mark);
			}
		}
	}
}
