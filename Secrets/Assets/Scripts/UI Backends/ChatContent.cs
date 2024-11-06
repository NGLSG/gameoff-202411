using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatContent : MonoBehaviour
{
    public TextMeshProUGUI content;
    public Image Background;
    public Image Avatar;
    public RectTransform MaxRect;
    private Coroutine handle;

    private void Awake()
    {
        MaxRect = transform.parent.GetComponent<RectTransform>();
        handle = StartCoroutine(Handler());
    }

    private IEnumerator Handler()
    {
        while (gameObject)
        {
            Background.rectTransform.sizeDelta =
                new Vector2(
                    Math.Min(content.preferredWidth + 10, MaxRect.rect.width - 63),
                    content.preferredHeight);
            var maxHeight = MaxRect.sizeDelta;
            maxHeight.y = Math.Max(content.preferredHeight + 10, 64);
            MaxRect.sizeDelta = maxHeight;
            yield return null;
        }
    }
}