using System;
using UnityEngine;

[Serializable]
public class SavedData
{
    public int currentHealth;
    public int amountOfKills;
    public Vector3 playerPosition;
    public int amountOfEnemies;
    public Vector3[] enemiesPositions;
}