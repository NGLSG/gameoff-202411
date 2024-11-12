using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Singleton<PlayerController>
{
    [Serializable]
    public struct Attributes
    {
        public float RunSpeed;
        public float WalkSpeed;
    }

    public enum DialogueType
    {
        Chat,
        Eavsdrop
    }

    public Attributes PlayerAttributes;
    private Rigidbody2D _rigidbody2D;
    private PlayerControls _playerControls;
    private Coroutine _moveCoroutine;

    // 玩家状态变量
    private bool isChatting;
    private bool isEvasdropping;

    // 玩家检测NPC
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask npcLayer;
    [SerializeField] private TextMeshProUGUI noteText;

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
        }

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _playerControls.Enable();
        _moveCoroutine = StartCoroutine(PlayerInputHandler());
    }

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 设置玩家聊天的状态为
        isChatting = false;
        isEvasdropping = false;
        noteText.gameObject.SetActive(false);
        DialogueSystem.Instance.SetLanguage(GameSetting.Setting.Language);
    }

    IEnumerator PlayerInputHandler()
    {
        while (gameObject.activeInHierarchy)
        {
            Vector2 moveVector2 = _playerControls.Player.Move.ReadValue<Vector2>();

            if (moveVector2 != Vector2.zero)
            {
                MovePlayer(moveVector2);
            }


            yield return null;
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        // 使用 Rigidbody2D 的 MovePosition 方法进行移动
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 newPosition = currentPosition + direction * PlayerAttributes.RunSpeed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(newPosition);
    }

    void Update()
    {
        // 这里可以处理其他逻辑，例如动画等
        if (Input.GetKeyDown(KeyCode.C))
        {
            DialogueSystem.Instance.SetLanguage("en");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            DialogueSystem.Instance.StartDialogueWithPause("No.壹号", "Start");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DialogueSystem.ClueData[] clueDatas = DialogueSystem.Instance.LoadAllCluesFromJson();
            foreach (var item in clueDatas)
            {
                Debug.Log(item.title);
            }
        }

        SenseNPC();
    }

    private void SenseNPC()
    {
        // 获取所有在检测范围内的NPC
        Collider2D[] npcsInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, npcLayer);
        Debug.Log($"Sense {npcsInRange.Length} NPC!!");

        if (npcsInRange.Length > 0)
        {
            // 找到最近的NPC
            Collider2D closestNPC = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider2D npc in npcsInRange)
            {
                float distance = Vector2.Distance(transform.position, npc.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestNPC = npc;
                }
            }

            if (closestNPC != null)
            {
                //Debug.Log($"the nearest NPC is {closestNPC.name}");
                NPC npcScript = closestNPC.GetComponent<NPC>();

                // 检测两者之间是否有其他碰撞体
                Vector2 direction = closestNPC.transform.position - transform.position;

                // 设置LayerMask忽略自己的碰撞体
                int layerMask = ~(1 << gameObject.layer); // 忽略当前物体所在的Layer
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, layerMask);


                // 如果没有障碍物直接接触到了NPC
                if (hit.collider != null && hit.collider.CompareTag("NPC"))
                {
                    // 若正在偷听，停止对话
                    if (isEvasdropping)
                    {
                        StopDialogue();
                        isEvasdropping = false;
                    }

                    DealWithDialogueType(npcScript, DialogueType.Chat);
                }
                else
                {
                    if (isChatting)
                    {
                        StopDialogue();
                        isChatting = false;
                    }

                    DealWithDialogueType(npcScript, DialogueType.Eavsdrop);
                }
            }
        }
        else
        {
            // close note
            noteText.gameObject.SetActive(false);

            // stop dialogue
            StopDialogue();
        }
    }

    // 处理对话逻辑
    private void DealWithDialogueType(NPC npcScript, DialogueType dialogueType)
    {
        // 修改提示词
        if (!(isChatting || isEvasdropping))
        {
            if (dialogueType == DialogueType.Chat) noteText.text = "Press 'E' to chat.";
            else noteText.text = "Press 'E' to eavesdrop.";

            noteText.gameObject.SetActive(true);
        }

        if (npcScript != null) // 确保NPC身上有NPCScript，执行对话
        {
            if (Input.GetKeyDown(KeyCode.E) && !DialogueSystem.Instance.IsDialogueRunning())
            {
                // close note
                noteText.gameObject.SetActive(false);
                switch (dialogueType)
                {
                    case DialogueType.Chat:
                        // set isChatting
                        isChatting = true;
                        // start dialogue
                        Chatting(npcScript);
                        break;
                    case DialogueType.Eavsdrop:
                        // set isEvasdropping
                        isEvasdropping = true;
                        // 如果当前没有正在进行的对话，同时偷听节点不为空（即又偷听内容要展示）
                        StartEavesDropping(npcScript);
                        break;
                }
            }
        }
    }

    // 和NPC对话
    public void Chatting(NPC npc)
    {
        if (!string.IsNullOrEmpty(npc.npcDialogueNode))
        {
            DialogueSystem.Instance.StartDialogueWithPause(npc.GetNPCName(), npc.npcDialogueNode);
        }
    }

    // 开始偷听
    public void StartEavesDropping(NPC npc)
    {
        if (!string.IsNullOrEmpty(npc.eavesdroppingNode))
        {
            DialogueSystem.Instance.StartDialogue(npc.GetNPCName(), npc.eavesdroppingNode,
                npc.eavesdroppingShowGapTime);
        }
    }

    // 停止对话
    public void StopDialogue()
    {
        DialogueSystem.Instance.StopDialogue();
    }


    private void OnDisable()
    {
        Debug. Log("here");
        _playerControls.Disable();
    }

    public bool IsChatting()
    {
        return isChatting;
    }

    public void SetChattingState(bool newState)
    {
        isChatting = newState;
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