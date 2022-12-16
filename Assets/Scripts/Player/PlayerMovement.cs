using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerAnimationState { Idle, Walk, Attack, Damage, Dies }

public class PlayerMovement : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider _collider;
    private Rigidbody _rigidbody;

    private PlayerAnimationState _currentState;

    private Vector3 _position;

    private LayerMask _enemyLayer;

    [SerializeField]
    private GameObject _currentEnemy;

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _bulletHolder;

    private float _shotTimer;

    [SerializeField]
    private Transform targetPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
        _currentState = PlayerAnimationState.Walk;
        _shotTimer = PlayerStats.PlayerCoolDown;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case PlayerAnimationState.Idle:
                HandleIdleState();
                break;

            case PlayerAnimationState.Walk:
                HandleWalkState();
                break;

            case PlayerAnimationState.Attack:
                HandleAttackState();
                break;
        }
    }

    private void HandleIdleState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // Handle idle state
        }
    }

    private void HandleWalkState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

        _animator.SetTrigger("isWalking");

        SearchForEnemy();
    }

    private void SearchForEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5f, _enemyLayer);

        if (enemies.Length.Equals(0))
        {
            _currentEnemy = null;
            return;
        }

        foreach (Collider enemy in enemies)
        {
            _currentEnemy = enemy.gameObject;
            _currentState = PlayerAnimationState.Attack;
            break;
        }
    }

    private void HandleAttackState()
    {
        if (_currentEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, _currentEnemy.transform.position);

            if (distance > 5f)
            {
                _currentEnemy = null;
                SearchForEnemy();
                return;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

            _animator.SetTrigger("isAttacking");

            if (_shotTimer > Time.deltaTime)
                _shotTimer -= Time.deltaTime;
            else
            {
                var bullet = Instantiate(_bulletPrefab, _bulletHolder.transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().LaunchBullet(_currentEnemy.transform, PlayerStats.PlayerDamage);
                _shotTimer = PlayerStats.PlayerCoolDown;
            }

            UpdateRotation();

        }
        else
        {
            RotateTowardsTargetPoint();
            _currentState = PlayerAnimationState.Walk;
        }
    }

    private void UpdateRotation()
    {
        var relativePos = _currentEnemy.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    private void RotateTowardsTargetPoint()
    {
        var relativePos = targetPoint.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    private void OnAnimatorMove()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        _position = _animator.rootPosition;
        transform.position = _position;
    }

    public void ApplyDamage(float damage)
    {
        damage -= PlayerStats.PlayerResistence;

        PlayerStats.PlayerHealth -= damage;

        PlayerUI.Instance.UpdatePlayerHealth();

        if (PlayerStats.PlayerHealth <= 0)
        {
            Destroy(gameObject);
            ReloadLevel();
        }
    }

    private void ReloadLevel()
    {
        PlayerStats.PlayerHealth = 1000f;
        PlayerStats.CurrencyPoints = 0;
        PlayerStats.PlayerCoolDown = 1f;
        PlayerStats.PlayerResistence = 0f;
        PlayerStats.PlayerDamage = 100;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
