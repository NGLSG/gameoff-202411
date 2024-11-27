using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public enum Gender
    {
        Male,
        Female
    }
    
    public enum SpecialIdentity
    {
        None,
        SpecialCharatcer,
        Teacher
    }
    
    [Header("Video Config")] 
    public string npcDialogueNode;
    public string eavesdroppingNode;
    public float eavesdroppingShowGapTime;

    [Header("Map Tag Config")] 
    [SerializeField] private string npcName;
    [SerializeField] private SpriteRenderer npcHeadPhoto;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI npcSpeak;

    [Header("Body Config")] 
    [SerializeField] private Gender gender;
    [SerializeField] private SpecialIdentity identity;
    [SerializeField] private string body;
    [SerializeField] private string hair;
    [SerializeField] private string arm;
    [SerializeField] private string bodyPath;
    [SerializeField] private string hairPath;
    [SerializeField] private string armPath;

    [Header("NPC Move Anim")]
    public Sprite frontSprite;

    [Header("NPC Audio Setting")]
    public AudioClip audioClip;
    public float triggerRadius = 5f;  // 播放音效的触发半径
    [SerializeField]private Transform playerTransform;  // 玩家位置
    private AudioSource audioSource;  // 用于播放音效的 AudioSource

    [Header("DO Tween Settings")]
    public float scaleDuration = 0.5f;  // 缩放动画持续时间
    public float scaleAmount = 0.14f;    // 压缩比例
    public int loopCount = -1;          // -1表示无限循环

    // 检测半径
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    
    // 访问状态
    [Header("Visited?")] public bool isVisited;

    private void Start()
    {
        // 初始化 NPC Speak
        npcSpeak.gameObject.SetActive(false);

        // 调用方法开始动画
        StartCompressionEffect();

        // 初始化玩家配置
        GenerateNPCConfig();
        audioSource = GetComponent<AudioSource>();
        npcNameText.text = npcName;
        isVisited = false;
    }

    private void Update()
    {
        // 在小圈内检测玩家是否靠近
        DetectPlayerInRadius();

        // 计算玩家与 NPC 的距离
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // 当玩家距离 NPC 小于触发范围时播放音效
        if (distanceToPlayer <= triggerRadius && !audioSource.isPlaying)
        {
            PlayNPCSound();
        }
        // 如果玩家离开了触发范围，则停止播放音效
        else if (distanceToPlayer > triggerRadius && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("NPC Sound Stop");
        }
    }

    void PlayNPCSound()
    {
        Debug.Log("NPC Sound Start");

        // 确保音效只播放一次
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    // 让sprite不断在y轴上轻微压缩和恢复
    private void StartCompressionEffect()
    {
        // 使用 DoTween 的 DOScale 来让SpriteRenderer在y轴上压缩和恢复
        npcHeadPhoto.transform.DOScaleY(scaleAmount, scaleDuration)
            .SetEase(Ease.InOutSine)  // 使用Ease的缓动效果
            .SetLoops(loopCount, LoopType.Yoyo);  // 无限循环，且每次反向播放
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
        Sprite[] bodyAssets;
        Sprite[] armAssets;
        Sprite[] hairAssets;
        Sprite[] npcHeadPhotos;

        // 判断是否有特殊身份
        if (identity != SpecialIdentity.None)
        {
            if (identity == SpecialIdentity.Teacher)
            {
                if (gender == Gender.Female)
                {
                    // 获取特殊身份的立绘路径
                    bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Female/Body");
                    armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Female/Arm");
                    hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Female/Hair");

                    // 存储路径
                    bodyPath = "NPCArtAssets/Teacher/Female/Body";
                    armPath = "NPCArtAssets/Teacher/Female/Arm";
                    hairPath = "NPCArtAssets/Teacher/Female/Hair";

                    npcHeadPhotos = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Female/Movement");
                    frontSprite = npcHeadPhotos[Random.Range(0, npcHeadPhotos.Length)];
                }
                else
                {
                    // 获取特殊身份的立绘路径
                    bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Male/Body");
                    armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Male/Arm");
                    hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Male/Hair");

                    // 存储路径
                    bodyPath = "NPCArtAssets/Teacher/Male/Body";
                    armPath = "NPCArtAssets/Teacher/Male/Arm";
                    hairPath = "NPCArtAssets/Teacher/Male/Hair";

                    npcHeadPhotos = Resources.LoadAll<Sprite>("NPCArtAssets/Teacher/Male/Movement");
                    frontSprite = npcHeadPhotos[Random.Range(0, npcHeadPhotos.Length)];
                }
            }
            else
            {
                // 获取特殊身份的立绘路径
                bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/SpecialCharacter/Body");
                armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/SpecialCharacter/Arm");
                hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/SpecialCharacter/Hair");
                
                // 动态获取当前NPC对应的资源
                body = Array.Find(bodyAssets, sprite => sprite.name.StartsWith(npcName)).name;
                arm = Array.Find(armAssets, sprite => sprite.name.StartsWith(npcName)).name;
                hair = Array.Find(hairAssets, sprite => sprite.name.StartsWith(npcName)).name;
                
                // 存储路径
                bodyPath = "NPCArtAssets/SpecialCharacter/Body";
                armPath = "NPCArtAssets/SpecialCharacter/Arm";
                hairPath = "NPCArtAssets/SpecialCharacter/Hair";
                
                frontSprite = Resources.Load<Sprite>($"NPCArtAssets/SpecialCharacter/Movement/{npcName}");
                npcHeadPhoto.sprite = frontSprite;
                return;
            }
        }
        else if (gender == Gender.Female)
        {
            // 获取普通学生的立绘路径
            bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Female/Body");
            armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Female/Arm");
            hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Female/Hair");

            // 存储路径
            bodyPath = "NPCArtAssets/Student/Female/Body";
            armPath = "NPCArtAssets/Student/Female/Arm";
            hairPath = "NPCArtAssets/Student/Female/Hair";

            npcHeadPhotos = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Female/Movement");
            frontSprite = npcHeadPhotos[Random.Range(0, npcHeadPhotos.Length)];
        }
        else
        {
            // 获取普通学生的立绘路径
            bodyAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Male/Body");
            armAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Male/Arm");
            hairAssets = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Male/Hair");

            // 存储路径
            bodyPath = "NPCArtAssets/Student/Male/Body";
            armPath = "NPCArtAssets/Student/Male/Arm";
            hairPath = "NPCArtAssets/Student/Male/Hair";

            npcHeadPhotos = Resources.LoadAll<Sprite>("NPCArtAssets/Student/Male/Movement");
            frontSprite = npcHeadPhotos[Random.Range(0, npcHeadPhotos.Length)];
        }

        // 随机选择资源
        body = bodyAssets[Random.Range(0, bodyAssets.Length)].name;
        arm = armAssets[Random.Range(0, armAssets.Length)].name;
        hair = hairAssets[Random.Range(0, hairAssets.Length)].name;

        npcHeadPhoto.sprite = frontSprite;
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
        npcConfig.Add("npcBodyPath", bodyPath);
        npcConfig.Add("npcHairPath", hairPath);
        npcConfig.Add("npcArmPath", armPath);

        return npcConfig;
    }

    public string GetNPCName()
    {
        return npcName;
    }
}
