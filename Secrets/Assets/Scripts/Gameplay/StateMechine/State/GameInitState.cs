using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInitState : BaseState
{
    public GameInitState(StateManager mgr) : base(mgr) { }
    
    public override void StateEnter()
    {
        base.StateEnter();
        GameManager.Instance.GetGameData().InitGameData();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit()
    {
        base.StateExit();
    }
}
