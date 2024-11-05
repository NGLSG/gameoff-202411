using System;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private List<SpriteInfo> SpriteInfos;
    private InputManager m_InputManager;

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
            Phone.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void Start()
    {
        m_InputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (m_InputManager.IsActionPressed("OpenPhone"))
        {
            ShowPhone(true);
        }
    }
}