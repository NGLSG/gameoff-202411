using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject backToMainMenu;
    public void SwitchBackToMainMenu()
    {
        GameManager.Instance.stateMechine.SetState("GameInitState");
        backToMainMenu.SetActive(true);
        Utils.LoadScene("Main");
    }
}
