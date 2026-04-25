using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float initialHealth;
    public float currentHealth;

    public float armorPercentage;

    public float normalSpeed;
    public float accelerateSpeed;
    public float moveSpeed;

    public float initialEnergy;
    public float currentEnergy;

    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public float playerRotationX;
    public float playerRotationY;
    public float playerRotationZ;

    public Dictionary<string, int> statLevels;
    public List<int> unlockedSkills;
    public List<int> unlockedTanks;
    public int playerCoins;
    public int selectedTankId;
    public GameData()
    {
        initialHealth = 50f;
        currentHealth = initialHealth;
        armorPercentage = 0f;
        initialEnergy = 20f;
        currentEnergy = initialEnergy;
        normalSpeed = 2f;
        moveSpeed = normalSpeed;
        accelerateSpeed = 3f;
        playerPosX = playerPosY = playerPosZ = 0f;
        playerRotationX = playerRotationY = playerRotationZ = 0f;

        statLevels = new Dictionary<string, int>();
        unlockedSkills = new List<int>();
        unlockedTanks = new List<int>();
        playerCoins = 10000;
        selectedTankId = 0;
    }


}
