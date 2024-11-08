using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
