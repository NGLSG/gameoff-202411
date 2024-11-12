using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string StateName;
    
    protected StateManager manager;
    
    public BaseState(StateManager mgr)
    {
        manager = mgr;
    }

    public virtual void StateEnter(){}
    public virtual void StateExit(){}
    public virtual void StateUpdate(){}

}