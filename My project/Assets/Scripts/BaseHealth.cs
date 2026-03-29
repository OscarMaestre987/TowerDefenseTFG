using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;

    public BaseHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Base recibe daño. Vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("¡La base ha sido destruida!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
