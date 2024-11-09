using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [Serializable]
    public struct TaskInfo
    {
        public int TaskID;
        public int NPCID;
        public bool IsFinished;
    }

    public Dictionary<int, Dictionary<int, TaskOptionInfo>> TaskOptions =
        new Dictionary<int, Dictionary<int, TaskOptionInfo>>();

    public GameObject TaskPrefab;
    public List<TaskInfo> Tasks = new List<TaskInfo>();
    [SerializeField] private GameObject TaskParent;

    private void OnEnable()
    {
        LoadingTasks();
        Utils.RemoveAllChildren(TaskParent.transform);
        foreach (var task in Tasks)
        {
            var taskObj = Instantiate(TaskPrefab, TaskParent.transform);
            taskObj.transform.parent = TaskParent.transform;
            taskObj.GetComponent<Task>().SetTaskInfo(task);
        }
    }

    private void LoadingTasks()
    {
        //TODO 从本地存储加载任务信息
    }
}