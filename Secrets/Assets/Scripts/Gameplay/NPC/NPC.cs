using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("Video Config")] 
    [SerializeField] private AudioClip eavesdroppingAudio;
    [SerializeField] private string dialogueNode;

    [Header("Map Tag Config")] 
    [SerializeField] private Image npcHeadPhoto;
    [SerializeField] private TextMeshProUGUI npcName;

    [Header("Body Config")] 
    [SerializeField] private string body;
    [SerializeField] private string hair;
    [SerializeField] private string leftArm;
    [SerializeField] private string rightArm;
    
    // other state that controll the conversation branch

    private void Start()
    {
        //TODO: Generate Body Config
        //TODO: Generate Map_Tag Config
        //TODO: Generate Video Config
    }

    private void Update()
    {
        //TODO: sense Player in a big circle
        // 1. start play audioClip;
        //TODO: sense Player in a small circle
    }

    public void Chatting()
    {
        //TODO: Player use this to chat with NPC
    }

    public void StartEavesDropping()
    {
        //TODO: Playe Audio Clip
    }
}
