using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum EnemyAnimationState { Idle, WalkToPlayer, AttackPlayer, GetsDamage, Dies }

public class EnemyMovement : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider _collier;
    private Rigidbody _rigidbody;

    private EnemyAnimationState _currentState;

    private GameObject _player;
    private Vector3 _position;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _collier = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _currentState = EnemyAnimationState.WalkToPlayer;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyAnimationState.Idle:
                break;

            case EnemyAnimationState.WalkToPlayer:
                MoveToPlayer();
                break;

            case EnemyAnimationState.AttackPlayer:
                AttackPlayer();
                break;
        }
    }

    private void MoveToPlayer()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;
        _animator.SetTrigger("isWalking");
    }

    private void AttackPlayer()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        _animator.SetTrigger("isAttacking");
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
        var relativePos = _player.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player)
            _currentState = EnemyAnimationState.AttackPlayer;
    }
}
