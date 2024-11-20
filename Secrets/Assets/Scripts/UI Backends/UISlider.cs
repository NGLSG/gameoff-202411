using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlider : MonoBehaviour
{
    [SerializeField] private RectTransform Fill;
    [SerializeField] private float MaxWidth;
    private float _rate;

    public float Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            SetRate(_rate);
        }
    }

    public void SetRate(float rate)
    {
        if (rate < 0)
        {
            _rate = 0;
            throw new Exception("Can not set rate less than 0");
        }

        if (rate > 1)
        {
            _rate = 1;
            throw new Exception("Can not set rate more than 1");
        }

        _rate = rate;
        Fill.sizeDelta = new Vector2(MaxWidth * rate, Fill.sizeDelta.y);
    }
#if UNITY_EDITOR
    [ContextMenu("TestAdd")]
    public void Add()
    {
        Rate += 0.1f;
    }
#endif
    private void Start()
    {
        SetRate(0);
    }
}