using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : BaseWeapon
{
    [Header("Whip")]
    [SerializeField]
    private int _damage;
    [SerializeField]
    private int _maxDamageEntities;

    public override void Attack(float force)
    {
        Collider2D[] colliders = new Collider2D[_maxDamageEntities];
        ContactFilter2D contact = new ContactFilter2D();
        contact.useTriggers = true;

        if (force > 0)
        {
            this.transform.Rotate(0, 0, 180);
        }

        int count = Physics2D.OverlapCollider(this.GetComponent<BoxCollider2D>(), contact, colliders);

        for (int i = 0; i < count; i++)
        {
            if (GetOwnCollider().Equals(colliders[i])) continue;

            Health health = colliders[i].GetComponent<Health>();
            DestructibleObject destructible = colliders[i].GetComponent<DestructibleObject>();

            if (health)
            {
                float hitOnPercentageOfBody = CalcPercentageOfBody(colliders[i], GetOwnCollider());

                int damageMultiplier = hitOnPercentageOfBody > 0.7f ? 2 : 1;
                health.ReduceLife(_damage * damageMultiplier, Health.EDamageTypes.Cutting);
            }
            else if (destructible)
            {
                destructible.DestroyObject();
            }
        }
    }

    protected override void Damage(Health health, float hitOnPercentageOfBody)
    {
    }
}
