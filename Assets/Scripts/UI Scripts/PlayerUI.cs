using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI Instance { get; set; }

    [SerializeField]
    private Text _playerCurrency;

    [SerializeField]
    private Slider _playerHealthSlider;


    [SerializeField]
    private GameObject reloadPanel;

    [SerializeField]
    private Button _yesButton;

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

    private void Start()
    {
        _yesButton.onClick.AddListener(ReloadGame);
        reloadPanel.SetActive(false);
    }


    public void UpdatePlayerCurrency()
    {
        _playerCurrency.text = PlayerStats.CurrencyPoints.ToString();
    }

    public void UpdatePlayerHealth()
    {
        _playerHealthSlider.value = PlayerStats.PlayerHealth / 1000;
    }

    public void DisplayreloadMenu()
    {
        reloadPanel.SetActive(true);
    }

    private void ReloadGame()
    {
        PlayerStats.PlayerHealth = 1000f;
        PlayerStats.CurrencyPoints = 0;
        PlayerStats.PlayerCoolDown = 1f;
        PlayerStats.PlayerResistence = 0f;
        PlayerStats.PlayerDamage = 100;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
