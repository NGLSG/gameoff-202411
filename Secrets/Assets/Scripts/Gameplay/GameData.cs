public class GameData
{
    private int exploreScore; // 探索值
    private int answerScore; // 普通任务分数
    private int personalityScore; // 任务回复后的积分
    private int hideOptionScore; // 隐藏选项分数
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
    
    // 调整探索分数值
    public void ChangeExploreScore(int changeAmount)
    {
        exploreScore += changeAmount;
    }
    
    // 调整普通分数值
    public void ChangerAnswerScore(int changeAmount)
    {
        answerScore += changeAmount;
    }
    
    // 调整个性分数值
    public void ChangePersonalityScore(int changeAmount)
    {
        personalityScore += changeAmount;
    }
    
    // 调整隐藏回答分数值
    public void ChangeHideOptionScore(int changeAmount)
    {
        hideOptionScore += changeAmount;
    }

    // 是否为真结局
    public void SetTrueEnding(bool isTrueEnding)
    {
        TrueEnding = isTrueEnding;
    }
    
    // 返回探索值
    public int GetExploreScore()
    {
        return exploreScore;
    }
    
    // 返回普通分数值
    public int GetAnswerScore()
    {
        return answerScore;
    }

    // 返回个性分数值
    public int GetPersonalityScore()
    {
        return personalityScore;
    }
    
    // 返回隐藏分数值
    public int GetHideOptionScore()
    {
        return hideOptionScore;
    }
    
    // 返回是否是真结局
    public bool GetTrueEnding()
    {
        return TrueEnding;
    }

    // 数据初始化
    public void InitGameData()
    {
        exploreScore = 0; // 探索值
        answerScore = 0; // 任务回复后的积分
        personalityScore = 0;
        TrueEnding = false; // 是否达到真结局
    }
}