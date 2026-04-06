using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float initialHealth;
    public float currentHealth;
    public float armorPercentage;
    public float moveSpeed;
    public Vector3 playerPos;
    public Vector3 playerRotation;

    public GameData()
    {
        initialHealth = 50;
        currentHealth = initialHealth;
        armorPercentage = 0;
        moveSpeed = 2.5f;
        playerPos = Vector3.zero;
        playerRotation = Vector3.zero;
    }


}
