using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    private SphereCollider _collider;
    private Rigidbody _rigidbody;

    private Transform _target;

    private float _force = 2f;

    public float _damage;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void LaunchBullet(Transform targetPos, float damage)
    {
        _target = targetPos;
        _damage = damage;

        // Calculate the direction from the bullet to the target
        Vector3 direction = _target.position - transform.position;

        // Add a vertical component to the direction vector
        direction.y += Mathf.Sin(Time.time * 10f) * 0.5f; // This will cause the bullet to oscillate up and down as it moves

        // Normalize the direction to get a unit vector
        direction = direction.normalized;

        // Apply the force to the bullet's rigidbody, using the direction as the force vector
        _rigidbody.AddForce(direction * _force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            // Calculate the direction from the bullet to the target
            Vector3 direction = _target.position - transform.position;

            // Normalize the direction to get a unit vector
            direction = direction.normalized;

            // Apply the force to the bullet's rigidbody, using the direction as the force vector
            _rigidbody.AddForce(direction * _force, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().ApplyDamage(_damage);
            Destroy(gameObject);
        }
    }
}