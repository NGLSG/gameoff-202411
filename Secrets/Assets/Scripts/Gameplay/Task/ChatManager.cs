using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private TaskManager.TaskInfo.State TaskState;

    public void Refresh()
    {
        Utils.RemoveAllChildren(TaskParent.transform);
        foreach (var info in DialogueInfos)
        {
            var chatContent = Instantiate(ChatContentPrefab, ChatParent.transform);
            chatContent.GetComponentInChildren<ChatContent>().NeedAnim = TaskState == TaskManager.TaskInfo.State.UnRead;
            if (TaskState == TaskManager.TaskInfo.State.UnRead)
            {
                TaskState = TaskManager.TaskInfo.State.InProgress;
                TaskManager.Instance.SetTaskState(TaskID, TaskState);
            }

            chatContent.transform.parent = ChatParent.transform;
            chatContent.GetComponentInChildren<ChatContent>().SetDialogueInfo(info);
        }

        foreach (var info in TaskOptionInfos)
        {
            if (info.sUnlocked)
            {
                var taskOption = Instantiate(TaskOptionPrefab, TaskParent.transform);
                taskOption.transform.parent = TaskParent.transform;
                taskOption.GetComponentInChildren<TaskOption>().SetTaskOptionInfo(info);
            }
        }
    }

    public void SetTaskID(int taskID)
    {
        TaskID = taskID;
        var tasks = TaskManager.Instance.TaskDialogues[TaskID];
        Utils.RemoveAllChildren(ChatParent.transform);
        DialogueInfos = tasks.DialogueInfos;
        TaskOptionInfos = TaskManager.Instance.TaskOptions[TaskID].Values.ToList();

        TaskState = TaskManager.Instance.GetTaskState(TaskID);
        Refresh();
    }


    public void SelectTaskOption(TaskOptionInfo taskOptionInfo)
    {
        if (TaskOptions.Find(x => x.TaskOptionInfo.sContent == taskOptionInfo.sContent))
        {
            TaskParent.SetActive(false);
            TaskManager.Instance.SetTaskState(TaskID, TaskManager.TaskInfo.State.Finished);
            switch (taskOptionInfo.sType)
            {
                case TaskOptionInfo.OptionType.Personal:
                    GameManager.Instance.GetGameData().ChangePersonalityScore(taskOptionInfo.sScore);
                    break;
                case TaskOptionInfo.OptionType.TrueEnding:
                    GameManager.Instance.GetGameData().SetTrueEnding(true);
                    break;
                case TaskOptionInfo.OptionType.Normal:
                case TaskOptionInfo.OptionType.Excellent:
                case TaskOptionInfo.OptionType.Ordinary:
                    GameManager.Instance.GetGameData().ChangerAnswerScore(taskOptionInfo.sScore);
                    break;
            }
        }
    }
}