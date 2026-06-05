using TMPro;
using UnityEngine;

public class TurretPurchaseZone : MonoBehaviour
{
    [System.Serializable]
    public class TowerOption
    {
        public string towerName;
        public GameObject[] turretLevels;
        public int[] turretCosts;
    }

    public TowerOption[] towerOptions;

    public Transform spawnPoint;
    public TextMeshProUGUI buyPromptText;

    [Header("UI Slots")]
    public GameObject towerSelectionPanel;
    public GameObject[] selectionBorders;

    private bool playerInside = false;
    private PlayerGold playerGold;

    private GameObject currentTurret;
    private int currentLevel = -1;
    private int selectedTowerIndex = 0;
    private int boughtTowerIndex = -1;

    void Start()
    {
        if (buyPromptText != null)
            buyPromptText.text = "";

        if (towerSelectionPanel != null)
            towerSelectionPanel.SetActive(false);

        UpdateSelectionUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerGold = other.GetComponent<PlayerGold>();

            if (towerSelectionPanel != null)
                towerSelectionPanel.SetActive(currentLevel == -1);

            UpdateSelectionUI();
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

            if (towerSelectionPanel != null)
                towerSelectionPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerInside || towerOptions.Length == 0)
            return;

        if (currentLevel == -1)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
                ChangeSelectedTower(-1);
            else if (scroll < 0f)
                ChangeSelectedTower(1);
        }


        if (Input.GetKeyDown(KeyCode.F))
            TryBuyOrUpgrade();
    }

    void ChangeSelectedTower(int direction)
    {
        selectedTowerIndex += direction;

        if (selectedTowerIndex < 0)
            selectedTowerIndex = towerOptions.Length - 1;
        else if (selectedTowerIndex >= towerOptions.Length)
            selectedTowerIndex = 0;

        UpdateSelectionUI();
        UpdateText();
    }

    void TryBuyOrUpgrade()
    {
        TowerOption selectedTower = towerOptions[selectedTowerIndex];

        if (currentLevel == -1)
        {
            BuyTower(selectedTower);
            return;
        }

        if (boughtTowerIndex != selectedTowerIndex)
        {
            if (buyPromptText != null)
                buyPromptText.text = "Ya hay una torre comprada aquí";
            return;
        }

        UpgradeTower(selectedTower);
    }

    void BuyTower(TowerOption selectedTower)
    {
        if (selectedTower.turretLevels.Length == 0)
            return;

        int cost = selectedTower.turretCosts[0];

        if (playerGold != null && playerGold.Gold >= cost)
        {
            playerGold.RemoveGold(cost);

            currentTurret = Instantiate(
                selectedTower.turretLevels[0],
                spawnPoint.position + Vector3.up * 0.25f,
                spawnPoint.rotation
            );

            currentLevel = 0;
            boughtTowerIndex = selectedTowerIndex;
            if (towerSelectionPanel != null)
                towerSelectionPanel.SetActive(false);
            UpdateText();
        }
        else
        {
            if (buyPromptText != null)
                buyPromptText.text = $"No tienes suficiente oro ({cost}G)";
        }
    }

    void UpgradeTower(TowerOption selectedTower)
    {
        int nextLevel = currentLevel + 1;

        if (nextLevel >= selectedTower.turretLevels.Length)
        {
            if (buyPromptText != null)
                buyPromptText.text = "Nivel máximo alcanzado";
            return;
        }

        int cost = selectedTower.turretCosts[nextLevel];

        if (playerGold != null && playerGold.Gold >= cost)
        {
            playerGold.RemoveGold(cost);

            if (currentTurret != null)
                Destroy(currentTurret);

            currentTurret = Instantiate(
                selectedTower.turretLevels[nextLevel],
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

    void UpdateSelectionUI()
    {
        for (int i = 0; i < selectionBorders.Length; i++)
        {
            if (selectionBorders[i] != null)
                selectionBorders[i].SetActive(i == selectedTowerIndex);
        }
    }

    void UpdateText()
    {
        if (buyPromptText == null || towerOptions.Length == 0)
            return;

        TowerOption selectedTower = towerOptions[selectedTowerIndex];

        if (currentLevel == -1)
        {
            buyPromptText.text =
                $"F + ({selectedTower.turretCosts[0]}G) = {selectedTower.towerName}";
        }
        else if (boughtTowerIndex != selectedTowerIndex)
        {
            buyPromptText.text = "Ya hay una torre comprada aquí";
        }
        else if (currentLevel < selectedTower.turretLevels.Length - 1)
        {
            buyPromptText.text =
                $"F + ({selectedTower.turretCosts[currentLevel + 1]}G) = Mejorar {selectedTower.towerName}";
        }
        else
        {
            buyPromptText.text = $"{selectedTower.towerName} al máximo";
        }
    }
}