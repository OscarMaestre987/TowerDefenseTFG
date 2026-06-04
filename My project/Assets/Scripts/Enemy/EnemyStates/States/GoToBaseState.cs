using UnityEngine;
using UnityEngine.AI;

public class GoToBaseState : EnemyState
{
    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private float randomPointDistance;

    public GoToBaseState(
        EnemyStateMachine stateMachine,
        EnemyAI enemy,
        EnemyAnimationsController enemyAnimation
    ) : base(stateMachine, enemy, enemyAnimation) { }

    public override void Enter()
    {
        agent = enemy.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = enemy.moveSpeed;
        agent.isStopped = false;
        randomPointDistance = Random.Range(0f, 25f);
        currentPointIndex = 0;

        if (enemy.pathPoints != null && enemy.pathPoints.Length > 0)
        {
            agent.SetDestination(
                 enemy.pathPoints[currentPointIndex].transform.position
             );
        }

        if (enemyAnimation != null)
        {
            enemyAnimation.SetMoving(true);
            enemyAnimation.SetAttack(false);
        }
    }

    public override void Update()
    {
        if (enemy.baseTarget == null)
            return;

        agent.speed = enemy.moveSpeed;

        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            enemy.transform.forward = agent.velocity.normalized;
        }

        if (enemy.pathPoints != null && currentPointIndex < enemy.pathPoints.Length)
        {
            MoveThroughPoints();
        }
        else
        {
            MoveForwardToBase();
        }
 
        CheckBaseDistance();
    }

    private void MoveThroughPoints()
    {
        if (enemy.pathPoints == null || currentPointIndex >= enemy.pathPoints.Length)
            return;

        float distanceToPoint = Vector3.Distance(
            enemy.transform.position,
            enemy.pathPoints[currentPointIndex].transform.position
        );

        if (distanceToPoint <= randomPointDistance)
        {
            currentPointIndex++;

            if (currentPointIndex < enemy.pathPoints.Length)
            {
                randomPointDistance = Random.Range(0f, 25f);

                agent.SetDestination(
                    enemy.pathPoints[currentPointIndex].transform.position
                );
            }
            else
            {
                agent.ResetPath();
            }
        }
    }

    private void MoveForwardToBase()
    {
        Vector3 direction = new Vector3(0f, 0f, -1f);
        agent.Move(direction * agent.speed * Time.deltaTime);
        enemy.transform.forward = direction;
        if (enemy.aggroThreshold >= 1)
            stateMachine.ChangeState(new ChasePlayerState(stateMachine, enemy, enemyAnimation));
    }

    private void CheckBaseDistance()
    {
        float distanceToBase = enemy.transform.position.z - enemy.baseTarget.position.z;

        if (distanceToBase <= enemy.attackRange + 0.5f)
        {
            if (enemyAnimation != null)
            {
                enemyAnimation.SetMoving(false);
                enemyAnimation.SetAttack(true);
            }

            stateMachine.ChangeState(
                new AttackState(stateMachine, enemy, enemyAnimation)
            );
        }
    }

    public override void Exit()
    {
        if (agent != null)
            agent.isStopped = true;
    }
}