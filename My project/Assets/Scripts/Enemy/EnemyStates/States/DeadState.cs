using UnityEngine;

public class DeadState : EnemyState
{
    public DeadState(EnemyStateMachine stateMachine, EnemyAI enemy, EnemyAnimationsController enemyAnimation ) : base(stateMachine, enemy, enemyAnimation) { }

    public override void Enter()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerGold score = player.GetComponent<PlayerGold>();
            if (score != null)
            {
                score.AddGold(enemy.goldGain);
            }
            if (enemyAnimation != null)
            {
                enemyAnimation.SetMoving(false);
                enemyAnimation.SetAttack(false);
                enemyAnimation.Die();
            }
        }

        enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        enemy.GetComponent<Collider>().enabled = false;

        GameObject.Destroy(enemy.gameObject, 0f);

        if (enemy.deathEffectPrefab != null)
        {
            GameObject vfx = GameObject.Instantiate(enemy.deathEffectPrefab, enemy.transform.position, Quaternion.identity);
            GameObject.Destroy(vfx, 2f); // destruye el efecto tras 2 segundos
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.enemyDeathClip);
        }

    }
    public override void Update() { }

    public override void Exit() { }
}
