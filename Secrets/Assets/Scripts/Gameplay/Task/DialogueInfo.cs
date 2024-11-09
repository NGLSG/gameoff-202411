using System;
using System.Collections.Generic;

[Serializable]
public struct DialogueInfo
{
    public int TaskID;
    public int NPCID;
    public string Dialogue;
}

[Serializable]
public class DialogueStorageInfo
{
    public List<DialogueInfo> DialogueInfos = new List<DialogueInfo>();
}