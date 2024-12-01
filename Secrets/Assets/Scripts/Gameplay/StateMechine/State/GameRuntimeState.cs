using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuntimeState : BaseState
{
    public bool isCharactersSet;
    public GameRuntimeState(StateManager mgr) : base(mgr) { }

    public override void StateEnter()
    {
        base.StateEnter();
        isCharactersSet = false;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        
        GameTimeManager.FactualTime targetTime = new GameTimeManager.FactualTime();
        GameTimeManager.FactualTime PMTime = new GameTimeManager.FactualTime();
        targetTime.SetTime(24, 0, 0); // 设置为 12:00:00
        PMTime.SetTime(12, 0, 0); 

        if (GameTimeManager.Instance.IsCurrentTimeGreaterThan(PMTime) && !isCharactersSet)
        {
            isCharactersSet = true;
            GameObject Herbert = GameObject.Find("Herbert");
            GameObject Simon = GameObject.Find("Simon");
            GameObject Hedgehog = GameObject.Find("Hedgehog");
            GameObject Quintina = GameObject.Find("Quintina");
            
            //赫伯特 1. 上午图书馆：toiletHerbert（对话） 2. 下午小花园：libraryHerbertQuintina（偷听）
            Herbert.GetComponent<NPC>().eavesdroppingNode = "libraryHerbertQuintina";
            Herbert.GetComponent<NPC>().npcDialogueNode = "EmptyDialogue";
            Herbert.transform.localPosition = new Vector3(-13, -12, 0);
            
            Quintina.GetComponent<NPC>().eavesdroppingNode = "libraryHerbertQuintina";
            Quintina.GetComponent<NPC>().npcDialogueNode = "EmptyDialogue";

            //Simon 1. 上午教室：libraryWMS（偷听） 2. 下午废弃厕所：toiletSimon（对话）
            Simon.GetComponent<NPC>().eavesdroppingNode = "EmptySpyHearing";
            Simon.GetComponent<NPC>().npcDialogueNode = "toiletSimon";
            Simon.transform.localPosition = new Vector3(2, 12, 0);

            //Hedgehog 1. 上午体育场角落：Yankee_MeetAM（对话） 2. 下午体育器材室：Yankee_MeetPM（对话）
            Hedgehog.GetComponent<NPC>().eavesdroppingNode = "EmptySpyHearing";
            Hedgehog.GetComponent<NPC>().npcDialogueNode = "Yankee_MeetPM";
            Hedgehog.transform.localPosition = new Vector3(10, -12, 0);


        } else if (GameTimeManager.Instance.IsCurrentTimeGreaterThan(targetTime))
        {
            GameManager.Instance.stateMechine.SetState("GameFinishState");
        }
        else
        {
            
        }
    }

    public override void StateExit()
    {
        base.StateExit();
    }
}