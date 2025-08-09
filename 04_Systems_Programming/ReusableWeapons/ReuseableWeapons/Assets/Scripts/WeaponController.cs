using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public WeaponData weaponConfig;
    public Transform firePoint;
    public int ammo=0;
    public bool canFire = true;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        weaponConfig = null;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponConfig != null) 
        {
            if (Input.GetMouseButton(0))
            {
                switch (weaponConfig.weaponType)
                {
                    case WeaponData.WeaponType.Rifle:
                        FireWeaponRifle();
                        break;
                    case WeaponData.WeaponType.Shotgun:
                        FireWeaponShotgun();
                        break;

                }
            }
            if (Input.GetKey(KeyCode.R))
            {
               
                StartCoroutine(ResetAmmoAfterDelay(weaponConfig.RelaodTime));
                
            }
        }
    }

    void FireWeaponRifle() 
    {
        if (canFire && ammo >0)
        {
            audioSource.PlayOneShot(weaponConfig.ShootSound);

            Ray ray = new Ray(firePoint.position, firePoint.forward);

            Debug.DrawRay(firePoint.position, firePoint.forward * weaponConfig.range, Color.red, 0.2f);
            if (Physics.Raycast(ray, out RaycastHit hit, weaponConfig.range))
            {
                Debug.DrawLine(firePoint.position, hit.point, Color.green, 0.2f);
            }
            StartCooldown();
            ammo -= 1;
        }
        if (ammo <= 0) 
        {
            StartCoroutine(ResetAmmoAfterDelay(weaponConfig.RelaodTime));
        }

        
    }

    void FireWeaponShotgun() 
    {
        if (canFire && ammo > 0)
        {
            int pellets = 6;
            float spreadAngle = 10f;

            for (int i = 0; i < pellets; i++)
            {
                Vector3 spreadDir = Quaternion.Euler(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0
                ) * firePoint.forward;

                
                Ray ray = new Ray(firePoint.position, spreadDir);

                Debug.DrawRay(firePoint.position, spreadDir * weaponConfig.range, Color.red, 0.2f);

                if (Physics.Raycast(ray, out RaycastHit hit, weaponConfig.range))
                {
                    Debug.DrawRay(firePoint.position, spreadDir * weaponConfig.range, Color.green, 0.2f);
                }
            }
            audioSource.PlayOneShot(weaponConfig.ShootSound);
            StartCooldown();
            ammo -= 1;
        }
        if (ammo <= 0)
        {
            StartCoroutine(ResetAmmoAfterDelay(weaponConfig.RelaodTime));
        }
    }

    private void StartCooldown()
    {
        canFire = false;
        StartCoroutine(ResetCanFireAfterDelay(1/weaponConfig.fireRate));
    }

    private IEnumerator ResetCanFireAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canFire = true;
    }

    private IEnumerator ResetAmmoAfterDelay(float delay)
    {
        canFire = false;
        yield return new WaitForSeconds(delay);
        ammo = weaponConfig.ammoCapacity;
        canFire = true;
    }
}
