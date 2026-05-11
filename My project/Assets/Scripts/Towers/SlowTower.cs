using System.Collections.Generic;
using UnityEngine;

public class SlowTower : MonoBehaviour
{
    public float range = 5f;
    public float slowAmount = 1f;

    private List<EnemyAI> enemiesInRange = new List<EnemyAI>();

    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        List<EnemyAI> currentEnemies = new List<EnemyAI>();

        foreach (var hit in hits)
        {
            EnemyAI enemy = hit.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                currentEnemies.Add(enemy);

                if (!enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                    enemy.ApplySlow(slowAmount);
                }
            }
        }

        // Detectar enemigos que salieron del rango
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            if (!currentEnemies.Contains(enemiesInRange[i]))
            {
                enemiesInRange[i].RemoveSlow(slowAmount);
                enemiesInRange.RemoveAt(i);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}