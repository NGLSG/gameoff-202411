using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public StateManager stateMechine;
    private GameData gameData;

    private void Start()
    {
        gameData = new GameData();
        stateMechine = new StateManager();
        stateMechine.SetState("GameInitState");
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    private void Update()
    {
        stateMechine.StateUpdate();
    }
}