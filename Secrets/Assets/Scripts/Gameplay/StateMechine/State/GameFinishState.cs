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

        if (gameData.GetTrueEnding())
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.AlienEndding);
            return;
        }
        
        int normalScore = gameData.GetAnswerScore();;
        int personalityScore = gameData.GetPersonalityScore();
        int perfectScore = 0;
        
        int totalScore = normalScore + personalityScore;

        if (totalScore >= 0 && totalScore <= 20)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.LazyEndding);
        }
        else if (totalScore >= 30 && totalScore <= 60)
        {
            GameUIManager.Instance.SetEndding(GameData.EnddingState.NormalEndding);
        }
        else if (totalScore >= 70 && totalScore <= 170)
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