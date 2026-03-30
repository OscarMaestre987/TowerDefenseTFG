using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;          // armas en el player
    public WeaponData[] weaponData;       // stats de cada arma

    private int[] currentAmmoPerWeapon;
    private int[] totalAmmoPerWeapon;

    private int currentIndex = 0;

    private PlayerShooting playerShooting;

    void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        currentAmmoPerWeapon = new int[weaponData.Length];
        totalAmmoPerWeapon = new int[weaponData.Length];
        for (int i = 0; i < weaponData.Length; i++)
        {
            currentAmmoPerWeapon[i] = weaponData[i].maxAmmo;
            totalAmmoPerWeapon[i] = weaponData[i].totalAmmo;
        }
        playerShooting.InitializeWeapons(currentAmmoPerWeapon, totalAmmoPerWeapon);
        SelectWeapon(0);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentIndex++;
            if (currentIndex >= weapons.Length)
                currentIndex = 0;

            SelectWeapon(currentIndex);
        }
        else if (scroll < 0f)
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = weapons.Length - 1;

            SelectWeapon(currentIndex);
        }
    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        playerShooting.ApplyWeapon(weaponData[index], index);
    }
}