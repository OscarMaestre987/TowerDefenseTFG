using UnityEngine;
using UnityEngine.AI;

public class GoToBaseState : EnemyState
{
    private NavMeshAgent agent;

    public GoToBaseState(EnemyStateMachine stateMachine, EnemyAI enemy, EnemyAnimationsController enemyAnimation) : base(stateMachine, enemy, enemyAnimation) { }

    public override void Enter()
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = enemy.moveSpeed; // ← aquí aplicamos la velocidad
        agent.isStopped = false;
    }

    public override void Update()
    {
        if (enemy.baseTarget == null) return;

        // Avanzar hacia adelante (en Z) localmente
        Vector3 backward = enemy.transform.forward;
        Vector3 nextPosition = enemy.transform.position + backward * agent.speed * Time.deltaTime;
        agent.Move(backward * agent.speed * Time.deltaTime);

        // Medir la distancia entre colliders o entre centros
        float distanceToBase = enemy.transform.position.z - enemy.baseTarget.position.z;
        if (distanceToBase <= enemy.attackRange + 0.5f)
        {
            if (enemyAnimation != null)
            {
                enemyAnimation.SetMoving(false);
                enemyAnimation.SetAttack(true);
            }
            stateMachine.ChangeState(new AttackState(stateMachine, enemy, enemyAnimation));
        }
    }

    public override void Exit()
    {
        agent.isStopped = true;
    }
}
