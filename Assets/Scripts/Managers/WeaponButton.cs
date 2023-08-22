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
    public Weapon weaponPrefab;

    public void SetWeapon(Weapon prefab, Sprite icon)
    {
        weaponIcon.sprite = icon;
        weaponPrefab = prefab;
        iconImage.name = prefab.name;
        weaponIconText.text = prefab.name;
    }

    public void OnPointerClick(PointerEventData eventData) // Using a button UI object didn't colors properly
    {
        Debug.Log($"Weapon Icon Clicked: {iconImage}");
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.ChangeWeapon(weaponPrefab, weaponIcon.sprite);

        // Set the icon in the HUD to active
        HUD.singleton.SetActiveIcon(this);
    }
}
