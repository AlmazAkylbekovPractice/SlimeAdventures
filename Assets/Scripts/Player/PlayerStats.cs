public static class PlayerStats
{
    private static float _playerHealth = 1000;
    private static float _playerDamage = 100;
    private static float _playerResistance = 0;
    private static float _playerCoolDown = 1;

    private static int _currencyPoints = 0;

    public static float PlayerHealth
    {
        get
        {
            return _playerHealth;
        }
        set
        {
            _playerHealth = value;
        }
    }

    public static float PlayerDamage
    {
        get
        {
            return _playerDamage;
        }
        set
        {
            _playerDamage = value;
        }
    }

    public static float PlayerResistence
    {
        get
        {
            return _playerResistance;
        }
        set
        {
            _playerResistance = value;
        }
    }

    public static int CurrencyPoints
    {
        get
        {
            return _currencyPoints;
        }
        set
        {
            _currencyPoints = value;
        }
    }

    public static float PlayerCoolDown
    {
        get
        {
            return _playerCoolDown;
        }
        set
        {
            _playerCoolDown = value;
        }
    }

}
