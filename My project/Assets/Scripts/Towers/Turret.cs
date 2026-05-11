using UnityEngine;

public class Turret : MonoBehaviour
{
    public float detectionRadius = 15f;
    public float fireRate = 1f;
    public float buletSpeed = 50f;
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public LayerMask enemyMask;

    private float lastFireTime = 0f;
    private Transform currentTarget;

    void Update()
    {
        FindClosestEnemy();

        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            direction.y = 0f;
            // Girar suavemente hacia el objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 50f);

            if (Time.time - lastFireTime >= fireRate)
            {
                Fire();
                lastFireTime = Time.time;
            }
        }
    }

    void FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyMask);

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }

        currentTarget = closest;
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = gunPoint.forward * buletSpeed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
