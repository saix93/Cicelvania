using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : BaseWeapon
{
    [Header("Boomerang")]
    [SerializeField]
    private float _attackDistance;
    [SerializeField]
    private float _speedXMovement;
    [SerializeField]
    private int _damageForward;
    [SerializeField]
    private int _damageBackwards;
    [SerializeField]
    private float _thresholdBackDistance;

    private bool _goingBack;
    private Vector2 _destinyPosition;

    private float _orientation;

    private void FixedUpdate()
    {
        if (Vector2.Distance(GetRigidbody().position, _destinyPosition) < _thresholdBackDistance)
        {
            this.transform.position = _destinyPosition;
            _goingBack = true;
        }

        Vector2 nextPosition = Vector2.zero;
        if (_goingBack)
        {
            Vector2 direction = Vector3.Normalize(GameManager.GetInstance().GetSimon().transform.position - this.transform.position);
            nextPosition = GetRigidbody().position + direction * _speedXMovement * Time.fixedDeltaTime;
        }
        else
        {
            nextPosition = GetRigidbody().position + Vector2.right * _orientation * _speedXMovement * Time.fixedDeltaTime;
        }

        GetRigidbody().MovePosition(nextPosition);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (_goingBack && collision.GetComponent<Simon>())
        {
            Destroy(this.gameObject);
        }
        else
        {
            base.OnTriggerEnter2D(collision);
        }
    }

    public override void Attack(float force)
    {
        _destinyPosition = GetRigidbody().position + Vector2.right * force * _attackDistance;
        _orientation = force;
    }

    protected override void Damage(Health health, float hitOnPercentageOfBody)
    {
        // Aplica un multiplicador de daño en caso de que el arma haya impactado en una parte superior del objetivo
        int damageMultiplier = hitOnPercentageOfBody > 0.7f ? 2 : 1;
        health.ReduceLife(_goingBack ? _damageBackwards : _damageForward * damageMultiplier, Health.EDamageTypes.Cutting);
    }
}
