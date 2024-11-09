using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public struct TaskOptionInfo
{
    public enum OptionType
    {
        Ordinary = 0,
        Normal,
        Personal,
        Excellent,
        TrueEnding
    }

    public OptionType sType;
    public int sScore;
    public string sContent;
    public bool sUnlocked;
}

[Serializable]
public class TaskOptionStorageInfo
{
    public List<TaskOptionInfo> TaskOptions = new List<TaskOptionInfo>();
}