public class GameData
{
    private int exploreScore; // 探索值
    private int answerScore; // 隐藏任务分数
    private int personalityScore; // 任务回复后的积分
    private bool TrueEnding; // 是否达到真结局
    
    public enum EnddingState
    {
        None,
        LazyEndding,
        NormalEndding,
        PersonalityEndding,
        AlienEndding,
        PerfectEndding
    }

    /// <summary>
    /// 任务积分变化
    /// </summary>
    /// <param name="changeAmount"> 任务积分变化量 </param>
    public void ChangerAnswerScore(int changeAmount)
    {
        answerScore += changeAmount;
    }

    public void SetTrueEnding(bool isTrueEnding)
    {
        TrueEnding = isTrueEnding;
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
    /// 设置个性值
    /// </summary>
    /// <returns></returns>
    public void ChangePersonalityScore(int changeAmount)
    {
        personalityScore += changeAmount;
    }

    /// <summary>
    /// 返回探索值
    /// </summary>
    /// <returns></returns>
    public int GetExploreScore()
    {
        return exploreScore;
    }

    public bool GetTrueEnding()
    {
        return TrueEnding;
    }

    /// <summary>
    /// 返回游戏回答积分
    /// </summary>
    /// <returns></returns>
    public int GetAnswerScore()
    {
        return answerScore;
    }


    /// <summary>
    /// 返回个性值积分
    /// </summary>
    /// <returns></returns>
    public int GetPersonalityScore()
    {
        return personalityScore;
    }

    public void InitGameData()
    {
        exploreScore = 0; // 探索值
        answerScore = 0; // 任务回复后的积分
        personalityScore = 0;
        TrueEnding = false; // 是否达到真结局
    }
}