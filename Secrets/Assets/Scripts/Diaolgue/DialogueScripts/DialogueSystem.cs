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
    public Image CharacterBodyImage;
    public Image CharacterArmImage;
    public Image CharacterHairImage;

    private Coroutine dialogueCoroutine;

    public void StartDialogueWithPause(string characterHair, string characterBody, string characterArm, string dialogueName)
    {
        //TODO: Set characterPaint as the paint we want
        ApplyBodyConfig(characterHair, characterBody, characterArm);
        
        //Start dialogue
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        dialogueRunner.StartDialogue(dialogueName);
    }

    public void SetLanguage(string languageCode)
    {
        AudioLineProvider textAudioLanguage = dialogueRunner.GetComponent<AudioLineProvider>();
        textAudioLanguage.textLanguageCode = languageCode;
    }

    public void StopDialogue()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
    }

    public bool IsDialogueRunning()
    {
        return dialogueRunner.IsDialogueRunning;
    }
    
    // Apply body, arm, and hair configuration to NPC
    private void ApplyBodyConfig(string characterHair, string characterBody, string characterArm)
    {
        // Assign the body, arm, and hair by loading from resources or directly applying sprites
        CharacterBodyImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Body/{characterBody}");
        CharacterArmImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Arm/{characterArm}");
        CharacterHairImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Hair/{characterHair}");
    }

    public void ResetPlayerChattingState()
    {
        PlayerController.Instance.SetChattingState(false);
    }
}
