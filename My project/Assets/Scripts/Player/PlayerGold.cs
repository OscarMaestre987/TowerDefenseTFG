using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerGold : MonoBehaviour
{
    public int Gold { get; private set; } = 0;

    public void RemoveGold(int amount)
    {
        Gold -= amount;
        if (Gold < 0) Gold = 0;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        Debug.Log("Gold actual: " + Gold);
    }
}
