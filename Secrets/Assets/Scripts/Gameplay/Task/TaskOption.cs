using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskOption : MonoBehaviour
{
    public TextMeshProUGUI content;
    public Image Background;
    public RectTransform MaxRect;
    private Coroutine handle;
    [SerializeField] private PreloadAnim preloadAnim;
    public TaskOptionInfo TaskOptionInfo;


    void Start()
    {
        if (handle != null)
        {
            StopCoroutine(handle);
        }

        MaxRect = transform.parent.GetComponent<RectTransform>();
        handle = StartCoroutine(Handler());
        GetComponent<Button>().onClick.AddListener(Select);
    }


    private IEnumerator Handler()
    {
        while (gameObject)
        {
            Background.rectTransform.sizeDelta =
                new Vector2(
                    Math.Min(content.preferredWidth + 10, MaxRect.rect.width - 32),
                    content.preferredHeight + 20);
            var maxHeight = MaxRect.sizeDelta;
            maxHeight.y = content.preferredHeight + 20;
            MaxRect.sizeDelta = maxHeight;
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetTaskOptionInfo(TaskOptionInfo info)
    {
        TaskOptionInfo = info;
        content.text = info.sContent;
    }
    
    public void Select()
    {
        ChatManager.Instance.SelectTaskOption(TaskOptionInfo);
    }
}