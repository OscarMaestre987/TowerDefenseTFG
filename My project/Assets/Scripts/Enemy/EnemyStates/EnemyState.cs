using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected EnemyAI enemy;
    protected EnemyAnimationsController enemyAnimation;

    public EnemyState(EnemyStateMachine stateMachine, EnemyAI enemy, EnemyAnimationsController enemyAnimation)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.enemyAnimation = enemyAnimation;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
