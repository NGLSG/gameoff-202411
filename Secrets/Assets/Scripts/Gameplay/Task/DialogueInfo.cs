using System;
using System.Collections.Generic;

[Serializable]
public struct DialogueInfo
{
    public string Dialogue;
}

[Serializable]
public class DialogueStorageInfo
{
    public int TaskID;
    public string NPCID;
    public List<DialogueInfo> DialogueInfos = new List<DialogueInfo>();
}