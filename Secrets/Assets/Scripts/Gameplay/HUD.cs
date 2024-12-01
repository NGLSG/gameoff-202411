using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : Singleton<HUD>
{
    [SerializeField] private UISlider exploreV;
    [SerializeField] private UISlider personalV;
    [SerializeField] private UISlider normalV;

    [SerializeField] public int exploreValueMax;
    [SerializeField] public int personalValueMax;
    [SerializeField] public int normalValueMax;
   public void SetExploreValue(int value)
    {
        exploreV.SetRate((float)value/exploreValueMax);
    }

    public void SetPersonalValue(int value)
    {
        personalV.SetRate((float)value/personalValueMax);
    }

    public void SetNormalValue(int value)
    {
        normalV.SetRate((float)value/normalValueMax);
    }
}
