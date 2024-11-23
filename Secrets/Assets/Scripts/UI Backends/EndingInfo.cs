using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "EndingInfo", menuName = "EndingInfo")]
public class EndingInfo : ScriptableObject
{
    public Sprite Sprite;
    public string Text;
}