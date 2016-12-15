using UnityEngine;
using System;
using System.Collections;

public class Player {

	protected GameManager _manager;

	protected int _ID;
	protected bool _isMyTurn = false;
	protected CellState _mark;

	//--------------------------------------------------------------
	//Get/Set
	//--------------------------------------------------------------

	public int ID {
		get {
			return _ID;
		}
	}

	public CellState Mark {
		get {
			return _mark;
		}
	}

	//--------------------------------------------------------------
	//Constructors
	//--------------------------------------------------------------

	public Player (int ID, CellState mark, GameManager manager) {
		this._ID = ID;
		this._mark = mark;
		this._manager = manager;
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public virtual void GiveTurn () {
		_isMyTurn = true;
	}

	public virtual void EndMove () {
		_isMyTurn = false;
	}

	public override string ToString () {
		return string.Format ("[Player: ", ID.ToString (), Mark.ToString ());
	}
}
