using TMPro;
using UnityEngine;

public class PlayerGoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public PlayerGold playerGold;

    void Update()
    {
        if (playerGold != null && goldText != null)
        {
            goldText.text = "Gold: " + playerGold.Gold;
        }
    }
}
