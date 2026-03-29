using UnityEngine;
using UnityEngine.UI;

public class BaseHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public BaseHealth baseHealth;

    void Update()
    {
        if (baseHealth != null && healthSlider != null)
        {
            healthSlider.value = baseHealth.currentHealth;
        }
    }
}
