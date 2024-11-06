using System;
using System.Collections;
using DG.Tweening;
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

    public Attributes PlayerAttributes;
    private Rigidbody2D _rigidbody2D;
    private PlayerControls _playerControls;
    private Coroutine _moveCoroutine;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            DialogueSystem.Instance.SetLanguage("en");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DialogueSystem.Instance.StartDialogueWithPause(Color.cyan, "Start");
        }
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}