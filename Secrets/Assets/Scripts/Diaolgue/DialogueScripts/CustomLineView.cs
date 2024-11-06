using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class CustomLineView : LineView
{
    [Header("Storage the Conversation")]
    public TextMeshProUGUI TextCustom;
    public TextMeshProUGUI CharacterName;
    
    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        base.InterruptLine(dialogueLine, onInterruptLineFinished);
        
        // TODO: Store the Conversation
        Debug.Log(TextCustom.text + CharacterName.text);
    }
}
