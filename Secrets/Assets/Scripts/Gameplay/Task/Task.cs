using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] private TaskManager.TaskInfo taskInfo;
    [SerializeField] private Image taskImage;

    public void SetTaskInfo(TaskManager.TaskInfo task)
    {
        taskInfo = task;
        //taskImage.sprite = Resources.Load<Sprite>($"Tasks/Task{task.TaskID}");
    }

    public void Select()
    {
        ChatManager.Instance.SetTaskID(taskInfo.TaskID);
        ChatManager.Instance.Refresh();
    }
}