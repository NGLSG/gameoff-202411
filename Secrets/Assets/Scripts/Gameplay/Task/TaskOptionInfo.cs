using System;
using System.Collections.Generic;

[Serializable]
public struct TaskOptionInfo
{
    public enum AnswerType
    {
        Ordinary = 0,
        Normal,
        Personal,
        Excellent,
        TrueEnding
    }

    public AnswerType sType;
    public int Score;
    public string Content;
}

[Serializable]
public class TaskOptionStorageInfo
{
    public List<TaskOptionInfo> TaskOptions = new List<TaskOptionInfo>();
}