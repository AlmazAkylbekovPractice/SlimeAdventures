using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    private SphereCollider _collider;
    private Rigidbody _rigidbody;

    private Transform _target;

    private float _force = 1f;
    public float _damage;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void LaunchBullet(Transform targetPos)
    {
        _target = targetPos;

        // Calculate the direction from the bullet to the target
        Vector3 direction = targetPos.position - transform.position;

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
            Destroy(gameObject);
        }
    }
}