using System;
using System.Collections.Generic;

[Serializable]
public struct DialogueInfo
{
    public LocalizedContent Content;

    public string Dialogue
    {
        get
        {
            if (GameSetting.Setting.Language == "en")
            {
                return Content.En;
            }
            else
            {
                return Content.Cn;
            }
        }
    }
}

[Serializable]
public class DialogueStorageInfo
{
    public int TaskID;
    public string NPCID;
    public bool Unlocked;
    public List<DialogueInfo> DialogueInfos = new List<DialogueInfo>();
}