using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : EnemyAI
{
    void Awake()
    {
        moveSpeed = 1.5f;
        maxHealth = 100;
        damage = 20;
        goldGain = 300;
    }
}
