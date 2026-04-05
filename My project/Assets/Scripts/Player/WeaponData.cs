using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Stats")]
    public int damage;
    public float fireRate;
    public float fireRange;
    public int maxAmmo;
    public int totalAmmo;
    public int currentAmmo;
    public int damageUpgrade;
}