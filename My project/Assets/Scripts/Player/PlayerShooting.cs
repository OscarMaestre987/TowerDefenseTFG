using UnityEngine;
using System.Collections;
using TMPro;
using System.Linq.Expressions;

public class PlayerShooting : MonoBehaviour
{
    [Header("Disparo")]
    public Transform firePoint;
    public float fireRate = 0.25f;
    public float fireRange = 100f;
    public int damage = 10;
    public LayerMask hitLayers;

    [Header("Munición")]
    public int maxAmmo = 7;
    public TextMeshProUGUI ammoText;
    public int shootMouseButton = 0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("Weapons")]
    public WeaponData currentWeapon;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    private int[] currentAmmoPerWeapon;
    private int[] totalAmmoPerWeapon;
    private int[] totalAmmoPerWeaponOriginal;
    private int index;


    void Start()
    {
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(shootMouseButton) && Time.time >= nextTimeToFire)
        {
            if (currentAmmoPerWeapon[index] > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
            else
            {
                Debug.Log("Sin munición");
            }
        }

        if (Input.GetKeyDown(reloadKey))
        {
            StartCoroutine(Reload());
        }
    }

    public void InitializeWeapons(int[] currentAmmoList, int[] totalAmmoList)
    {
        currentAmmoPerWeapon = currentAmmoList;
        totalAmmoPerWeapon = totalAmmoList;
        totalAmmoPerWeaponOriginal = (int[])totalAmmoList.Clone();
        UpdateAmmoUI();
    }
    void Shoot()
    {
        currentAmmoPerWeapon[index]--;
        UpdateAmmoUI();

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, fireRange, hitLayers))
        {

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, gameObject);
            }
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * fireRange, Color.red, 1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.shootClip);
    }

    IEnumerator Reload()
    {
        if (totalAmmoPerWeapon[index] > 0)
        {


            isReloading = true;
            Debug.Log("Recargando...");

            SoundManager.Instance.PlaySound(SoundManager.Instance.reloadClip);

            // Espera 1.5 segundos (puedes ajustar)
            yield return new WaitForSeconds(1.5f);

            if (totalAmmoPerWeapon[index] > (maxAmmo - currentAmmoPerWeapon[index]))
            {
                totalAmmoPerWeapon[index] = totalAmmoPerWeapon[index] - (maxAmmo - currentAmmoPerWeapon[index]);
                currentAmmoPerWeapon[index] = maxAmmo;
            }
            else
            {
                currentAmmoPerWeapon[index] = currentAmmoPerWeapon[index] + totalAmmoPerWeapon[index];
                totalAmmoPerWeapon[index] = 0;
            }
            UpdateAmmoUI();

            isReloading = false;
        }
    }

    public void ApplyWeapon(WeaponData weapon, int index)
    {
        currentWeapon = weapon;

        damage = weapon.damage;
        fireRate = weapon.fireRate;
        fireRange = weapon.fireRange;
        maxAmmo = weapon.maxAmmo;
        this.index = index;


        UpdateAmmoUI();
    }

    public void ResetAmmo(int index2)
    {
        Debug.Log($"Reset Ammo {totalAmmoPerWeaponOriginal[index2]}");
        totalAmmoPerWeapon[index2] = totalAmmoPerWeaponOriginal[index2];
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmoPerWeapon[index] + " / " + totalAmmoPerWeapon[index];
        }
    }
}
