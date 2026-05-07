using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    public float initialHealth;
    public float currentHealth;

    public float armorPercentage;

    public float initialSpeed;
    public float accelerateSpeed;
    public float moveSpeed;

    public float initialEnergy;
    public float currentEnergy;

    public float dmgMult;

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
        initialSpeed = 2f;
        moveSpeed = initialSpeed;
        accelerateSpeed = 3f;
        dmgMult = 1f;
        playerPosX = playerPosY = playerPosZ = 0f;
        playerRotationX = playerRotationY = playerRotationZ = 0f;

        statLevels = new Dictionary<string, int>();
        unlockedSkills = new List<int>();
        unlockedTanks = new List<int>();
        playerCoins = 10000;
        selectedTankId = 0;
    }


}
