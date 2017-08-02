using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : BaseCharacter
{
    [Header("Bat")]
    [SerializeField]
    private float _minDistanceToFollow;
    [SerializeField]
    private float _minDistanceToAttack;
    [SerializeField]
    private float _minDistanceToIdle;
    [SerializeField]
    private int _amountDamage;
    [SerializeField]
    private Health.EDamageTypes _damageType;
    [SerializeField]
    private int _moneyToGiveOnDead = 5;

    private Vector3 _originalPosition;
    private bool _bIsSleeping = true;

    protected override void Awake()
    {
        base.Awake();

        GetAnimator().SetLayerWeight(1, 1);
    }

    private void Start()
    {
        _originalPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 direction = GameManager.GetInstance().GetSimon().GetAttackPosition() - this.transform.position;
        float distance = direction.magnitude;

        Debug.DrawRay(this.transform.position, direction, Color.red, .2f);

        // Ataca
        if (distance < _minDistanceToAttack)
        {
            Movement(Vector2.zero, direction.x > 0, 1);
            if (CanAttack())
            {
                RaycastHit2D[] hitArray = Physics2D.RaycastAll(this.transform.position, direction.normalized, _minDistanceToAttack);
                
                Health health = null;

                foreach (RaycastHit2D hit in hitArray)
                {
                    if (hit.collider.Equals(this.GetComponent<Collider2D>())) continue;

                    health = hit.transform.GetComponent<Health>();
                    if (health)
                    {
                        health.ReduceLife(_amountDamage, _damageType);
                        Attack();
                        return;
                    }
                }
            }
        }
        // Persigue
        else if (distance < _minDistanceToFollow)
        {
            _bIsSleeping = false;
            Movement(direction.normalized);
        }
        // Vuelve a su posición original
        else if (!_bIsSleeping)
        {
            Vector3 directionToOriginal = _originalPosition - this.transform.position;
            float distanceToOriginal = directionToOriginal.magnitude;

            if (distanceToOriginal < _minDistanceToIdle)
            {
                Movement(Vector2.zero, false, 0);
                this.transform.position = _originalPosition;
                _bIsSleeping = true;

                return;
            }

            Movement(directionToOriginal.normalized);
        }
    }

    public override void OnDead()
    {
        DropRandomItem();
        Destroy(this.gameObject);
    }
}
