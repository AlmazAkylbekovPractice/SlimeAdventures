using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationState { Idle, Walk, Attack, Damage, Dies }

public class PlayerMovement : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider _collier;
    private Rigidbody _rigidbody;

    private PlayerAnimationState _currentState;

    private Vector3 _position;

    private LayerMask _enemyLayer;

    private GameObject _currentEnemy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _collier = GetComponent<BoxCollider>();
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
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

                break;

            case PlayerAnimationState.Walk:
                Walk();
                break;

            case PlayerAnimationState.Attack:
                AttackEnemy();
                break;
        }
    }

    private void Walk()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

        _animator.SetTrigger("isWalking");

        SearchEnemy();
    }

    private void SearchEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, 4f, _enemyLayer);

        foreach (Collider enemy in enemies)
        {
            _currentEnemy = enemy.gameObject;
            _currentState = PlayerAnimationState.Attack;
            break;
        }
    }

    private void AttackEnemy()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        _animator.SetTrigger("isAttacking");
        Debug.Log("Enemy Attacked");
    }


    private void OnAnimatorMove()
    {
        ChangePosition();
        UpdateRotation();
    }

    private void ChangePosition()
    {
        _position = _animator.rootPosition;
        transform.position = _position;
    }

    private void UpdateRotation()
    {
        if (_currentEnemy == null) return;

        var relativePos = _currentEnemy.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

}
