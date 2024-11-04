using System;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private List<SpriteInfo> SpriteInfos;

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
}