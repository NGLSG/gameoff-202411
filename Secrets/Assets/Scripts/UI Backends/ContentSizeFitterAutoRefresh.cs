using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitterAutoRefresh : MonoBehaviour
{
    private VerticalLayoutGroup fitterVerticalLayoutGroup;
    private HorizontalLayoutGroup fitterHorizontalLayoutGroup;
    private Coroutine handle;
    public bool IsVertical = false;
    public bool IsHorizontal = false;

    private void Awake()
    {
        if (IsVertical)
            fitterVerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        if (IsHorizontal)
            fitterHorizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        handle = StartCoroutine(Handler());
    }

    private IEnumerator Handler()
    {
        while (gameObject)
        {
            if (fitterVerticalLayoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(fitterVerticalLayoutGroup.GetComponent<RectTransform>());
            }

            if (fitterHorizontalLayoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(fitterHorizontalLayoutGroup.GetComponent<RectTransform>());
            }
            yield return null;
        }
    }
}