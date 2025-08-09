using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI AmmoMsg;
    public TextMeshProUGUI WeaponSelected;
    private WeaponController Controller;

    void Start()
    {
        Controller = GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.weaponConfig != null)
        {
            AmmoMsg.text = Controller.ammo + " / " + Controller.weaponConfig.ammoCapacity;
            WeaponSelected.text = Controller.weaponConfig.name;
        }
        else
        {
            AmmoMsg.text = "0 / 0";
            WeaponSelected.text = "";

        }
    }
}
