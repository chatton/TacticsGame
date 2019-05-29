using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{


    public Material activeMaterial;
    public Material selectedMaterial;
    public Material moveNotMyTurn;
    public Material moveMyTurn;


    private Material moveMat
    {
        get
        {
            if (IsMyTurn)
            {
                return moveMyTurn;
            }
            return moveNotMyTurn;
        }
    }
    private Color activeColour;
    private Color moveColour
    {
        get
        {
            if (IsMyTurn)
            {
                return Color.green;
            }
            return Color.red;
        }
    }
    public Color selectedColour;
    public bool IsMyTurn
    {
        get
        {
            return turnTracker.currentTeam.Equals(team);
        }
    }

    private bool needsToShowMovementRadius = true;

    public Tile currentTile
    {
        get
        {
            // does a ray cast vertically downwards to detect which tile it is over
            float dist = 10f;
            Vector3 dir = new Vector3(0, -1, 0);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, dist))
            {
                return hit.transform.gameObject.GetComponent<Tile>();
            }
            throw new ArgumentException("Should always be above a tile!");
        }
    }

    public List<Tile> currentPath;

    public int moveDistance = 5;

    private GameBoard board;
    public TurnTracker turnTracker;
    public Team team;

    public bool HasTakenTurn;

    public bool IsSelected { get { return turnTracker.currentTeam.selectedUnit == this; } }
    private bool mouseOver;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        board = FindObjectOfType<GameBoard>();
        turnTracker = FindObjectOfType<TurnTracker>();
        meshRenderer = GetComponent<MeshRenderer>();
        activeColour = meshRenderer.material.color;
        currentPath = new List<Tile>();
    }


    void Update()
    {
        if (!IsSelected)
        {
            return;
        }
        UpdateSelected();
        DrawPath();
    }

    private void UpdateSelected()
    {
        if (IsSelected)
        {
            //meshRenderer.material.color = selectedColour;
            meshRenderer.material = selectedMaterial;
            if (needsToShowMovementRadius)
            {
                ShowMovementRadius();
                needsToShowMovementRadius = false;
            }

        }
        else if (!HasTakenTurn)
        {
            //meshRenderer.material.color = activeColour;
            meshRenderer.material = activeMaterial;

        }
        else
        {
            meshRenderer.material.color = Color.grey;
        }
    }

    private void DrawPath()
    {
        if (!IsMyTurn)
        {
            return;
        }
        board.DrawPath(currentPath, Color.red);
    }

    private void OnMouseDown()
    {
        if (IsMyTurn)
        {
            print("Selected: " + gameObject.name);
            Select();
            //EndIndividualAction();
        }
    }


    void Select()
    {
        turnTracker.currentTeam.selectedUnit = this;
        needsToShowMovementRadius = true;
    }

    void OnMouseOver()
    {
        if (!mouseOver && !HasTakenTurn)
        {
            ShowMovementRadius();
        }
    }

    public void OnBeginTurn()
    {
        HasTakenTurn = false;
        //meshRenderer.material.color = activeColour;
        meshRenderer.material = activeMaterial;
    }

    public void EndIndividualAction()
    {
        // go gray when turn is over.
        HasTakenTurn = true;
        meshRenderer.material.color = Color.grey;
        board.Clear();
    }

    public void OnEndTurn()
    {
        HasTakenTurn = false;
        //meshRenderer.material.color = activeColour;
        meshRenderer.material = activeMaterial;
    }

    void ShowMovementRadius()
    {
        board.HighlightMovementRangeFromTile(currentTile, moveDistance, moveMat);
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (mouseOver)
        {
            board.Clear();
            mouseOver = false;
        }
    }

}
