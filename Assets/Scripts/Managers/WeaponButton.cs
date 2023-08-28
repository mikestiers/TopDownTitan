using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IPointerClickHandler // Implement IPointerClickHandler
{
    public Image weaponIcon;
    public Image iconImage;
    public Text weaponIconText;

    public Weapon weapon;
    private int weaponIndex;
    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Initialize(Weapon weapon, int index)
    {
        this.weapon = weapon;
        weaponIndex = index;
    }

    private void Update()
    {
        if (weapon != null)
        {
            if (weaponIndex == 0)
                weaponIconText.text = "∞";
            else
                weaponIconText.text = weapon.currentBullets.ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData) // Using a button UI object didn't colors properly
    {
        player.inventory.EquipWeapon(weaponIndex);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}