using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeak : EnemyAI
{
    void Awake()
    {
        moveSpeed = 2.5f;
        maxHealth = 50;
        damage = 20;
        goldGain = 100;
    }
}
