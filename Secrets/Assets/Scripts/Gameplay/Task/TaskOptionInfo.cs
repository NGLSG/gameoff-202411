﻿using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public struct LocalizedContent
{
    public string En;
    public string Cn;
}

[Serializable]
public struct TaskOptionInfo
{
    public enum OptionType
    {
        Worse = 0,
        Normal,
        Personal,
        Hidden,
        TrueEnding
    }

    public int OptID;
    public OptionType sType;
    public int sScore;
    public LocalizedContent loclizedContent;

    public string sContent
    {
        get
        {
            if (GameSetting.Setting.Language == "en")
            {
                return loclizedContent.En;
            }
            else
            {
                return loclizedContent.Cn;
            }
        }
    }

    public bool sUnlocked;
}

[Serializable]
public class TaskOptionStorageInfo
{
    public int TaskID;
    public List<TaskOptionInfo> TaskOptions = new List<TaskOptionInfo>();
}