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
    public Vector3 playerPos;
    public Vector3 playerRotation;
    public Dictionary<string, int> statLevels;
    public List<int> unlockedSkills;
    public List<int> unlockedTanks;
    public int playerCoins;
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
        playerPos = Vector3.zero;
        playerRotation = Vector3.zero;

        statLevels = new Dictionary<string, int>();
        unlockedSkills = new List<int>();
        unlockedTanks = new List<int>();
        playerCoins = 0;
    }


}
