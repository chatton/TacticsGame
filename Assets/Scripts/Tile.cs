using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int x;
    public int y;
    public int depth;
    public Tile parent;
    public TurnTracker tt;
    private GameBoard board;
    public Unit unit
    {
        get
        {
            // does a ray cast vertically updates to detect which unit it on this tile
            float dist = 10f;
            Vector3 dir = new Vector3(0, 1, 0);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, dist))
            {
                return hit.transform.gameObject.GetComponent<Unit>();
            }
            return null;
        }
    }


    private void Start()
    {
        board = GetComponentInParent<GameBoard>();
        tt = FindObjectOfType<TurnTracker>();
    }
    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }
        var otherTile = other as Tile;

        return otherTile.x == x && otherTile.y == y;
    }

    public override int GetHashCode()
    {
        return x * 7 | y * 21;
    }

    private void OnMouseDown()
    {

        Team current = tt.currentTeam;
        if (current.selectedUnit != null) // this means a unit is selected, let's try and move here!
        {

            float dist = Vector3.Distance(current.selectedUnit.currentTile.transform.position, transform.position);
            if (dist > current.selectedUnit.moveDistance)
            {
                return; // definitely too far, don't bother calculating path
            }
            print("Calculating path");
            current.selectedUnit.currentPath = board.GetPathBetween(
                current.selectedUnit.currentTile, this, current.selectedUnit.moveDistance);
        }
    }
}
