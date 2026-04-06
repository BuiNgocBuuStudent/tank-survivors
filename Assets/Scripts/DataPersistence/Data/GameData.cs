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

    public GameData()
    {
        initialHealth = 50f;
        currentHealth = initialHealth;
        armorPercentage = 0f;
        initialEnergy = 20f;
        currentEnergy = initialEnergy;
        normalSpeed = 2f;
        moveSpeed = normalSpeed;
        accelerateSpeed = 4f;
        playerPos = Vector3.zero;
        playerRotation = Vector3.zero;
    }


}
