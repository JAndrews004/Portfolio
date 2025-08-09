using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float fireRate;
    public int ammoCapacity;
    public float range;
    public float RelaodTime;
    public AudioClip ShootSound;
    public enum WeaponType { Rifle, Shotgun, Launcher }
    public WeaponType weaponType;
    public Sprite icon;
    // Add more fields as needed
}
