using UnityEngine;

public class EnemyAnimationsController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.Log("Animator not get");
    }

    public void SetMoving(bool moving)
    {
        animator.SetBool("IsMoving", moving);
    }

    public void SetAttack(bool atacking)
    {
        animator.SetBool("IsAtacking", atacking);
    }

    public void Die()
    {
        animator.SetBool("IsDying", true);
    }
}