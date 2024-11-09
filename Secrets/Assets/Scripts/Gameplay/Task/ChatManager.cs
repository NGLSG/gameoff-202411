using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : Singleton<ChatManager>
{
    public List<DialogueInfo> DialogueInfos;
    public List<TaskOptionInfo> TaskOptionInfos;
    [SerializeField] private GameObject ChatParent;
    [SerializeField] private GameObject TaskParent;
    [SerializeField] private GameObject ChatContentPrefab;
    [SerializeField] private GameObject TaskOptionPrefab;
    [SerializeField] private List<TaskOption> TaskOptions;
    [SerializeField] private int TaskID;

    public void Refresh()
    {
        Utils.RemoveAllChildren(ChatParent.transform);
        Utils.RemoveAllChildren(TaskParent.transform);

        foreach (var info in DialogueInfos)
        {
            var chatContent = Instantiate(ChatContentPrefab, ChatParent.transform);
            chatContent.transform.parent = ChatParent.transform;
            chatContent.GetComponent<ChatContent>().SetDialogueInfo(info);
        }

        foreach (var info in TaskOptionInfos)
        {
            var taskOption = Instantiate(TaskOptionPrefab, TaskParent.transform);
            taskOption.transform.parent = TaskParent.transform;
            taskOption.GetComponent<TaskOption>().SetTaskOptionInfo(info);
        }
    }

    public void SetTaskID(int taskID)
    {
        TaskID = taskID;
        //TODO 根据TaskID在Resources/Tasks里找到对应Dialogues和Options

        Refresh();
    }
}