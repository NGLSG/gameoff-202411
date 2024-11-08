using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int exploreScore; // 探索值
    public int answerScore; // 任务回复后的积分

    /// <summary>
    /// 任务积分变化
    /// </summary>
    /// <param name="changeAmount"> 任务积分变化量 </param>
    public void ChangerAnswerScore(int changeAmount)
    {
        answerScore += changeAmount;
    }

    /// <summary>
    /// 探索值变化
    /// </summary>
    /// <param name="changeAmount">探索值积分变化量</param>
    public void ChangeExploreScore(int changeAmount)
    {
        exploreScore += changeAmount;
    }

    /// <summary>
    /// 返回探索值
    /// </summary>
    /// <returns></returns>
    public int GetExploreScore()
    {
        return exploreScore;
    }

    /// <summary>
    /// 返回游戏回答积分
    /// </summary>
    /// <returns></returns>
    public int GetAnswerScore()
    {
        return answerScore;
    }
}
