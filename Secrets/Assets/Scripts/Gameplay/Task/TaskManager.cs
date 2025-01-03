﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class TaskManager : Singleton<TaskManager>
{
    [Serializable]
    public class TaskInfo
    {
        public enum State
        {
            UnRead,
            InProgress,
            Finished
        }

        public bool unlocked;

        public int TaskID;
        public State TaskState;
        public int OptionID = -1;
        public string NPCID;
    }

    public Dictionary<int, List<TaskOptionInfo>> TaskOptions =
        new Dictionary<int, List<TaskOptionInfo>>();

    bool initialized = false;

    public Dictionary<int, DialogueStorageInfo> TaskDialogues = new Dictionary<int, DialogueStorageInfo>();

    public GameObject TaskPrefab;
    public List<TaskInfo> Tasks = new List<TaskInfo>();
    [SerializeField] private GameObject TaskParent;

    public void OnShhow()
    {
        if (!initialized)
        {
            initialized = true;

            //SaveTempJson();
            LoadingTasks();
        }

        Utils.RemoveAllChildren(TaskParent.transform);
        foreach (var task in Tasks)
        {
            if (task.unlocked || GameManager.Instance.GetGameData().GetExploreScore() == HUD.Instance.exploreValueMax)
            {
                var taskObj = Instantiate(TaskPrefab, TaskParent.transform);
                taskObj.transform.parent = TaskParent.transform;
                taskObj.GetComponentInChildren<Task>().SetTaskInfo(task);
            }
        }
    }
    private void OnEnable()
    {

    }

    private void SaveTempJson()
    {
    }

    private void LoadingTasks()
    {
        bool isExist = false;
        TextAsset[] tasks = Resources.LoadAll<TextAsset>("Tasks");
        TextAsset[] taskOpts = Resources.LoadAll<TextAsset>("Tasks/TaskOptions");
        Tasks.Clear();
        if (File.Exists(Application.persistentDataPath + "/Tasks.json"))
        {
            Tasks = Utils.LoadFromJson<List<TaskInfo>>(Application.persistentDataPath + "/Tasks.json");
            isExist = true;
        }

        var taskInfos = Utils.ConvertTextAssetArray<DialogueStorageInfo>(tasks);
        var taskOptionInfos = Utils.ConvertTextAssetArray<TaskOptionStorageInfo>(taskOpts);
        TaskDialogues.Clear();
        TaskOptions.Clear();
        foreach (var taskInfo in taskInfos)
        {
            if (taskInfo.DialogueInfos.Count == 0) continue;
            TaskDialogues.Add(taskInfo.TaskID, taskInfo);
            if (!isExist)
                Tasks.Add(new TaskInfo()
                {
                    TaskID = taskInfo.TaskID,
                    NPCID = taskInfo.NPCID,
                    unlocked = taskInfo.Unlocked,
                });
        }

        foreach (var opts in taskOptionInfos)
        {
            TaskOptions[opts.TaskID] = opts.TaskOptions;
        }
    }

    public void UnlockOption(int optionID)
    {
        Debug.Log($"<color=green> Try Unlock task Option {optionID}</color>");
        int idx = 0;

        foreach (var v1 in TaskOptions.Values)
        {
            if (v1.Any(info => info.OptID == optionID))
            {
                var opt = v1.FirstOrDefault(info => info.OptID == optionID);
                opt.sUnlocked = true;
                TaskOptions[idx][optionID % 100 - 1] = opt;
                break;
            }

            idx++;
        }
    }


    public TaskInfo.State GetTaskState(int taskID)
    {
        return Tasks.FirstOrDefault(x => x.TaskID == taskID)!.TaskState;
    }

    public void SetTaskState(int taskID, TaskInfo.State state)
    {
        Tasks.FirstOrDefault(x => x.TaskID == taskID)!.TaskState = state;
    }
}