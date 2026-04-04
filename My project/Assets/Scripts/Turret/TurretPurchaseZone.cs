using TMPro;
using UnityEngine;

public class TurretPurchaseZone : MonoBehaviour
{
    public GameObject[] turretLevels;
    public int[] turretCosts;

    public Transform spawnPoint;
    public TextMeshProUGUI buyPromptText;

    private bool playerInside = false;
    private PlayerGold playerGold;

    private GameObject currentTurret;
    private int currentLevel = -1;

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
            UpdateText();
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
            TryBuyOrUpgrade();
        }
    }

    void TryBuyOrUpgrade()
    {
        int nextLevel = currentLevel + 1;

        // 🔥 si ya está al máximo → no hacer nada
        if (nextLevel >= turretLevels.Length)
        {
            if (buyPromptText != null)
                buyPromptText.text = "Nivel máximo alcanzado";
            return;
        }

        int cost = turretCosts[nextLevel];

        if (playerGold != null && playerGold.Gold >= cost)
        {
            playerGold.RemoveGold(cost);

            // 🔥 destruir torreta anterior
            if (currentTurret != null)
                Destroy(currentTurret);

            // 🔥 instanciar nueva
            currentTurret = Instantiate(
                turretLevels[nextLevel],
                spawnPoint.position,
                spawnPoint.rotation
            );

            currentLevel = nextLevel;

            UpdateText();
        }
        else
        {
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({cost}G)";
        }
    }

    void UpdateText()
    {
        if (buyPromptText == null) return;

        if (currentLevel == -1)
        {
            buyPromptText.text = $"F: Comprar torreta ({turretCosts[0]}G)";
        }
        else if (currentLevel < turretLevels.Length - 1)
        {
            buyPromptText.text = $"F: Mejorar torreta ({turretCosts[currentLevel + 1]}G)";
        }
        else
        {
            buyPromptText.text = "Torreta al máximo";
        }
    }
}