using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChatManager : Singleton<ChatManager>
{
    [Serializable]
    public struct NPCInfo
    {
        public string NPCID;
        public Sprite NPCSprite;
    }

    public List<DialogueInfo> DialogueInfos;
    public List<NPCInfo> NPCInfos;
    public List<TaskOptionInfo> TaskOptionInfos;
    [SerializeField] private GameObject ChatParent;
    [SerializeField] private GameObject TaskParent;
    [SerializeField] private GameObject ChatContentPrefab;
    [SerializeField] private GameObject PlayerContentPrefab;
    [SerializeField] private GameObject TaskOptionPrefab;
    [SerializeField] private List<TaskOption> TaskOptions;
    [SerializeField] private int TaskID;
    [SerializeField] private TaskManager.TaskInfo.State TaskState;
    [SerializeField] private Transform _1;
    [SerializeField] private Transform _2;
    [SerializeField] private TextMeshProUGUI NPC;
    [SerializeField] private TextMeshProUGUI Input;
    [SerializeField] private TaskOptionInfo currentTaskOptionInfo;
    public bool Finished;
    private int count = 0;

    public IEnumerator Refresh()
    {
        if (TaskState == TaskManager.TaskInfo.State.Finished)
            ChatParent.transform.SetPositionAndRotation(_2.position, _2.rotation);
        else
            ChatParent.transform.SetPositionAndRotation(_1.position, _1.rotation);

        Utils.RemoveAllChildren(ChatParent.transform);
        bool delayPlay = TaskState == TaskManager.TaskInfo.State.UnRead;
        foreach (var info in DialogueInfos)
        {
            var chatContent = Instantiate(ChatContentPrefab, ChatParent.transform);
            chatContent.GetComponentInChildren<ChatContent>().NeedAnim = TaskState == TaskManager.TaskInfo.State.UnRead;
            chatContent.GetComponentInChildren<ChatContent>().Avatar.sprite = GetNPCSprite(NPC.text);
            chatContent.transform.parent = ChatParent.transform;
            chatContent.GetComponentInChildren<ChatContent>().SetDialogueInfo(info);
            if (delayPlay)
                yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.5f));
        }

        if (TaskState == TaskManager.TaskInfo.State.UnRead)
        {
            TaskState = TaskManager.TaskInfo.State.InProgress;
            TaskManager.Instance.SetTaskState(TaskID, TaskState);
        }


        if (TaskState == TaskManager.TaskInfo.State.Finished)
        {
            var go = Instantiate(PlayerContentPrefab, ChatParent.transform);
            go.GetComponentInChildren<ChatContent>().content.text = TaskOptionInfos.FirstOrDefault(info =>
                info.OptID == TaskManager.Instance.Tasks.First(x => x.TaskID == TaskID).OptionID).sContent;
            yield break;
        }

        TaskParent.transform.parent.gameObject.SetActive(true);
        Utils.RemoveAllChildren(TaskParent.transform);
        foreach (var info in TaskOptionInfos)
        {
            if (info.sUnlocked || GameManager.Instance.GetGameData().GetExploreScore() == 60)
            {
                var taskOption = Instantiate(TaskOptionPrefab, TaskParent.transform);
                taskOption.transform.parent = TaskParent.transform;
                taskOption.GetComponentInChildren<TaskOption>().SetTaskOptionInfo(info);
            }
        }

        yield break;
    }

    public Sprite GetNPCSprite(string NPCID)
    {
        return NPCInfos.First(x => x.NPCID == NPCID).NPCSprite;
    }

    public void SetTaskID(int taskID)
    {
        TaskID = taskID;
        var tasks = TaskManager.Instance.TaskDialogues[TaskID];
        Utils.RemoveAllChildren(ChatParent.transform);
        DialogueInfos = tasks.DialogueInfos;
        TaskOptionInfos = TaskManager.Instance.TaskOptions[TaskID];
        NPC.text = TaskManager.Instance.Tasks.First(x => x.TaskID == TaskID).NPCID;

        TaskState = TaskManager.Instance.GetTaskState(TaskID);
        StartCoroutine(Refresh());
    }


    public void SelectTaskOption(TaskOptionInfo taskOptionInfo)
    {
        currentTaskOptionInfo = taskOptionInfo;
        Input.text = taskOptionInfo.sContent;
    }

    public void Send()
    {
        if (TaskOptionInfos.Any(x => x.sContent == currentTaskOptionInfo.sContent))
        {
            ChatParent.transform.SetPositionAndRotation(_2.position, _2.rotation);
            TaskParent.transform.parent.gameObject.SetActive(false);
            TaskManager.Instance.SetTaskState(TaskID, TaskManager.TaskInfo.State.Finished);
            switch (currentTaskOptionInfo.sType)
            {
                case TaskOptionInfo.OptionType.Personal:
                    GameManager.Instance.GetGameData().ChangePersonalityScore(currentTaskOptionInfo.sScore);
                    break;
                case TaskOptionInfo.OptionType.Hidden:
                    GameManager.Instance.GetGameData().ChangeHideOptionScore(currentTaskOptionInfo.sScore);
                    break;
                case TaskOptionInfo.OptionType.Normal:
                case TaskOptionInfo.OptionType.Worse:
                    GameManager.Instance.GetGameData().ChangerAnswerScore(currentTaskOptionInfo.sScore);
                    break;
                case TaskOptionInfo.OptionType.TrueEnding:
                    GameManager.Instance.GetGameData().SetTrueEnding(true);
                    break;
            }

            count++;

            TaskManager.Instance.SetTaskState(TaskID, TaskManager.TaskInfo.State.Finished);
            TaskManager.Instance.Tasks.First(x => x.TaskID == TaskID).OptionID = currentTaskOptionInfo.OptID;
            var go = Instantiate(PlayerContentPrefab, ChatParent.transform);
            go.GetComponentInChildren<ChatContent>().content.text = currentTaskOptionInfo.sContent;
            Input.text = "";
        }

        if (count == TaskOptions.Count)
        {
            Finished = true;
            GameManager.Instance.stateMechine.SetState(nameof(GameFinishState));
        }
    }
}