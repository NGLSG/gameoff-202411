using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public StateManager stateMechine;
    private GameData gameData;

    private void Awake()
    {
        gameData = new GameData();
        stateMechine = new StateManager();
        stateMechine.SetState("GameInitState");
        
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, "Resources/Clues")))
        {
            Directory.Delete(Path.Combine(Application.persistentDataPath, "Resources/Clues"), true);
        }
    }

    private void Start()
    {
        stateMechine.SwitchToNextState();
    }

    public GameData GetGameData()
    {
        if (gameData == null)
        {
            gameData = new GameData();
        }

        return gameData;
    }

    private void Update()
    {
        stateMechine.StateUpdate();
    }
}