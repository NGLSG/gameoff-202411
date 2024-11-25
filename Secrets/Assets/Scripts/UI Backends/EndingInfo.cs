using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EndingInfo", menuName = "EndingInfo")]
public class EndingInfo : ScriptableObject
{
    public Sprite Sprite;

    public LocalizedContent Content;

    public string Text
    {
        get
        {
            if (GameSetting.Setting.Language == "en")
            {
                return Content.En;
            }
            else
            {
                return Content.Cn;
            }
        }
    }
}