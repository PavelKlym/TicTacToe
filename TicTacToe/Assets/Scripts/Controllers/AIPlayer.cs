using UnityEngine;
using System;
using System.Collections.Generic;


public class AIPlayer : Player {

	private int _minimaxDepth = 1;
	CellState[,] _cells;
	CellState _opponentMark;

	int iterations = 0;

	//--------------------------------------------------------------
	//Constructors
	//--------------------------------------------------------------

	public AIPlayer (int ID, CellState mark, GameManager manager, int minimaxDepth) : base (ID, mark, manager) {
		_minimaxDepth = minimaxDepth;

		if (mark == CellState.CROSS) {
			_opponentMark = CellState.NOUGHT;
		} else {
			_opponentMark = CellState.CROSS;
		}
	}

	//--------------------------------------------------------------
	//Public methods
	//--------------------------------------------------------------

	public override void GiveTurn () {
		_isMyTurn = true;

		_cells = _manager.GetCells;
		iterations = 0;
		int[] res = Minimax (_minimaxDepth, _opponentMark);
		_manager.TrySet (new Vector2 (res[1], res[2]), _mark);
		Debug.Log ("iterations " + iterations);
	}

	public override void EndMove () {
		_isMyTurn = false;
	}

	//--------------------------------------------------------------
	//Private methods
	//--------------------------------------------------------------

	private int[] Minimax(int depth, CellState player) {
		List<int[]> nextMoves = GenerateMoves();
		iterations++;
		int bestScore = (player == _mark) ? Int32.MinValue : Int32.MaxValue;
		int currentScore;
		int bestRow = -1;
		int bestCol = -1;

		if (nextMoves.Count == 0 || depth == 0) {
			bestScore = Evaluate();
		} else {
			foreach (int[] move in nextMoves) {
				_cells[move[0], move[1]] = player;
				if (player == _mark) { 
					currentScore = Minimax(depth - 1, _opponentMark)[0];
					if (currentScore > bestScore) {
						bestScore = currentScore;
						bestRow = move[0];
						bestCol = move[1];
					}
				} else { 
					currentScore = Minimax(depth - 1, _mark)[0];
					if (currentScore < bestScore) {
						bestScore = currentScore;
						bestRow = move[0];
						bestCol = move[1];
					}
				}

				_cells[move[0], move[1]] = CellState.EMPTY;
			}
		}
		return new int[] {bestScore, bestRow, bestCol};
	}

	private List<int[]> GenerateMoves() {
		List<int[]> nextMoves = new List<int[]>(); 

		for (int row = 0; row < 3; ++row) {
			for (int col = 0; col < 3; ++col) {
				if (_cells[row, col] == CellState.EMPTY) {
					nextMoves.Add(new int[] {row, col});
				}
			}
		}
		return nextMoves;
	}

	private int Evaluate() {
		int score = 0;

		score += EvaluateLine(0, 0, 0, 1, 0, 2);  // row 0
		score += EvaluateLine(1, 0, 1, 1, 1, 2);  // row 1
		score += EvaluateLine(2, 0, 2, 1, 2, 2);  // row 2
		score += EvaluateLine(0, 0, 1, 0, 2, 0);  // col 0
		score += EvaluateLine(0, 1, 1, 1, 2, 1);  // col 1
		score += EvaluateLine(0, 2, 1, 2, 2, 2);  // col 2
		score += EvaluateLine(0, 0, 1, 1, 2, 2);  // diagonal
		score += EvaluateLine(0, 2, 1, 1, 2, 0);  // alternate diagonal

		return score;
	}
		
	private int EvaluateLine(int row1, int col1, int row2, int col2, int row3, int col3) {
		int score = 0;

		if (_cells[row1, col1] == _mark) {
			score = 1;
		} else if (_cells[row1, col1] == _opponentMark) {
			score = -1;
		}

		if (_cells[row2, col2] == _mark) {
			if (score == 1) {  
				score = 10;
			} else if (score == -1) {  
				return 0;
			} else {  
				score = 1;
			}
		} else if (_cells[row2, col2] == _opponentMark) {
			if (score == -1) {
				score = -10;
			} else if (score == 1) { 
				return 0;
			} else { 
				score = -1;
			}
		}

		if (_cells[row3, col3] == _mark) {
			if (score > 0) {  
				score *= 10;
			} else if (score < 0) { 
				return 0;
			} else {  
				score = 1;
			}
		} else if (_cells[row3, col3] == _opponentMark) {
			if (score < 0) {  
				score *= 10;
			} else if (score > 1) {  
				return 0;
			} else {  
				score = -1;
			}
		}
		return score;
	}
}
