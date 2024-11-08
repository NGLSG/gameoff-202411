public struct TaskOptionInfo
{
    public enum AnswerType
    {
        Ordinary = 0,
        Normal,
        Personal,
        Excellent,
        Alien
    }

    public AnswerType sType;
    public string sText;
    public int Score;
}