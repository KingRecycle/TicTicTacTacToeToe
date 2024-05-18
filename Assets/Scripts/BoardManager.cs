using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharlieMadeAThing.TicTicTacTacToeToe {
    public enum WinState {
        Red,
        Blue,
        Draw,
    }
    public class BoardManager : MonoBehaviour {
        [SerializeField] GameManager gameManager;
        [SerializeField] GamePiece gamePiecePrefab;
        [SerializeField] Transform teamRedParent;
        [SerializeField] Transform teamBlueParent;
    
        Stack<GamePiece>[] _board = new Stack<GamePiece>[9];
        GamePiece[] _teamRed = new GamePiece[6];
        GamePiece[] _teamBlue = new GamePiece[6];
        GamePiece[] _inHandRedPieces = new GamePiece[6];
        GamePiece[] _inHandBluePieces = new GamePiece[6];

        readonly List<List<int>> _winningCombinations = new() {
            new List<int> {0, 1, 2}, // Horizontal lines
            new List<int> {3, 4, 5},
            new List<int> {6, 7, 8},
            new List<int> {0, 3, 6}, // Vertical lines
            new List<int> {1, 4, 7},
            new List<int> {2, 5, 8},
            new List<int> {0, 4, 8}, // Diagonal lines
            new List<int> {2, 4, 6}
        };
        
    
        void Start() {
            for (var i = 0; i < _board.Length; i++) {
                _board[i] = new Stack<GamePiece>(6);
            }
        
            for (var i = 0; i < 6; i++) {
                var redPiece = Instantiate(gamePiecePrefab);
                redPiece.SetPiece((PieceLevel)i, PieceTeam.Red);
                redPiece.transform.SetPositionAndRotation( teamRedParent.GetChild(i).position, Quaternion.identity );
                redPiece.name = $"Red Piece {i + 1}";
                _teamRed[i] = redPiece;
                _inHandRedPieces[i] = redPiece;
            
                var bluePiece = Instantiate(gamePiecePrefab);
                bluePiece.SetPiece((PieceLevel)i, PieceTeam.Blue);
                bluePiece.transform.SetPositionAndRotation( teamBlueParent.GetChild(i).position, Quaternion.identity );
                bluePiece.name = $"Blue Piece {i + 1}";
                _teamBlue[i] = bluePiece;
                _inHandBluePieces[i] = bluePiece;
            }
        }
        
        public (bool, PieceTeam) CheckForWin() {
            foreach ( var pieces in _winningCombinations
                         .Select( combination => combination.Select(GetTopPiece).ToList() )
                         .Where( pieces => pieces.All(piece => piece && piece.GetTeam() == pieces[0].GetTeam()) ) ) {
                gameManager.EndGame( pieces[0].GetTeam() == PieceTeam.Red ? WinState.Red : WinState.Blue );
                return (true, pieces[0].GetTeam());
            }

            return (false, PieceTeam.Red); // Return Red as default, this will not matter as the first item in the tuple is false
        }

        public bool CheckForDraw() {
            var currentTurn = gameManager.GetCurrentTurn();
            
            // If the current team has pieces in hand and there is a valid move on the board, return false.
            for (var i = 0; i < 6; i++) {
                if ( currentTurn == PieceTeam.Red ) {
                    if ( _inHandRedPieces[i] == null ) continue;
                    
                    for (var j = 0; j < _board.Length; j++) {
                        if ( IsValidMove(_inHandRedPieces[i], j) ) {
                            return false;
                        }
                    }
                }
                else {
                    if ( _inHandBluePieces[i] == null ) continue;
                    
                    for (var j = 0; j < _board.Length; j++) {
                        if ( IsValidMove(_inHandBluePieces[i], j) ) {
                            return false;
                        }
                    }
                }
            }
            
            // If the current team has pieces on the board that has any valid moves, return false.
            for (var i = 0; i < _board.Length; i++) {
                var topPiece = GetTopPiece(i);
                if ( topPiece == null || topPiece.GetTeam() != currentTurn ) continue;
                
                for (var j = 0; j < _board.Length; j++) {
                    if ( IsValidMove(topPiece, j) ) {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
    
        public void AddPiece(GamePiece piece, int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError($"Invalid column index. Index {positionIndex}");
                return;
            }
            _board[positionIndex].Push(piece);
            piece.SetOnBoard( true );
            piece.SetCurrentArea( positionIndex );
        }
    
        public GamePiece RemovePiece(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError($"Invalid column index. Index {positionIndex}");
                return null;
            }
        
            return _board[positionIndex].Count == 0 ? null : _board[positionIndex].Pop();
        }
        
        public void RemovePieceFromHand(GamePiece piece) {
            if ( piece.GetTeam() == PieceTeam.Red ) {
                _inHandRedPieces[(int)piece.GetLevel()] = null;
            }
            else {
                _inHandBluePieces[(int)piece.GetLevel()] = null;
            }
        }

        void ResetHands() {
            for (var i = 0; i < 6; i++) {
                _inHandRedPieces[i] = _teamRed[i];
                _inHandBluePieces[i] = _teamBlue[i];
            }
        }

        GamePiece GetTopPiece(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError($"Invalid column index. Index {positionIndex}");
                return null;
            }
            return _board[positionIndex].Count == 0 ? null : _board[positionIndex].Peek();
        }
        
        public bool IsPieceATopPiece(GamePiece piece) {
            for (var i = 0; i < _board.Length; i++) {
                if ( _board[i].Count == 0 ) continue;
                if ( _board[i].Peek() == piece ) {
                    return true;
                }
            }
            return false;
        }
    
        public bool IsValidMove(GamePiece pieceToMove, int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError($"Invalid column index. Index {positionIndex}");
                return false;
            }
        
            var topPiece = GetTopPiece(positionIndex);
            return _board[positionIndex].Count == 0 ||  topPiece.GetTeam() != pieceToMove.GetTeam() && topPiece.GetLevel() < pieceToMove.GetLevel();
        }
    
        public bool IsThereANextPieceInStack(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError($"Invalid column index. Index {positionIndex}");
                return false;
            }
            return _board[positionIndex].Count > 1;
        }
        
        public void ResetBoard() {
            for (var i = 0; i < _board.Length; i++) {
                while ( _board[i].Count > 0 ) {
                    var piece = _board[i].Pop();
                    piece.SetOnBoard( false );
                    piece.SetCurrentArea( -1 );
                    StartCoroutine(LerpToOriginalPosition(piece));
                }
            }
            ResetHands();
        }
    
        IEnumerator LerpToOriginalPosition(GamePiece piece) {
            var t = 0f;
            var startPos = piece.transform.position;
            var endPos = piece.GetTeam() == PieceTeam.Red ? teamRedParent.GetChild((int)piece.GetLevel()).position : 
                teamBlueParent.GetChild((int)piece.GetLevel()).position;
            while ( t < 1f ) {
                t += Time.deltaTime;
                piece.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
    }
}