using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceLevel {
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
}

public enum PieceTeam {
    Red,
    Blue,
}
public class GamePiece : MonoBehaviour {
    [SerializeField] Sprite[] blueSprites;
    [SerializeField] Sprite[] redSprites;
    [SerializeField] float[] levelScale;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    [SerializeField] PieceLevel level;
    [SerializeField] PieceTeam team;
    [SerializeField] bool isOnBoard;
    [SerializeField] int currentAreaIndex;

    void OnValidate() {
        spriteRenderer.sprite = team == PieceTeam.Blue ? blueSprites[(int)level] : redSprites[(int)level];
        transform.localScale = new Vector3(levelScale[(int)level], levelScale[(int)level], 1);
    }
    
    public void SetPiece(PieceLevel pLevel, PieceTeam pTeam) {
        level = pLevel;
        team = pTeam;
        spriteRenderer.sprite = pTeam == PieceTeam.Blue ? blueSprites[(int)pLevel] : redSprites[(int)pLevel];
        transform.localScale = new Vector3(levelScale[(int)pLevel], levelScale[(int)pLevel], 1);
        spriteRenderer.sortingOrder = (int)pLevel;
        currentAreaIndex = -1;
    }
    
    public PieceLevel GetLevel() {
        return level;
    }
    
    public PieceTeam GetTeam() {
        return team;
    }
    
    public bool IsOnBoard() {
        return isOnBoard;
    }
    
    public void SetOnBoard(bool value) {
        isOnBoard = value;
    }
    
    public int GetCurrentArea() {
        return currentAreaIndex;
    }
    
    public void SetCurrentArea(int areaIndex) {
        currentAreaIndex = areaIndex;
    }
    
    
    
}
