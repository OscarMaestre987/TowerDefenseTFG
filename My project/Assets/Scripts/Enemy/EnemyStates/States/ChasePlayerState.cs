using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerState : EnemyState
{
    private NavMeshAgent agent;
    public ChasePlayerState(EnemyStateMachine stateMachine, EnemyAI enemy, EnemyAnimationsController enemyAnimation) : base(stateMachine, enemy, enemyAnimation) { }

    public override void Enter()
    {
        Debug.Log("Entra en estado: Perseguir jugador");

        agent = enemy.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.updateUpAxis = true;
        agent.speed = enemy.moveSpeed; // ← aplicar velocidad personalizada
        agent.SetDestination(enemy.playerTarget.position);
    }

    public override void Update()
    {
        if (enemy.playerTarget == null) return;

        Vector3 dir = (enemy.playerTarget.position - enemy.transform.position).normalized;
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.SetDestination(enemy.playerTarget.position);

        float distance = Vector3.Distance(enemy.transform.position, enemy.currentTarget.position);
        if (distance < enemy.attackRange + 0.5f)
        {
            // estás cerca de la base
            if (enemyAnimation != null)
            {
                enemyAnimation.SetMoving(false);
                enemyAnimation.SetAttack(true);
            }
            stateMachine.ChangeState(new AttackState(stateMachine, enemy, enemyAnimation));
        }
    }

    public override void Exit() { }
}
