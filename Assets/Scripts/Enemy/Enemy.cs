using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum EnemyState { Idle, WalkToPlayer, AttackPlayer, GetsDamage, Dies }

public class Enemy : MonoBehaviour
{
    public float _enemyHealth = 100f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private GameObject _player;

    private EnemyState _currentState;

    private void Awake()
    {
        _enemyHealth = Random.Range(50, 200);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentState = EnemyState.WalkToPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            _currentState = EnemyState.AttackPlayer;
            _player.GetComponent<PlayerMovement>().ApplyDamage(100);
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.WalkToPlayer:
                _animator.SetTrigger("isWalking");
                break;

            case EnemyState.AttackPlayer:
                _animator.SetTrigger("isAttacking");
                break;

        }
    }

    public void ApplyDamage(float damage)
    {
        _enemyHealth -= damage;

        if (_enemyHealth <= 0)
        {
            _animator.SetTrigger("isDead");

            PlayerStats.CurrencyPoints += 10;
            PlayerUI.Instance.UpdatePlayerCurrency();

            Destroy(gameObject);
        }
    }

    private void OnAnimatorMove()
    {
        ChangePosition();
        UpdateRotation();
    }

    private void ChangePosition()
    {
        transform.position = _animator.rootPosition;
    }

    private void UpdateRotation()
    {
        var relativePos = _player.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

}
