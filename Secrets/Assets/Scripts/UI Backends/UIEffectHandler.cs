using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.InputSystem; // Import DOTween namespace

public class UIEffectHandler : MonoBehaviour
{
    [SerializeField] private float shrinkDuration = 0.5f;
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private Vector3 ShowTargetScale = Vector3.one;
    [SerializeField] private Vector3 ShrinkTargetScale = Vector3.zero;
    [SerializeField] private Ease ShowEase = Ease.OutBack;
    [SerializeField] private Ease ShrinkEase = Ease.InBack;
    [SerializeField] private bool CloseOnClickOutside = true;

    private Coroutine handle;

    void OnEnable()
    {
        if (handle != null)
        {
            StopCoroutine(handle);
        }

        handle = StartCoroutine(InputHandle());
    }

    private IEnumerator InputHandle()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        while (gameObject)
        {
            GameUIManager.Instance.EscPause = false;
            if (Mouse.current.leftButton.isPressed && CloseOnClickOutside)
            {
                if (!IsPointerOverUIObject())
                {
                    ShrinkAndClose();
                }
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                ShrinkAndClose();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    public void ShrinkAndClose()
    {
        GameUIManager.Instance.TogglePause(false);
        transform.DOScale(ShrinkTargetScale, shrinkDuration).SetEase(ShrinkEase).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameUIManager.Instance.EscPause = true;
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        GameUIManager.Instance.TogglePause(true);
        transform.DOScale(ShowTargetScale, showDuration).SetEase(ShowEase).SetUpdate(true);
    }
}