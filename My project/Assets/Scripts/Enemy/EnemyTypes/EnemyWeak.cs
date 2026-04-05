using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeak : EnemyAI
{
    void Awake()
    {
        moveSpeed = 4f;
        maxHealth = 50;
        damage = 5;
        goldGain = 100;
        aggroThreshold = 0.8f;
        attackSpeed = 1f;
        attackRange = 1.5f;
    }
}
