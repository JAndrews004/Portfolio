using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private WeaponController controller; 
    public WeaponData weaponConfig1;
    public WeaponData weaponConfig2;
    public WeaponData weaponConfig3;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) 
        {
            controller.weaponConfig = weaponConfig1;
            controller.ammo = weaponConfig1.ammoCapacity;

        }
        if (Input.GetKeyDown("2")) 
        {
            controller.weaponConfig = weaponConfig2;
            controller.ammo = weaponConfig2.ammoCapacity;
        }
        if (Input.GetKeyDown("3")) 
        {
            controller.weaponConfig = weaponConfig3;
            controller.ammo = weaponConfig3.ammoCapacity;
        }
    }
}
