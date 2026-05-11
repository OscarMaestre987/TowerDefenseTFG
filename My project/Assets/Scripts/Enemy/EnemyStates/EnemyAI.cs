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
    private float originalSpeed;
    private float currentSlow = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        playerTarget = GameObject.FindWithTag("Player")?.transform;
        baseTarget = GameObject.FindWithTag("Base")?.transform;
        currentTarget = baseTarget;
        stateMachine = new EnemyStateMachine();
        enemyAnimation = GetComponent<EnemyAnimationsController>();
        originalSpeed = moveSpeed;
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

    public void ApplySlow(float amount)
    {
        currentSlow += amount;
        UpdateSpeed();
    }

    public void RemoveSlow(float amount)
    {
        currentSlow -= amount;
        currentSlow = Mathf.Max(1, currentSlow);
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        moveSpeed = originalSpeed / currentSlow;
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

        // Comprueba si el enemigo debe cambiar a estado agresivo (perseguir al jugador)
        else if (
            // El enemigo tiene la vida por debajo del umbral definido (estado "aggro")
            currentHealth <= maxHealth * aggroThreshold &&

            // Evita cambiar de estado si ya está persiguiendo al jugador
            !(stateMachine.currentState is ChasePlayerState) &&

            // Solo se activa si el daño proviene del jugador (no de torres u otros)
            source.CompareTag("Player")
        )
        {
            // Establece al jugador como nuevo objetivo
            currentTarget = playerTarget;

            // Cambia el estado de la IA a persecución del jugador
            stateMachine.ChangeState(new ChasePlayerState(stateMachine, this, enemyAnimation));
        }
    }
}
