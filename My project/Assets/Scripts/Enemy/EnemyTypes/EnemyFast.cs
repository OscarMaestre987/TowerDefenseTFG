using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : EnemyAI
{
    void Awake()
    {
        moveSpeed = 7f;
        maxHealth = 25;
        damage = 5;
        goldGain = 200;
        aggroThreshold = 0.8f;
        attackSpeed = 0.5f;
        attackRange = 1.5f;
    }
}
