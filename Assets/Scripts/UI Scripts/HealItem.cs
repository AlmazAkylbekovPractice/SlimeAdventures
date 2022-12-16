using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealItem : MonoBehaviour
{
    private int _price = 10;

    [SerializeField]
    private Button _buyButton;

    [SerializeField]
    private Text _warningText;

    [SerializeField]
    private Text _priceText;

    private void Awake()
    {
        _warningText.enabled = false;
        _buyButton.onClick.AddListener(Heal);
    }

    private void Heal()
    {
        if (PlayerStats.CurrencyPoints >= _price)
        {
            _warningText.enabled = false;

            PlayerStats.PlayerHealth = 1000f;

            PlayerUI.Instance.UpdatePlayerHealth();

        } else
        {
            _warningText.enabled = true;
        }
    }
}
