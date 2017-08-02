using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : BaseWeapon
{
    [Header("Axe")]
    [SerializeField]
    private float _speedXMovement = 1;
    [SerializeField]
    private float _speedYMovement = 1;
    [SerializeField]
    private int _damage;

    private float _force;
    private float _initialTime;

    protected override void Start()
    {
        base.Start();
        _initialTime = Time.time;
    }

    private void FixedUpdate()
    {
        Vector3 nextPosition = this.transform.position + Vector3.right * _force * _speedXMovement * Time.fixedDeltaTime;

        if (Time.time - _initialTime > 1)
        {
            nextPosition -= Vector3.up * _speedYMovement * Time.fixedDeltaTime;
        }
        else
        {
            nextPosition += Vector3.up * _speedYMovement * Time.fixedDeltaTime;
        }

        GetRigidbody().MovePosition(nextPosition);
    }

    public override void Attack(float force)
    {
        _force = force;
    }

    protected override void Damage(Health health, float hitOnPercentageOfBody)
    {
        // Aplica un multiplicador de daño en caso de que el arma haya impactado en una parte superior del objetivo
        int damageMultiplier = hitOnPercentageOfBody > 0.7f ? 2 : 1;
        health.ReduceLife(_damage * damageMultiplier, Health.EDamageTypes.Cutting);
        Destroy(this.gameObject);
    }
}
