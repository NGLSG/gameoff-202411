using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [SerializeField] private TaskManager.TaskInfo taskInfo;
    [SerializeField] private Image taskImage;
    [SerializeField] private TextMeshProUGUI NPC;

    public void SetTaskInfo(TaskManager.TaskInfo task)
    {
        taskInfo = task;
        NPC.text = task.NPCID;
        taskImage.sprite = ChatManager.Instance.GetNPCSprite(task.NPCID);
    }

    public void Select()
    {
        ChatManager.Instance.GetComponent<UIEffectHandler>().Show();
        ChatManager.Instance.SetTaskID(taskInfo.TaskID);
        TaskManager.Instance.GetComponent<UIEffectHandler>().ShrinkAndClose();
    }
}