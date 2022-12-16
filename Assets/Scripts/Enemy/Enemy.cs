using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public enum EnemyState { Idle, WalkToPlayer, AttackPlayer, GetsDamage, Dies }

public class Enemy : MonoBehaviour
{
    public float _enemyHealth = 100f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private GameObject _player;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Transform _cameraPos;

    [SerializeField]
    private GameObject _damageText;

    [SerializeField]
    private GameObject _canvas;

    private EnemyState _currentState;
    private float _factor;

    private void Awake()
    {
        _enemyHealth = Random.Range(150, 300);
        _factor = 100 / _enemyHealth;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
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
        if (_player != null)
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

        // Rotate the health slider towards the camera
        _healthSlider.transform.LookAt(_cameraPos.transform.position);
    }

    public void ApplyDamage(float damage)
    {
        _enemyHealth -= damage;

        _healthSlider.value = (_enemyHealth / 100) * _factor;

        var text = Instantiate(_damageText, new Vector3 (-0.303f, -0.33f, 0), Quaternion.identity);
        text.transform.SetParent(_canvas.transform, false);
        text.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        text.GetComponent<DamageText>().SetText(damage);

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
        if (_player != null)
        {
            ChangePosition();
            UpdateRotation();
        }
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
