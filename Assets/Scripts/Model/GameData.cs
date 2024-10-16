using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentHealth;

    public GameData(GameManager gameManager)
    {
        currentHealth = gameManager.currentHealth;
    }

}
