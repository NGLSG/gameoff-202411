﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.InputSystem; // Import DOTween namespace

public class UIEffectHandler : MonoBehaviour
{
    [SerializeField] private float shrinkDuration = 0.5f;
    [SerializeField] private Vector3 targetScale = Vector3.zero;
    [SerializeField] private Ease ShowEase = Ease.OutBack;
    [SerializeField] private Ease ShrinkEase = Ease.InBack;
    [SerializeField] private bool CloseOnClickOutside = true;

    private Coroutine handle;

    void Start()
    {
        handle = StartCoroutine(InputHandle());
    }

    private IEnumerator InputHandle()
    {
        while (gameObject)
        {
            GameUIManager.Instance.EscPause = false;
            if (Input.GetMouseButtonDown(0) && CloseOnClickOutside)
            {
                if (!IsPointerOverUIObject())
                {
                    ShrinkAndClosePhone();
                }
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                ShrinkAndClosePhone();
                GameUIManager.Instance.EscPause = true;
            }

            yield return null;
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

    public void ShrinkAndClosePhone()
    {
        transform.DOScale(targetScale, shrinkDuration).SetEase(ShrinkEase).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 0.5f).SetEase(ShowEase);
    }
}