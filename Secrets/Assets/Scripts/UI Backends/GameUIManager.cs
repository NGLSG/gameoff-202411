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

    [Serializable]
    [CreateAssetMenu(fileName = "EndingInfo", menuName = "EndingInfo")]
    public class EndingInfo : ScriptableObject
    {
        public Sprite Sprite;
        public string Text;
    }

    [SerializeField] private Image ShowingSprite1;
    [SerializeField] private Image ShowingSprite2;
    [SerializeField] private GameObject Phone;
    [SerializeField] private List<UIEffectHandler> UIsOnPhoneShowShouldClose = new List<UIEffectHandler>();
    [SerializeField] private List<SpriteInfo> SpriteInfos;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private GameObject Setting;
    [SerializeField] private Image GameOverImage;
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private EndingInfo[] EnddingInfos = new EndingInfo[5];
    [SerializeField] private GameObject GameOver;


    private InputManager m_InputManager;
    private Coroutine handle;
    private bool isPaused = false;

    private volatile bool escPause = true;
    private readonly object lockObject = new object();

    public bool EscPause
    {
        get
        {
            lock (lockObject)
            {
                return escPause;
            }
        }
        set
        {
            lock (lockObject)
            {
                escPause = value;
            }
        }
    }

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

            if (EscPause)
            {
                if (Keyboard.current.escapeKey.isPressed)
                {
                    Setting.GetComponent<UIEffectHandler>().Show();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    // 设置结局页面
    public void SetEndding(GameData.EnddingState enddingState)
    {
        switch (enddingState)
        {
            case GameData.EnddingState.LazyEndding:
                GameOverImage.sprite = EnddingInfos[0].Sprite;
                GameOverText.text = EnddingInfos[0].Text;
                break;
            case GameData.EnddingState.NormalEndding:
                GameOverImage.sprite = EnddingInfos[1].Sprite;
                GameOverText.text = EnddingInfos[1].Text;
                break;
            case GameData.EnddingState.PersonalityEndding:
                GameOverImage.sprite = EnddingInfos[2].Sprite;
                GameOverText.text = EnddingInfos[2].Text;
                break;
            case GameData.EnddingState.AlienEndding:
                GameOverImage.sprite = EnddingInfos[3].Sprite;
                GameOverText.text = EnddingInfos[3].Text;
                break;
            case GameData.EnddingState.PerfectEndding:
                GameOverImage.sprite = EnddingInfos[4].Sprite;
                GameOverText.text = EnddingInfos[4].Text;
                break;
            default:
                break;
        }
        
        GameOver.SetActive(true);
    }
}