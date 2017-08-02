using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : BaseWeapon
{
    [Header("Dagger")]
    [SerializeField]
    private float _speedXMovement = 1;
    [SerializeField]
    private int _damage;

    private float _force;

    private void FixedUpdate()
    {
        Vector3 nextPosition = this.transform.position + Vector3.right * _force * _speedXMovement * Time.fixedDeltaTime;

        GetRigidbody().MovePosition(nextPosition);
    }

    public override void Attack(float force)
    {
        _force = force;
        this.GetComponent<SpriteRenderer>().flipX = force > 0;
    }

    protected override void Damage(Health health, float hitOnPercentageOfBody)
    {
        // Aplica un multiplicador de daño en caso de que el arma haya impactado en una parte superior del objetivo
        int damageMultiplier = hitOnPercentageOfBody > 0.7f ? 2 : 1;
        health.ReduceLife(_damage * damageMultiplier, Health.EDamageTypes.Perforating);
        Destroy(this.gameObject);
    }
}
