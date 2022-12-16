using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private float _damage = 0;
    private Transform _cameraPos;

    private float _timer = 2f;

    public void SetText(float damage)
    {
        _cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _damage = damage;
        gameObject.GetComponent<Text>().text = "-" + _damage.ToString();
    }

    private void FixedUpdate()
    {
        if (_damage != 0)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;

                transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.03f,
                transform.position.z);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
