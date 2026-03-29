using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState;

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
