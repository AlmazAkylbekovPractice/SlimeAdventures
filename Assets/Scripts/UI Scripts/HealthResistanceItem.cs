using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthResistanceItem : MonoBehaviour
{
    private int _level = 1;
    private int _price = 10;

    [SerializeField]
    private Button _buyButton;

    [SerializeField]
    private Text _levelText;

    [SerializeField]
    private Text _warningText;

    [SerializeField]
    private Text _priceText;

    private void Awake()
    {
        _levelText.text = _level.ToString();
        _priceText.text = _price.ToString();

        _warningText.enabled = false;
        _buyButton.onClick.AddListener(IncreaseShootingSpeed);
    }


    private void IncreaseShootingSpeed()
    {
        if (PlayerStats.CurrencyPoints >= _price)
        {
            PlayerStats.CurrencyPoints -= _price;

            PlayerStats.PlayerResistence += 5;

            _price += 10;
            _level += 1;

            _warningText.enabled = false;

            _levelText.text = _level.ToString();
            _priceText.text = _price.ToString();

            PlayerUI.Instance.UpdatePlayerCurrency();

        }
        else
        {
            _warningText.enabled = true;
        }
    }
}
