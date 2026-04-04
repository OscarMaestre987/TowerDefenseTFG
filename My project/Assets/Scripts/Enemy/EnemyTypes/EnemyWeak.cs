using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeak : EnemyAI
{
    void Awake()
    {
        moveSpeed = 4f;
        maxHealth = 50;
        damage = 20;
        goldGain = 100;
        aggroThreshold = 0.8f;
    }
}
