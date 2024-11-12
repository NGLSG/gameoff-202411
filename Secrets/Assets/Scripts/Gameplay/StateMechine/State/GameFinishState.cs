using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishState : BaseState
{
    public GameFinishState(StateManager mgr) : base(mgr) { }
    
    public override void StateEnter()
    {
        base.StateEnter();
        GameData gameData = GameManager.Instance.GetGameData();

        int normalScore = gameData.GetExploreScore();
        int personalityScore = gameData.GetPersonalityScore();
        int perfectScore = gameData.GetAnswerScore();
        
        int totalScore = normalScore + personalityScore;

        if (totalScore >= 0 && totalScore <= 20)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.LazyEndding);
        }
        else if (normalScore >= 30 && normalScore <= 60)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.NormalEndding);
        }
        else if (personalityScore >= 70 && personalityScore <= 170)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.PersonalityEndding);
        }
        else if (totalScore == 180 && perfectScore == 180)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.PerfectEndding);
        }

        GameUIManager.Instance.SetEndding(GameData.EnddingState.None);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit()
    {
        base.StateExit();
        GameManager.Instance.GetGameData().InitGameData();
    }
}