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
    public LineView lineView;
    public Image CharacterBodyImage;
    public Image CharacterArmImage;
    public Image CharacterHairImage;

    private Coroutine dialogueCoroutine;

    // 开启对话
    public void StartDialogueWithPause(string characterName, string dialogueName)
    {
        //TODO: Set characterPaint as the paint we want
        ApplyBodyConfig(characterName);
        
        //Start dialogue
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        dialogueRunner.StartDialogue(dialogueName);
    }
    
    // 开启偷听
    public void StartDialogue(string characterName, string dialogueNode, float timeToWait)
    {
        //TODO: Set characterPaint as the paint we want
        ApplyBodyConfig(characterName);
        
        // 停止之前的协程（如果有的话）
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
    
        dialogueRunner.StartDialogue(dialogueNode);
        dialogueCoroutine = StartCoroutine(AutoAdvance(timeToWait));
    }

    private IEnumerator AutoAdvance(float timeToWait)
    {
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return new WaitForSeconds(timeToWait);
            lineView.OnContinueClicked();
        }
    }

    // 停止对话
    public void StopDialogue()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
    }
    
    // 检测是否有对话正在运行
    public bool IsDialogueRunning()
    {
        return dialogueRunner.IsDialogueRunning;
    }

    // 切换语言
    public void SetLanguage(string languageCode)
    {
        AudioLineProvider textAudioLanguage = dialogueRunner.GetComponent<AudioLineProvider>();
        textAudioLanguage.textLanguageCode = languageCode;
    }
    
    // 根据场景内的玩家信息拼装立绘
    private void ApplyBodyConfig(string characterName)
    {
        // 获取所有的NPC对象
        NPC[] npcs = GameObject.FindObjectsOfType<NPC>();

        GameObject npc = null;

        // 遍历所有的NPC对象，找到名字匹配的NPC
        foreach (var npcComponent in npcs)
        {
            if (npcComponent.GetNPCName() == characterName)
            {
                npc = npcComponent.gameObject;
                break; // 找到第一个匹配的NPC，退出循环
            }
        }

        if (npc != null)
        {
            Dictionary<string, string> characterConfig = npc.GetComponent<NPC>().GetConfigOfNPC();
            
            if (characterConfig.ContainsKey("npcBody"))
            {
                CharacterBodyImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Body/{characterConfig["npcBody"]}");
            }
        
            if (characterConfig.ContainsKey("npcArm"))
            {
                CharacterArmImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Arm/{characterConfig["npcArm"]}");
            }
        
            if (characterConfig.ContainsKey("npcHair"))
            {
                CharacterHairImage.sprite = Resources.Load<Sprite>($"NPCArtAssets/Hair/{characterConfig["npcHair"]}");
            }
        }
        else
        {
            Debug.LogError("NPC not found!");
        }
    }
    
    public void ResetPlayerChattingState()
    {
        PlayerController.Instance.SetChattingState(false);
        Debug.Log("DialogueFinished");
    }
    
    // 重置NPC立绘
    [YarnCommand("switch_character")]
    public void SwitchCharacterPaint(string characterName)
    {
        ApplyBodyConfig(characterName);
    }
}
