using UnityEngine;

public class Task : MonoBehaviour
{
    private TaskManager.TaskInfo taskInfo;

    public void SetTaskInfo(TaskManager.TaskInfo task)
    {
        taskInfo = task;
    }

    public void Select()
    {
        ChatManager.Instance.SetTaskID(taskInfo.TaskID);
        ChatManager.Instance.Refresh();
    }
}