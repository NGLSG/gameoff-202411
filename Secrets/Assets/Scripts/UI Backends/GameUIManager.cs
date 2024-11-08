using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [Serializable]
    public struct SpriteInfo
    {
        public Sprite Sprite;
        public string Name;
    }

    [SerializeField] private Image ShowingSprite1;
    [SerializeField] private Image ShowingSprite2;
    [SerializeField] private GameObject Phone;
    [SerializeField] private List<UIEffectHandler> UIsOnPhoneShowShouldClose = new List<UIEffectHandler>();
    [SerializeField] private List<SpriteInfo> SpriteInfos;
    [SerializeField] private TextMeshProUGUI TimeText;

    private InputManager m_InputManager;
    private Coroutine handle;
    private bool isPaused = false;

    public bool EscPause = true;

    public void ShowSprite(string spriteName, bool show = true, int idxOfImgToShow = 0)
    {
        foreach (var spriteInfo in SpriteInfos)
        {
            if (spriteInfo.Name == spriteName)
            {
                if (idxOfImgToShow == 0)
                {
                    if (show)
                        ShowingSprite1.sprite = spriteInfo.Sprite;
                    else
                        ShowingSprite1.gameObject.SetActive(false);
                }
                else
                {
                    if (show)
                        ShowingSprite2.sprite = spriteInfo.Sprite;
                    else
                        ShowingSprite2.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShowPhone(bool show)
    {
        Phone.SetActive(show);
        if (show)
        {
            Phone.GetComponent<UIEffectHandler>().Show();
        }
    }

    public void TogglePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        isPaused = pause;
        GameTimeManager.Instance.StopTime(pause);
    }

    private void Start()
    {
        m_InputManager = InputManager.Instance;
        handle = StartCoroutine(UpdateBackend());
    }

    private IEnumerator UpdateBackend()
    {
        while (gameObject)
        {
            TimeText.text = GameTimeManager.Instance.currentFactualTime.GetTime2();
            if (m_InputManager.IsActionPressed("OpenPhone"))
            {
                foreach (var ui in UIsOnPhoneShowShouldClose)
                {
                    ui.ShrinkAndClose();
                }

                ShowPhone(true);
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                if (EscPause)
                {
                    TogglePause(!isPaused);
                }
            }

            yield return null;
        }
    }
}