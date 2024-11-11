using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Checkbox : MonoBehaviour
{
    public List<Action<bool>> OnChange = new List<Action<bool>>();
    public bool isOn = false;
    [SerializeField] private Sprite SpriteOn;
    [SerializeField] private Sprite SpriteOff;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.image.sprite = isOn ? SpriteOn : SpriteOff;
        btn.onClick.AddListener(() =>
        {
            isOn = !isOn;
            btn.image.sprite = isOn ? SpriteOn : SpriteOff;
            foreach (var action in OnChange)
            {
                action(isOn);
            }
        });
    }
}