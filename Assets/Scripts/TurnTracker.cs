using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTracker : MonoBehaviour
{
    public int turnNo = 1;

    private int teamIndex = 0;
    private List<Team> teams;
    public Team currentTeam { get { return teams[teamIndex]; } }

    void Start()
    {
        teams = new List<Team>(FindObjectsOfType<Team>());
        for (int i = 0; i < teams.Count; i++)
        {
            teams[i].teamId = i;
        }
        currentTeam.OnTurnBegin();
    }

    void nextTurn()
    {
        turnNo++;
        teamIndex = (teamIndex + 1) % teams.Count;
        print("nextTurn");
    }

    // Update is called once per frame
    void Update()
    {
        // don't want to do anything if the player is still taking their turn.
        if (currentTeam.InProgress())
        {
            return;
        }

        // it is now the end of the turn
        currentTeam.OnTurnEnd();
        nextTurn(); // progress to the next turn
        currentTeam.OnTurnBegin();
    }
  
}
