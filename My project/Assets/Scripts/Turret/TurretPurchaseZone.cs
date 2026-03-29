using TMPro;
using UnityEngine;

public class TurretPurchaseZone : MonoBehaviour
{
    public GameObject turretPrefab;
    public Transform spawnPoint;
    public int turretCost = 100;
    public TextMeshProUGUI buyPromptText;

    private bool playerInside = false;
    private PlayerGold playerGold;

    void Start()
    {
        if (buyPromptText != null)
            buyPromptText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerGold = other.GetComponent<PlayerGold>();

            if (buyPromptText != null)
                buyPromptText.text = $"Presiona F para comprar torreta ({turretCost}G)";
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
        if (playerGold != null && playerGold.Gold >= turretCost)
        {
            playerGold.RemoveGold(turretCost);

            Instantiate(turretPrefab, spawnPoint.position, spawnPoint.rotation);

            if (buyPromptText != null)
                buyPromptText.text = "";

            gameObject.SetActive(false);
        }
        else
        {
            // Opcional: mostrar que no hay oro
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({turretCost}G)";
        }
    }
}

