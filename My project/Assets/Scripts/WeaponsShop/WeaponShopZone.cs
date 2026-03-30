using TMPro;
using UnityEngine;

public class WeaponShopZone : MonoBehaviour
{
    public int ammoCost = 100;
    public TextMeshProUGUI buyPromptText;
    public int index = 0;

    private bool playerInside = false;
    private PlayerGold playerGold;
    private Collider player;

    void Start()
    {
        if (buyPromptText != null)
            buyPromptText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other;
            playerInside = true;
            playerGold = other.GetComponent<PlayerGold>();

            if (buyPromptText != null)
                buyPromptText.text = $"Presiona F para comprar munición ({ammoCost}G)";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerGold = null;

            if (buyPromptText != null)
                buyPromptText.text = "";
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            TryPurchase();
        }
    }

    void TryPurchase()
    {
        if (playerGold != null && playerGold.Gold >= ammoCost)
        {
            playerGold.RemoveGold(ammoCost);
            player.GetComponent<PlayerShooting>().ResetAmmo(index);

        }
        else
        {
            // Opcional: mostrar que no hay oro
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({ammoCost}G)";
        }
    }
}

