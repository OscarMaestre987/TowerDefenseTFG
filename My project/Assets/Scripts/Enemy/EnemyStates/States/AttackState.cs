using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyState
{
    private NavMeshAgent agent;
    private float lastAttackTime;
    float distanceToBase;

    public AttackState(EnemyStateMachine stateMachine, EnemyAI enemy, EnemyAnimationsController enemyAnimation) : base(stateMachine, enemy, enemyAnimation) { }

    public override void Enter()
    {
        Debug.Log("Entra en estado: Atacar");

        agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = true; // 🔒 Detiene al enemigo al entrar
        enemyAnimation.SetMoving(false);
        enemyAnimation.SetAttack(true);
        DealDamage();
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        if (enemy.currentTarget == null) return;

        // 🔥 esperar cooldown
        if (Time.time - lastAttackTime >= enemy.attackSpeed)
        {
            // comprobar distancia
            float distance = Vector3.Distance(enemy.transform.position, enemy.currentTarget.position);

            if (distance <= enemy.attackRange)
            {
                stateMachine.ChangeState(new AttackState(stateMachine, enemy, enemyAnimation));
                return;
            }
        }

        // 🔁 si se aleja → perseguir
        float distanceToTarget = Vector3.Distance(enemy.transform.position, enemy.currentTarget.position);

        if (distanceToTarget > enemy.attackRange + 0.5f)
        {
            agent.isStopped = false;
            enemyAnimation.SetMoving(true);
            enemyAnimation.SetAttack(false);

            if (enemy.currentHealth <= enemy.aggroThreshold)
                stateMachine.ChangeState(new ChasePlayerState(stateMachine, enemy, enemyAnimation));
            else
                stateMachine.ChangeState(new GoToBaseState(stateMachine, enemy, enemyAnimation));
        }
    }

    void DealDamage()
    {
        if (enemy.currentTarget.CompareTag("Player"))
        {
            PlayerHealth player = enemy.currentTarget.GetComponent<PlayerHealth>();
            if (player != null)
                player.TakeDamage(enemy.damage);
        }
        else if (enemy.currentTarget.CompareTag("Base"))
        {
            BaseHealth baseHealth = enemy.currentTarget.GetComponent<BaseHealth>();
            if (baseHealth != null)
                baseHealth.TakeDamage(enemy.damage);
        }

        Debug.Log($"{enemy.name} ataca a {enemy.currentTarget.name}");
    }

    public override void Exit()
    {
        agent.isStopped = false; // Por seguridad
    }
}
