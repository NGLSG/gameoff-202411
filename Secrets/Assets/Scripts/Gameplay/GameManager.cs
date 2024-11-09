using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameData gameData;

    private void Start()
    {
        gameData = new GameData();
    }

    public GameData GetGameData()
    {
        return gameData;
    }
}