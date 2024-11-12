using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private BaseState currentState;
    
    private List<BaseState> states;

    public StateManager()
    {
        states = new List<BaseState>();
        states.Add(new GameInitState(this));
        states.Add(new GameFinishState(this));
        
        currentState = states[0];
    }

    public void SetState(string newState)
    {
        currentState.StateExit();

        foreach (var state in states)
        {
            if (state.ToString() == newState)
            {
                currentState = state;
                break;
            }
        }
        
        currentState.StateEnter();
    }

    public void StateUpdate()
    {
        currentState.StateUpdate();
    }

    public void PrintCurState()
    {
        Debug.Log("---------Current State: " + currentState.ToString());
    }

    public string GetCurState()
    {
        return currentState.ToString();
    }
}