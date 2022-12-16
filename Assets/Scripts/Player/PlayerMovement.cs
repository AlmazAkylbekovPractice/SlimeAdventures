using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationState { Idle, Walk, Attack, Damage, Dies }

public class PlayerMovement : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider _collider;
    private Rigidbody _rigidbody;

    private PlayerAnimationState _currentState;

    private Vector3 _position;

    private LayerMask _enemyLayer;

    private GameObject _currentEnemy;

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _bulletHolder;

    private float _shotTimer = 0f;

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
        Collider[] enemies = Physics.OverlapSphere(transform.position, 50f, _enemyLayer);

        foreach (Collider enemy in enemies)
        {
            _currentEnemy = enemy.gameObject;
            _currentState = PlayerAnimationState.Attack;
            break;
        }
    }

    private void HandleAttackState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

        _animator.SetTrigger("isAttacking");

        if (_shotTimer > Time.deltaTime)
            _shotTimer -= Time.deltaTime;
        else
        {
            var bullet = Instantiate(_bulletPrefab, _bulletHolder.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().LaunchBullet(_currentEnemy.transform);
            _shotTimer = 1f;
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (_currentEnemy == null) return;

        var relativePos = _currentEnemy.transform.position - transform.position;
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
}
