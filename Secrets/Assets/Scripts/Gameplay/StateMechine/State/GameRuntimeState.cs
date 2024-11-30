using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuntimeState : BaseState
{
    public GameRuntimeState(StateManager mgr) : base(mgr) { }

    public override void StateEnter()
    {
        base.StateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        
        GameTimeManager.FactualTime targetTime = new GameTimeManager.FactualTime();
        targetTime.SetTime(24, 0, 0); // 设置为 12:00:00

        
        // 如果到时间了就切换为游戏完成状态
        if (GameTimeManager.Instance.IsCurrentTimeGreaterThan(targetTime))
        {
            GameManager.Instance.stateMechine.SetState("GameFinishState");
        }
    }

    public override void StateExit()
    {
        base.StateExit();
    }
}