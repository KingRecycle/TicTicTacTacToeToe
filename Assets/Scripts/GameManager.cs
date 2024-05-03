using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] BoardManager boardManager;
    [SerializeField] PieceTeam currentTurn;
    [SerializeField] int redWins;
    [SerializeField] int blueWins;
    [SerializeField] int draws;

    public bool IsGameOver { get; private set; }
    
    //UI
    [SerializeField] TMPro.TextMeshProUGUI turnText;
    [SerializeField] TMPro.TextMeshProUGUI winText;
    [SerializeField] TMPro.TextMeshProUGUI redWinsText;
    [SerializeField] TMPro.TextMeshProUGUI blueWinsText;
    
    void Start() {
        currentTurn = PieceTeam.Red;
        turnText.text = "Red's Turn";
        winText.text = "";
        
    }
    
    public void ChangeTurn() {
        currentTurn = currentTurn == PieceTeam.Red ? PieceTeam.Blue : PieceTeam.Red;
        turnText.text = currentTurn == PieceTeam.Red ? "Red's Turn" : "Blue's Turn";
    }
    
    public void ChangeTurn( PieceTeam teamToChangeTo ) {
        currentTurn = teamToChangeTo;
        turnText.text = currentTurn == PieceTeam.Red ? "Red's Turn" : "Blue's Turn";
    }
    
    public PieceTeam GetCurrentTurn() {
        return currentTurn;
    }

    public void ResetGame() {
        IsGameOver = false;
        winText.text = "";
        boardManager.ResetBoard();
    }
    
    public void EndGame( WinState winState ) {
        IsGameOver = true;
        switch (winState) {
            case WinState.Red:
                Debug.Log("Red wins!");
                winText.text = "Red wins!";
                redWins++;
                redWinsText.text = redWins.ToString();
                break;
            case WinState.Blue:
                Debug.Log("Blue wins!");
                winText.text = "Blue wins!";
                blueWins++;
                blueWinsText.text = blueWins.ToString();
                break;
            case WinState.Draw:
                Debug.Log("It's a draw!");
                winText.text = "It's a draw!";
                draws++;
                break;
        }
    }
    
}
