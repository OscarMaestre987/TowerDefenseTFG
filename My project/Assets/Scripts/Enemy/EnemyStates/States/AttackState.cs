using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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
    }

    public override void Update()
    {
        if (enemy.currentTarget == null) return;
        float distance = 0f; ;
        if (enemy.currentTarget == enemy.baseTarget)
        {
            distance = enemy.transform.position.z - enemy.baseTarget.position.z;
        }
        if (enemy.currentTarget == enemy.playerTarget)
        {
            distance = Vector3.Distance(enemy.transform.position, enemy.currentTarget.position);
        }
        Debug.Log($"distance: {distance}");
        // 🔥 esperar cooldown
        if (distance <= enemy.attackRange + 0.5f)
        {
            if (Time.time - lastAttackTime >= enemy.attackSpeed)
            {
                DealDamage();
                lastAttackTime = Time.time;
            }
            // comprobar distancia  Time.time - lastAttackTime >= enemy.attackSpeed
        }
        if (distance > enemy.attackRange + 0.5f)
        {
            agent.isStopped = false;
            enemyAnimation.SetMoving(true);
            enemyAnimation.SetAttack(false);

            if (enemy.currentHealth <= (enemy.maxHealth * enemy.aggroThreshold))
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
