using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("Disparo")]
    public Transform firePoint;
    public float fireRate = 0.25f;
    public float fireRange = 100f;
    public int damage = 10;
    public LayerMask hitLayers;

    [Header("Munición")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;
    public int shootMouseButton = 0;
    public KeyCode reloadKey = KeyCode.R;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;


    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(shootMouseButton) && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
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

    void Shoot()
    {
        currentAmmo--;
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
        isReloading = true;
        Debug.Log("Recargando...");

        SoundManager.Instance.PlaySound(SoundManager.Instance.reloadClip);

        // Espera 1.5 segundos (puedes ajustar)
        yield return new WaitForSeconds(1.5f);

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        isReloading = false;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}
