using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Video Config")] 
    [SerializeField] private string eavesdroppingNode;
    [SerializeField] private string npcDialogueNode;

    [Header("Map Tag Config")] 
    [SerializeField] private SpriteRenderer npcHeadPhoto;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcSpeak;

    [Header("Body Config")] 
    [SerializeField] private string body;
    [SerializeField] private string hair;
    [SerializeField] private string arm;
    
    // 检测半径
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    private void Start()
    {
        // 初始化 NPC Speak
        npcSpeak.gameObject.SetActive(false);
        
        // 初始化玩家身体配置
        GenerateNPCConfig();
    }

    private void Update()
    {
        // 在小圈内检测玩家是否靠近
        DetectPlayerInRadius();
    }

    // 检测小圈内玩家
    private void DetectPlayerInRadius()
    {
        // Physics2D.OverlapCircle检测小圈内是否有玩家
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (playerCollider != null) // 检测到玩家
        {
            Debug.Log("Something is comming, will it be Player?");
            // Perform raycast from NPC to player to check for obstructions
            Vector2 direction = playerCollider.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius);

            // Ignore hits with NPC's own collider
            if (hit.collider != null && hit.collider.CompareTag("Player") && hit.collider.gameObject != gameObject)
            {
                Debug.Log("Oh!,it is Player!");
                if (Input.GetKeyDown(KeyCode.E) && !DialogueSystem.Instance.IsDialogueRunning())
                {
                    Chatting(npcDialogueNode);
                    
                    // 修改玩家为正在对话状态
                    PlayerController.Instance.SetChattingState(true);
                }

                if (!PlayerController.Instance.IsChatting())
                {
                    npcSpeak.gameObject.SetActive(true);
                    StopChatting();
                }
            }
            else
            {
                // 如果当前没有正在进行的对话，同时偷听节点不为空（即又偷听内容要展示）
                if (!DialogueSystem.Instance.IsDialogueRunning() && !string.IsNullOrEmpty(eavesdroppingNode))
                {
                    StartEavesDropping(eavesdroppingNode);
                }
            }
        }
        else
        {
            // 当玩家不在NPC身边，关闭显示NPC的界面台词显示,同时停止对白聊天
            npcSpeak.gameObject.SetActive(false);
            StopChatting();
        }
    }


    // 随机选择NPC立绘的身体
    private void GenerateNPCConfig()
    {
        // Load all assets from Resources folders
        // string[] bodyAssets = Resources.LoadAll<TextAsset>("NPCArtAssets/Body");
        // string[] armAssets = Resources.LoadAll<TextAsset>("NPCArtAssets/Arm");
        // string[] hairAssets = Resources.LoadAll<TextAsset>("NPCArtAssets/Hair");
        
        //Randomly pick one file from each folder
        // body = bodyAssets[Random.Range(0, bodyAssets.Length)].name;
        // arm = armAssets[Random.Range(0, armAssets.Length)].name;
        // hair = hairAssets[Random.Range(0, hairAssets.Length)].name;
        
    }

    // Handle chatting with NPC
    public void Chatting(string dialogueNode)
    {
        if (!string.IsNullOrEmpty(dialogueNode))
        {
            DialogueSystem.Instance.StartDialogueWithPause(hair, body, arm, dialogueNode);
        }
    }

    // 停止对话
    public void StopChatting()
    {
        DialogueSystem.Instance.StopDialogue();
    }

    // 开始偷听
    public void StartEavesDropping(string eavesdroppingNode)
    {
        if (!string.IsNullOrEmpty(eavesdroppingNode))
        {
            DialogueSystem.Instance.StartDialogueWithPause(hair, body, arm, eavesdroppingNode);
        }
    }

    // Draw Gizmos for detecting radius (only in the Scene view)
    private void OnDrawGizmos()
    {
        // Set the color for the gizmo (e.g., green for detection range)
        Gizmos.color = Color.green;
        
        // Draw a wireframe sphere to indicate the detection radius
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
