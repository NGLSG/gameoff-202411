using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public class CustomLineView : LineView
{
    [Header("Storage the Conversation")]
    public TextMeshProUGUI TextCustom;
    public TextMeshProUGUI CharacterName;

    private List<string> clues = new List<string>();

    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        base.InterruptLine(dialogueLine, onInterruptLineFinished);

        Debug.Log(TextCustom.text + CharacterName.text);
        clues.Add($"{CharacterName.text} : {TextCustom.text}");


    }

    public List<string> GetClues()
    {
        return clues;
    }

    public void InitClues()
    {
        // 清空clues列表
        clues.Clear();
    }
}