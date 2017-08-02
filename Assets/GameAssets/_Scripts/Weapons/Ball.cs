using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseWeapon
{
    [Header("Ball")]
    [SerializeField]
    private float _forceX = 10;
    [SerializeField]
    private float _forceY = 10;
    [SerializeField]
    private float _damageRadius;
    [SerializeField]
    private int _hitDamage;
    [SerializeField]
    private int _explosionDamage;
    [SerializeField]
    private Health.EDamageTypes _hitType;
    [SerializeField]
    private Health.EDamageTypes _explosionType;

    public override void Attack(float force)
    {
        float randomX = Random.Range(_forceX, _forceX / 2) * force;
        float randomY = Random.Range(_forceY, _forceY / 2);
        Vector2 direction = Vector2.right * randomX + Vector2.up * randomY;

        GetRigidbody().AddForce(direction, ForceMode2D.Impulse);
    }

    protected override void Damage(Health health, float hitOnPercentageOfBody)
    {
        // Aplica un multiplicador de daño en caso de que el arma haya impactado en una parte superior del objetivo
        int damageMultiplier = hitOnPercentageOfBody > 0.7f ? 2 : 1;
        health.ReduceLife(_hitDamage * damageMultiplier, _hitType);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (_ownCollider && collision.Equals(_ownCollider))
        {
            return;
        }

        foreach (Collider2D col in Physics2D.OverlapCircleAll(this.transform.position, _damageRadius))
        {
            if (col.Equals(GetOwnCollider())) continue;

            Health health = col.GetComponent<Health>();

            if (health)
            {
                health.ReduceLife(_explosionDamage, _explosionType);
            }
        }

        Transform child = this.transform.GetChild(0);

        child.parent = null;
        child.gameObject.SetActive(true);
        child.localScale = Vector3.one * _damageRadius;
        Destroy(child.gameObject, 1);

        Destroy(this.gameObject);
        
    }
}
