using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    [SerializeField] BoardManager boardManager;
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform[] placementAreas;
    
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip placeSound;
    
    GamePiece _selectedPiece;
    Vector3 _selectedPieceLastPosition;
    const float MIN_DISTANCE = 2f;
    
    void Update() {
        var mousePos = Input.mousePosition;
        mousePos.z = 0;
        if ( Input.GetMouseButtonDown(0) ) {
            DoOnButtonDown(mousePos);
        }
        
        if ( Input.GetMouseButton(0) && _selectedPiece ) {
            DoPieceDrag(mousePos);
        }
        
        if ( Input.GetMouseButtonUp(0) && _selectedPiece ) {
            DoOnButtonRelease();
            SoundManager.Instance.PlaySoundFx(placeSound, 1f);
        }
    }

    void DoOnButtonDown( Vector3 mousePos ) {
        if (gameManager.IsGameOver) { return; }
        var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePos), Vector2.zero, Mathf.Infinity, layerMask);
        if ( !hit.collider ) { return; }

        _selectedPiece = hit.collider.GetComponent<GamePiece>();
        if ( _selectedPiece && _selectedPiece.GetTeam() != gameManager.GetCurrentTurn() ) {
            _selectedPiece = null;
            return;
        }
        //If there isn't a next piece in the stack, the player can't move the piece.
        if ( _selectedPiece.IsOnBoard() ) {
            var closestArea = GetClosestPlacementArea();
            if ( !boardManager.IsThereANextPieceInStack(System.Array.IndexOf(placementAreas, closestArea)) ) {
                _selectedPiece = null;
                return;
            }
        }

        _selectedPieceLastPosition = _selectedPiece.transform.position;
        SoundManager.Instance.PlaySoundFx(pickUpSound, 1f);
        Debug.Log("Piece selected: " + _selectedPiece.name);
    }

    void DoPieceDrag( Vector3 mousePos ) {
        var newPos = mainCamera.ScreenToWorldPoint(mousePos);
        newPos.z = _selectedPiece.transform.position.z;
        _selectedPiece.transform.position = newPos;
    }

    void DoOnButtonRelease() {
        var closestArea = GetClosestPlacementArea();
        if ( closestArea && Vector3.Distance(_selectedPiece.transform.position, closestArea.position) < MIN_DISTANCE ){
            if ( boardManager.IsValidMove(_selectedPiece, System.Array.IndexOf(placementAreas, closestArea)) ) {
                if ( _selectedPiece.IsOnBoard() ) {
                    boardManager.RemovePiece(System.Array.IndexOf(placementAreas, _selectedPiece.GetCurrentArea()));
                }
                
                boardManager.AddPiece(_selectedPiece, System.Array.IndexOf(placementAreas, closestArea));
                _selectedPiece.transform.position = closestArea.position;
                if ( !boardManager.CheckForWin() ) {
                    gameManager.ChangeTurn();
                }
            }
            else {
                _selectedPiece.transform.position = _selectedPieceLastPosition;
                Debug.Log("Invalid move.");
            }
        }
        else {
            _selectedPiece.transform.position = _selectedPieceLastPosition;
            Debug.Log($"Piece returned to last position. {_selectedPieceLastPosition}");
        }
        _selectedPiece = null;
    }
    
    
    Transform GetClosestPlacementArea() {
        Transform closestArea = null;
        var closestDistance = Mathf.Infinity;
        foreach (var area in placementAreas) {
            var distance = Vector3.Distance(_selectedPiece.transform.position, area.position);
            if ( distance < closestDistance ) {
                closestDistance = distance;
                closestArea = area;
            }
        }
        return closestArea;
    }
    
    
}
