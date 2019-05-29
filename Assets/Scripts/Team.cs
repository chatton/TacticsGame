using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team : MonoBehaviour
{

    public Tile fromTile;
    public Tile toTile;
    public Unit selectedUnit;
    public int teamId;

    public List<Unit> units;

    private void Awake()
    {
        units = new List<Unit>();
        foreach (Transform child in transform)
        {
            RegisterUnit(child.GetComponent<Unit>());
        }
    }

    // if there are any players who have not taken their turn
    public bool AnyActiveUnits()
    {
        return units.Any(unit => !unit.HasTakenTurn);
    }

    public void RegisterUnit(Unit unit)
    {
        if (units.Contains(unit))
        {
            throw new ArgumentException("Cannot join team you're alread in!");
        }
        units.Add(unit);
        unit.team = this;
    }


    //  handle anything that needs to be done before the turn begins
    public void OnTurnBegin()
    {
        print("turn beginning for team: " + teamId);
        units.ForEach(unit => unit.OnBeginTurn());
    }

    // method which indicates if the turn is still taking place
    public bool InProgress()
    {
        return AnyActiveUnits();
    }

    public void OnTurnEnd()
    {
        print("turn ending for team: " + teamId);
        units.ForEach(unit => unit.OnEndTurn());
        selectedUnit = null;
        fromTile = null;
        toTile = null;
    }



    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }
        var otherTile = other as Team;

        return otherTile.teamId == teamId;
    }

    public override int GetHashCode()
    {
        return teamId;
    }
}
