using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    public void Awake()
    {
        Instance = this;
    }

    public DialogueRunner dialogueRunner;
    public Image characterPaint;

    public void StartDialogueWithPause(Color characterName, string dialogueName)
    {
        //TODO: Set characterPaint as the paint we want
        characterPaint.color = characterName;
        
        //Start dialogue
        dialogueRunner.StartDialogue(dialogueName);
    }

    public void SetLanguage(string languageCode)
    {
        AudioLineProvider textAudioLanguage = dialogueRunner.GetComponent<AudioLineProvider>();
        textAudioLanguage.textLanguageCode = languageCode;
    }
}
