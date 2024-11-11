using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Video Config")] 
    public string npcDialogueNode;
    public string eavesdroppingNode;
    public float eavesdroppingShowGapTime;

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
            //Debug.Log("Something is comming, will it be Player?");
        
            // 检测两者之间是否存在碰撞体
            Vector2 direction = playerCollider.transform.position - transform.position;

            // 设置LayerMask忽略自己的碰撞体
            int layerMask = ~(1 << gameObject.layer); // 忽略当前物体所在的Layer

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, layerMask);

            // 如果射线碰撞到的不是玩家自身
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if (!PlayerController.Instance.IsChatting()) // 如果玩家没有在聊天
                {
                    npcSpeak.gameObject.SetActive(true);
                }
            }
            else
            {
                npcSpeak.gameObject.SetActive(false);
            }
        }
        else
        {
            // 当玩家不在NPC身边，关闭显示NPC的界面台词显示
            npcSpeak.gameObject.SetActive(false);
        }
    }

    
    // 随机选择NPC立绘的身体
    private void GenerateNPCConfig()
    {
        // Load all assets from Resources folders (assumes PNG files in these folders)
        Sprite[] bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Body");
        Sprite[] armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Arm");
        Sprite[] hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Hair");

        // Randomly pick one file from each folder and get its name
        body = bodyAssets[Random.Range(0, bodyAssets.Length)].name;
        arm = armAssets[Random.Range(0, armAssets.Length)].name;
        hair = hairAssets[Random.Range(0, hairAssets.Length)].name;
    }

    // Draw Gizmos for detecting radius (only in the Scene view)
    private void OnDrawGizmos()
    {
        // Set the color for the gizmo (e.g., green for detection range)
        Gizmos.color = Color.green;
        
        // Draw a wireframe sphere to indicate the detection radius
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    public Dictionary<string,string> GetConfigOfNPC()
    {
        Dictionary<string, string> npcConfig = new Dictionary<string, string>();
        npcConfig.Add("npcBody", body);
        npcConfig.Add("npcHair", hair);
        npcConfig.Add("npcArm", arm);

        return npcConfig;
    }

    public string GetNPCName()
    {
        return npcName.text;
    }
}
