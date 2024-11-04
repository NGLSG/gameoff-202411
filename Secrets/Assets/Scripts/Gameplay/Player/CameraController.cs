using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Camera _mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance != null)
        {
            _mainCamera.transform.DOMove(
                new Vector3(PlayerController.Instance.transform.position.x,
                    PlayerController.Instance.transform.position.y, _mainCamera.transform.position.z), 0.5f);
        }
    }
}