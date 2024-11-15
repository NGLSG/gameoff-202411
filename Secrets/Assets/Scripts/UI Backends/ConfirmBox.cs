using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBox : MonoBehaviour
{
    public GameObject confirmBox;

    public void FinishGame()
    {
        GameManager.Instance.stateMechine.SetState("GameFinishState");
    }

    public void BackToGame()
    {
        confirmBox.SetActive(false);
    }
}
