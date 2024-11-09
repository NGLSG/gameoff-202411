using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskOption : MonoBehaviour
{
    public TaskOptionInfo TaskOptionInfo;
    [SerializeField] private TextMeshProUGUI OptionText;
    private Coroutine handle;
    private Button btn;
    private RectTransform ParentRectTransform;

    public void SetTaskOptionInfo(TaskOptionInfo info)
    {
        TaskOptionInfo = info;
    }

    public void Select()
    {
        switch (TaskOptionInfo.sType)
        {
            case TaskOptionInfo.OptionType.Normal:
            case TaskOptionInfo.OptionType.Excellent:
            case TaskOptionInfo.OptionType.Ordinary:
                GameManager.Instance.GetGameData().ChangerAnswerScore(TaskOptionInfo.sScore);
                break;
            case TaskOptionInfo.OptionType.Personal:
                GameManager.Instance.GetGameData().ChangePersonalityScore(TaskOptionInfo.sScore);
                break;
            case TaskOptionInfo.OptionType.TrueEnding:
                GameManager.Instance.GetGameData().SetTrueEnding(true);
                break;
        }
    }

    private void OnEnable()
    {
        if (handle != null)
        {
            StopCoroutine(handle);
        }

        ParentRectTransform = transform.parent.GetComponent<RectTransform>();
        btn = GetComponent<Button>();
        handle = StartCoroutine(Handler());
    }

    private IEnumerator Handler()
    {
        while (gameObject)
        {
            OptionText.text = TaskOptionInfo.sContent;
            btn.GetComponent<RectTransform>().sizeDelta =
                new Vector2(Math.Min(OptionText.preferredWidth + 10, ParentRectTransform.sizeDelta.x),
                    OptionText.preferredHeight + 20);
            ParentRectTransform.sizeDelta =
                new Vector2(ParentRectTransform.sizeDelta.x, OptionText.preferredHeight + 20);
            yield return null;
        }
    }
}