using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : Singleton<WeaponInventory>
{
    public PlayerController player;
    public Weapon currentWeapon => weapons[currentIndex];
    public Weapon startingWeapon;
    private List<Weapon> weapons = new List<Weapon>();

    private int currentIndex;

    private void Start()
    {
        AddWeapon(startingWeapon);
    }

    public void AddWeapon(Weapon weapon)
    {
        Debug.Log("List of Weapons:");
        foreach (Weapon wep in weapons)
        {
            Debug.Log(wep.weaponId);
        }
        if (!CheckWeaponAlreadyExists(weapon))
        {
            Weapon w = Instantiate(weapon.gameObject, transform).GetComponent<Weapon>();
            weapons.Add(w);
            HUD.singleton.CreateWeaponButton(w, weapons.Count - 1);
            //EquipWeapon(weapons.Count - 1);
        }
        else
        {
            AddAmmo(weapon.weaponId, weapon.currentBullets);
        }
    }

    public bool CheckWeaponAlreadyExists(Weapon weapon)
    {
        Debug.Log($"Weapon exists: {weapons.Exists(w => w.weaponId == weapon.weaponId)}");
        Debug.Log($"Weapon exists: {weapon.weaponId}");
        return weapons.Exists(w => w.weaponId == weapon.weaponId);
    }

    public void EquipWeapon(int index)
    {
        currentIndex = index;
        HUD.singleton.OnSelectWeaponIndex(currentIndex);
    }

    public void RemoveWeapon(int weaponId)
    {
        Weapon w = GetWeapon(weaponId);
        EquipWeapon(0);
        Destroy(w.gameObject);
        weapons.RemoveAt(weaponId);
        HUD.singleton.RemoveWeaponButton(w, weaponId);
    }

    public Weapon GetWeapon(int index)
    {
        return weapons[index];
    }

    public void AddAmmo(int weaponId, int ammoPickup)
    {
        Weapon[] currentWeapons = player.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in currentWeapons)
        {
            if (weapon.weaponId == weaponId)
                weapon.currentBullets += ammoPickup;
        }
    }
}
