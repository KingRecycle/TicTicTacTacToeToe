using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] GamePiece gamePiecePrefab;
    [SerializeField] Transform teamRedParent;
    [SerializeField] Transform teamBlueParent;
    [SerializeField] PieceTeam currentTurn;

    public bool IsGameOver { get; private set; }
    
    //UI
    [SerializeField] TMPro.TextMeshProUGUI turnText;
    [SerializeField] TMPro.TextMeshProUGUI winText;
    
    void Start() {
        currentTurn = PieceTeam.Red;
        turnText.text = "Red's Turn";
        winText.text = "";
        for (var i = 0; i < 6; i++) {
            GamePiece redPiece = Instantiate(gamePiecePrefab);
            redPiece.SetPiece((PieceLevel)i, PieceTeam.Red);
            redPiece.transform.SetPositionAndRotation( teamRedParent.GetChild(i).position, Quaternion.identity );
            redPiece.name = $"Red Piece {i + 1}";
            
            GamePiece bluePiece = Instantiate(gamePiecePrefab);
            bluePiece.SetPiece((PieceLevel)i, PieceTeam.Blue);
            bluePiece.transform.SetPositionAndRotation( teamBlueParent.GetChild(i).position, Quaternion.identity );
            bluePiece.name = $"Blue Piece {i + 1}";
        }
    }
    
    public void ChangeTurn() {
        currentTurn = currentTurn == PieceTeam.Red ? PieceTeam.Blue : PieceTeam.Red;
        turnText.text = currentTurn == PieceTeam.Red ? "Red's Turn" : "Blue's Turn";
    }
    
    public PieceTeam GetCurrentTurn() {
        return currentTurn;
    }

    public void ResetGame() {
        IsGameOver = false;
    }
    
    public void EndGame( WinState winState ) {
        IsGameOver = true;
        switch (winState) {
            case WinState.Red:
                Debug.Log("Red wins!");
                winText.text = "Red wins!";
                break;
            case WinState.Blue:
                Debug.Log("Blue wins!");
                winText.text = "Blue wins!";
                break;
            case WinState.Draw:
                Debug.Log("It's a draw!");
                winText.text = "It's a draw!";
                break;
        }
    }
    
}
