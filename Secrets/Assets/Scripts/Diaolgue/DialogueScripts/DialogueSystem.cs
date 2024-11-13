using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.IO;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    public void Awake()
    {
        Instance = this;
    }

    public DialogueRunner dialogueRunner;
    public CustomLineView lineView;
    public Image CharacterBodyImage;
    public Image CharacterArmImage;
    public Image CharacterHairImage;

    private Coroutine dialogueCoroutine;
    
    // 数据存储类
    [Serializable]
    public class ClueData
    {
        public string title;
        public string cluesType;
        public List<ClueContent> content;

        public ClueData(string title, string cluesType, List<ClueContent> content)
        {
            this.title = title;
            this.cluesType = cluesType;
            this.content = content;
        }
    }

    // 数据项存储类
    [Serializable]
    public class ClueContent
    {
        public string Speaker;
        public string Text;

        public ClueContent(string speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }

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
                CharacterBodyImage.sprite = Resources.Load<Sprite>($"{characterConfig["npcBodyPath"]}/{characterConfig["npcBody"]}");
            }
        
            if (characterConfig.ContainsKey("npcArm"))
            {
                CharacterArmImage.sprite = Resources.Load<Sprite>($"{characterConfig["npcArmPath"]}/{characterConfig["npcArm"]}");
            }
        
            if (characterConfig.ContainsKey("npcHair"))
            {
                CharacterHairImage.sprite = Resources.Load<Sprite>($"{characterConfig["npcHairPath"]}/{characterConfig["npcHair"]}");
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
    
    // 重置NPC立绘
    [YarnCommand("change_score")]
    public void ChangeScore(string npcName)
    {
        // 获取场景中所有带有NPC脚本的游戏对象
        NPC[] npcs = FindObjectsOfType<NPC>();

        // 遍历每个NPC
        foreach (NPC npc in npcs)
        {
            // 判断NPC的名字是否与传入的名字相同
            if (npc.GetNPCName() == npcName)
            {
                if (!npc.isVisited)
                {
                    // 设置isVisited为true
                    npc.isVisited = true;
                    // 增加exploreScore的值
                    GameManager.Instance.GetGameData().ChangeExploreScore(1);
                    Debug.Log($"Visited {npcName}: exploreScore increased to {GameManager.Instance.GetGameData().GetExploreScore()}");
                }
            }
        }
    }

    [YarnCommand("unlock_option")]
    public void UnlockOption(string taskID, string optionID)
    {
        // 
    }
    
    // 输出Json线索
    [YarnCommand("save_clues")]
    public void SaveCluesToJson(string title, string dialogueType)
    {
        // 获取Clues数据并转换为List<ClueContent>
        List<ClueContent> cluesContent = new List<ClueContent>();
        foreach (string clue in lineView.GetClues())
        {
            string[] parts = clue.Split(':'); // 调整格式为 "Speaker : Text"
            if (parts.Length == 2)
            {
                cluesContent.Add(new ClueContent(parts[0].Trim(), parts[1].Trim()));
            }
        }

        // 创建ClueData对象
        ClueData clueData = new ClueData(title, dialogueType, cluesContent);

        // 将ClueData对象转换为JSON字符串
        string json = JsonUtility.ToJson(clueData, true);

        // 设置文件保存路径
        string filePath = Path.Combine(Application.dataPath, "Resources/Clues", $"{title}.json");

        // 创建文件夹（如果不存在）
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 保存JSON到文件
        File.WriteAllText(filePath, json);

        Debug.Log($"Clues saved to {filePath}");

        // 清空clues列表
        lineView.InitClues();
    }

    
    // 读取所有的Json
    public ClueData[] LoadAllCluesFromJson()
    {
        // Clues 文件夹路径
        string folderPath = Path.Combine(Application.dataPath, "Resources/Clues");

        // 获取文件夹下所有 JSON 文件路径
        string[] files = Directory.GetFiles(folderPath, "*.json");

        // 用于存储所有 ClueData 的列表
        List<ClueData> cluesList = new List<ClueData>();

        // 遍历每个文件并加载数据
        foreach (string filePath in files)
        {
            string json = File.ReadAllText(filePath);
            ClueData clueData = JsonUtility.FromJson<ClueData>(json);

            if (clueData != null)
            {
                cluesList.Add(clueData);
                Debug.Log($"Loaded clues from {filePath}");
            }
            else
            {
                Debug.LogWarning($"Failed to load clues from {filePath}");
            }
        }

        // 将列表转换为数组并返回
        return cluesList.ToArray();
    }


}
