using TMPro;
using UnityEngine;

public class WeaponShopZone : MonoBehaviour
{
    public int ammoBaseCost = 100;
    public int upgradeBaseCost = 100;
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
                buyPromptText.text = $"Presiona F para comprar munición ({ammoBaseCost}G) \n Presiona T para mejorar arma ({upgradeBaseCost}G)";
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
        if (playerInside && Input.GetKeyDown(KeyCode.T))
        {
            TryUpgrade();
        }
    }

    void TryUpgrade()
    {
        if (playerGold != null && playerGold.Gold >= upgradeBaseCost)
        {
            playerGold.RemoveGold(upgradeBaseCost);
            player.GetComponent<PlayerShooting>().UpgradeWeapon(index);
            upgradeBaseCost = upgradeBaseCost * 2;
            ammoBaseCost = ammoBaseCost + 500;
            if (buyPromptText != null)
                buyPromptText.text = $"Presiona F para comprar munición ({ammoBaseCost}G) \n Presiona T para mejorar arma ({upgradeBaseCost}G)";
        }
        else
        {
            // Opcional: mostrar que no hay oro
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({upgradeBaseCost}G)";
        }
    }

    void TryPurchase()
    {
        if (playerGold != null && playerGold.Gold >= ammoBaseCost)
        {
            playerGold.RemoveGold(ammoBaseCost);
            player.GetComponent<PlayerShooting>().ResetAmmo(index);

        }
        else
        {
            // Opcional: mostrar que no hay oro
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({ammoBaseCost}G)";
        }
    }
}

