using System.Collections;
using System.Collections.Generic;
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
    
        void Start() {
            for (var i = 0; i < _board.Length; i++) {
                _board[i] = new Stack<GamePiece>(6);
            }
        
            for (var i = 0; i < 6; i++) {
                var redPiece = Instantiate(gamePiecePrefab);
                redPiece.SetPiece((PieceLevel)i, PieceTeam.Red);
                redPiece.transform.SetPositionAndRotation( teamRedParent.GetChild(i).position, Quaternion.identity );
                redPiece.name = $"Red Piece {i + 1}";
                _teamBlue[i] = redPiece;
            
                var bluePiece = Instantiate(gamePiecePrefab);
                bluePiece.SetPiece((PieceLevel)i, PieceTeam.Blue);
                bluePiece.transform.SetPositionAndRotation( teamBlueParent.GetChild(i).position, Quaternion.identity );
                bluePiece.name = $"Blue Piece {i + 1}";
                _teamBlue[i] = bluePiece;
            }
        }
    
        public bool CheckForWin() {
            //Check for win conditions, the win conditions are:
            //1. If a player has 3 top pieces in a row.
            var (hasWonH, teamH) = CheckForHorizontalWin();
            if (hasWonH) {
                gameManager.EndGame( teamH == PieceTeam.Red ? WinState.Red : WinState.Blue );
                return true;
            }
        
            //2. If a player has 3 top pieces in a column.
            var (hasWonV, teamV) = CheckForVerticalWin();
            if (hasWonV) {
                gameManager.EndGame( teamV == PieceTeam.Red ? WinState.Red : WinState.Blue );
                return true;
            }
        
            //3. If a player has 3 top pieces in a diagonal.
            var (hasWonD, teamD) = CheckForDiagonalWin();
            if (hasWonD) {
                gameManager.EndGame( teamD == PieceTeam.Red ? WinState.Red : WinState.Blue );
                return true;
            }
        
            //A draw happens when the current player can't make anymore valid moves.
            if ( CheckForDraw() ) {
                gameManager.EndGame( WinState.Draw );
                return true;
            }
        
            //If none of the win conditions are met, return false.
            return false;
        }
    
        public void AddPiece(GamePiece piece, int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError("Invalid column index.");
                return;
            }
            _board[positionIndex].Push(piece);
            piece.SetOnBoard( true );
            piece.SetCurrentArea( positionIndex );
        }
    
        public GamePiece RemovePiece(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError("Invalid column index.");
                return null;
            }
        
            return _board[positionIndex].Count == 0 ? null : _board[positionIndex].Pop();
        }
    
        public GamePiece GetTopPiece(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError("Invalid column index.");
                return null;
            }
            return _board[positionIndex].Count == 0 ? null : _board[positionIndex].Peek();
        }
    
        public bool IsValidMove(GamePiece pieceToMove, int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError("Invalid column index.");
                return false;
            }
        
            var topPiece = GetTopPiece(positionIndex);
            return _board[positionIndex].Count == 0 ||  topPiece.GetTeam() != pieceToMove.GetTeam() && topPiece.GetLevel() < pieceToMove.GetLevel();
        }
    
        public bool IsThereANextPieceInStack(int positionIndex) {
            if ( positionIndex < 0 || positionIndex >= _board.Length) {
                Debug.LogError("Invalid column index.");
                return false;
            }
            return _board[positionIndex].Count > 1;
        }
    
        (bool, PieceTeam) CheckForHorizontalWin() {
            if ( GetTopPiece(0) && GetTopPiece(1) && GetTopPiece(2)
                 && GetTopPiece(0).GetTeam() == GetTopPiece(1).GetTeam() && GetTopPiece(1).GetTeam() == GetTopPiece(2).GetTeam() ) {
                return (true, GetTopPiece(0).GetTeam());
            }
            if ( GetTopPiece(3) && GetTopPiece(4) && GetTopPiece(5)
                 && GetTopPiece(3).GetTeam() == GetTopPiece(4).GetTeam() && GetTopPiece(4).GetTeam() == GetTopPiece(5).GetTeam() ) {
                return (true, GetTopPiece(3).GetTeam());
            }
            if ( GetTopPiece(6) && GetTopPiece(7) && GetTopPiece(8)
                 && GetTopPiece(6).GetTeam() == GetTopPiece(7).GetTeam() && GetTopPiece(7).GetTeam() == GetTopPiece(8).GetTeam() ) {
                return (true, GetTopPiece(6).GetTeam());
            }
            return (false, PieceTeam.Red);
        }

        (bool, PieceTeam) CheckForVerticalWin() {
            if ( GetTopPiece(0) && GetTopPiece(3) && GetTopPiece(6)
                 && GetTopPiece(0).GetTeam() == GetTopPiece(3).GetTeam() && GetTopPiece(6).GetTeam() == GetTopPiece(2).GetTeam() ) {
                return (true, GetTopPiece(0).GetTeam());
            }
            if ( GetTopPiece(1) && GetTopPiece(4) && GetTopPiece(7)
                 && GetTopPiece(1).GetTeam() == GetTopPiece(4).GetTeam() && GetTopPiece(4).GetTeam() == GetTopPiece(7).GetTeam() ) {
                return (true, GetTopPiece(1).GetTeam());
            }
            if ( GetTopPiece(2) && GetTopPiece(5) && GetTopPiece(8)
                 && GetTopPiece(2).GetTeam() == GetTopPiece(5).GetTeam() && GetTopPiece(5).GetTeam() == GetTopPiece(8).GetTeam() ) {
                return (true, GetTopPiece(2).GetTeam());
            }
            return (false, PieceTeam.Red);
        }
    
        (bool, PieceTeam) CheckForDiagonalWin() {
            if ( GetTopPiece(0) && GetTopPiece(4) && GetTopPiece(8)
                 && GetTopPiece(0).GetTeam() == GetTopPiece(4).GetTeam() && GetTopPiece(4).GetTeam() == GetTopPiece(8).GetTeam() ) {
                return (true, GetTopPiece(0).GetTeam());
            }
            if ( GetTopPiece(2) && GetTopPiece(4) && GetTopPiece(6)
                 && GetTopPiece(2).GetTeam() == GetTopPiece(4).GetTeam() && GetTopPiece(4).GetTeam() == GetTopPiece(6).GetTeam() ) {
                return (true, GetTopPiece(2).GetTeam());
            }
            return (false, PieceTeam.Red);
        }

        bool CheckForDraw() {
            return false;
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