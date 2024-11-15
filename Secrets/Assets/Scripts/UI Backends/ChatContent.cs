using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatContent : MonoBehaviour
{
    public TextMeshProUGUI content;
    public Image Background;
    public Image Avatar;
    public DialogueInfo DialogueInfo;
    public RectTransform MaxRect;
    public bool NeedAnim = true;
    private Coroutine handle;
    [SerializeField] private PreloadAnim preloadAnim;


    void Awake()
    {
        if (handle != null)
        {
            StopCoroutine(handle);
        }

        if (NeedAnim)
            PlayShowAnim();
        MaxRect = transform.parent.GetComponent<RectTransform>();
        handle = StartCoroutine(Handler());
    }

    public void SetDialogueInfo(DialogueInfo info)
    {
        DialogueInfo = info;
        content.text = info.Dialogue;
    }

    public void PlayShowAnim()
    {
        if (preloadAnim == null)
        {
            return;
        }

        preloadAnim.gameObject.SetActive(true);
        preloadAnim.Play(UnityEngine.Random.Range(0.5f, 1f));
        StartCoroutine(AnimHandler());
    }

    private IEnumerator AnimHandler()
    {
        content.gameObject.SetActive(false);
        Background.enabled = false;
        while (!preloadAnim.stopPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        content.gameObject.SetActive(true);
        Background.enabled = true;
    }

    private IEnumerator Handler()
    {
        while (gameObject)
        {
            Background.rectTransform.sizeDelta =
                new Vector2(
                    Math.Min(content.preferredWidth + 10, MaxRect.rect.width - 32),
                    content.preferredHeight);
            var maxHeight = MaxRect.sizeDelta;
            maxHeight.y = content.preferredHeight + 20;
            MaxRect.sizeDelta = maxHeight;
            yield return new WaitForEndOfFrame();
        }
    }
}