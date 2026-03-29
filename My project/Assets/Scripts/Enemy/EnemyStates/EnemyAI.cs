using System.Xml;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int damage = 10;
    public float moveSpeed = 3f;
    public float aggroThreshold = 0.8f;
    public float attackRange = 1.5f;
    public float attackSpeed = 1f;
    public int goldGain = 500;

    public int currentHealth;

    [Header("Efectos visuales")]
    public GameObject hitEffectPrefab;
    public GameObject deathEffectPrefab;

    [HideInInspector] public Transform playerTarget;
    [HideInInspector] public Transform baseTarget;
    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public EnemyAnimationsController enemyAnimation;


    void Start()
    {
        currentHealth = maxHealth;
        playerTarget = GameObject.FindWithTag("Player")?.transform;
        baseTarget = GameObject.FindWithTag("Base")?.transform;
        currentTarget = baseTarget;
        stateMachine = new EnemyStateMachine();
        enemyAnimation = GetComponent<EnemyAnimationsController>();
        if (enemyAnimation == null)
        {
            Debug.Log("enemyAnimator no encontrado");
        }
        stateMachine.ChangeState(new GoToBaseState(stateMachine, this, enemyAnimation));
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void TakeDamage(int amount, GameObject source)
    {
        currentHealth -= amount;
        if (hitEffectPrefab != null)
        {
            GameObject vfx = Instantiate(hitEffectPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
            Destroy(vfx, vfx.GetComponent<ParticleSystem>().main.duration + 0.5f);

        }
        if (currentHealth <= 0)
        {
            stateMachine.ChangeState(new DeadState(stateMachine, this, enemyAnimation));
        }
        else if (
            currentHealth <= maxHealth * aggroThreshold &&
            !(stateMachine.currentState is ChasePlayerState) &&
            source.CompareTag("Player") // ✅ Solo si fue el jugador
        )
        {
            currentTarget = playerTarget;
            stateMachine.ChangeState(new ChasePlayerState(stateMachine, this, enemyAnimation));
        }
    }
}
