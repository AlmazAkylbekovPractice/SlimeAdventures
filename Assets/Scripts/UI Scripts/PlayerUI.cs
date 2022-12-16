using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance { get; set; }

    [SerializeField]
    private Text _playerCurrency;

    [SerializeField]
    private Slider _playerHealthSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void UpdatePlayerCurrency()
    {
        _playerCurrency.text = PlayerStats.CurrencyPoints.ToString();
    }

    public void UpdatePlayerHealth()
    {
        _playerHealthSlider.value = PlayerStats.PlayerHealth / 1000;
    }
}
