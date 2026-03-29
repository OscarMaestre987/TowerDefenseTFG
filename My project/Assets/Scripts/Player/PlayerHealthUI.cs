using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth player;

    void Update()
    {
        if (player != null && healthSlider != null)
        {
            healthSlider.value = player.currentHealth;
        }
    }
}
