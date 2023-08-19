using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public Image weaponIcon;
    public Button buttonPrefab;
    public Weapon weaponPrefab;

    public void SetWeapon(Weapon prefab, Sprite icon)
    {
        weaponIcon.sprite = icon;
        weaponPrefab = prefab;
        buttonPrefab.name = prefab.name;

        // Set up the button click event
        buttonPrefab.onClick.AddListener(OnButtonClick);

        // Get the Text component of the button and set its text property
        Text buttonText = buttonPrefab.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = prefab.name;
        }
    }

    public void OnButtonClick()
    {
        Debug.Log($"Weapon Button Clicked: {buttonPrefab}");
        ColorBlock colorBlock = buttonPrefab.colors;
        colorBlock.pressedColor = Color.green;
        colorBlock.normalColor = Color.green;
        buttonPrefab.colors = colorBlock;
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.ChangeWeapon(weaponPrefab, weaponIcon.sprite);
    }
}