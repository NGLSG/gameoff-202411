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

        public int TaskID;
        public State TaskState;
        public int OptionID;
    }

    public Dictionary<int, List<TaskOptionInfo>> TaskOptions =
        new Dictionary<int, List<TaskOptionInfo>>();

    bool initialized = false;

    public Dictionary<int, DialogueStorageInfo> TaskDialogues = new Dictionary<int, DialogueStorageInfo>();

    public GameObject TaskPrefab;
    public List<TaskInfo> Tasks = new List<TaskInfo>();
    [SerializeField] private GameObject TaskParent;

    private void OnEnable()
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
            var taskObj = Instantiate(TaskPrefab, TaskParent.transform);
            taskObj.transform.parent = TaskParent.transform;
            taskObj.GetComponent<Task>().SetTaskInfo(task);
        }
    }

    private void SaveTempJson()
    {
#if UNITY_EDITOR
        var t = new DialogueStorageInfo();
        t.DialogueInfos.Add(new DialogueInfo() { Dialogue = "Hello" });

        Utils.SaveAsJson(t, Application.dataPath + "/Resources/Tasks/TempDialog.json");
        var t2 = new TaskOptionStorageInfo();
        t2.TaskOptions.Add(new TaskOptionInfo()
            { sType = TaskOptionInfo.OptionType.Normal, sScore = 10, sContent = "Hello", sUnlocked = true });
        Utils.SaveAsJson(t2,
            Application.dataPath + "/Resources/Tasks/TaskOptions/TempDialog.json");
#endif
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
                    TaskID = taskInfo.TaskID
                });
            foreach (var optInfos in taskOptionInfos)
            {
                if (optInfos.TaskOptions.Count == 0) continue;
                int idx = 0;
                foreach (var optInfo in optInfos.TaskOptions)
                {
                    if (!TaskOptions.ContainsKey(taskInfo.TaskID))
                    {
                        TaskOptions.Add(taskInfo.TaskID, new List<TaskOptionInfo>());
                    }

                    TaskOptions[taskInfo.TaskID].Add(optInfo);
                    idx++;
                }
            }
        }
    }

    public void UnlockOption(int taskID, int optionID)
    {
        if (TaskOptions.TryGetValue(taskID, out var v1))
        {
            if (v1.Any(info => info.OptID == optionID))
            {
                var opt = v1.FirstOrDefault(info => info.OptID == optionID);
                opt.sUnlocked = true;
            }
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