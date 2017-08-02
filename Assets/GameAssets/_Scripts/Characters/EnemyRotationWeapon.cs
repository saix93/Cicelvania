using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationWeapon : MonoBehaviour
{
    [Header("Enemy Rotation Weapon")]
    [SerializeField]
    private Health.EDamageTypes _damageType;
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody2D _rb;
    private int _damage;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        Vector3 rotation = new Vector3(0, 0, 1);
        this.transform.Rotate(rotation * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            simon.GetHealth().ReduceLife(_damage, _damageType);

            Destroy(this.gameObject);
        }
    }

    public void SetDamage(int newDamage)
    {
        _damage = newDamage;
    }
}
