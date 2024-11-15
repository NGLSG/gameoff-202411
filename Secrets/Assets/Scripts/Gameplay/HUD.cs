using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : Singleton<HUD>
{
    [SerializeField] private TextMeshProUGUI exploreV;
    [SerializeField] private TextMeshProUGUI personalV;
    [SerializeField] private TextMeshProUGUI normalV;

   public void SetExploreValue(int value)
    {
        exploreV.text = value.ToString();
    }

    public void SetPersonalValue(int value)
    {
        personalV.text = value.ToString();
    }

    public void SetNormalValue(int value)
    {
        normalV.text = value.ToString();
    }
}
