using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject backToMainMenu;

    public void SwitchBackToMainMenu()
    {
        GameManager.Instance.stateMechine.SetState("GameInitState");
        backToMainMenu.SetActive(true);
        Utils.LoadScene("Main");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject)
        {
            SwitchBackToMainMenu();
        }
    }
}