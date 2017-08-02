using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : BaseCharacter
{
    [Header("Skeleton")]
    [Header("Properties")]
    [SerializeField]
    private int _amountDamage;
    [SerializeField]
    private float _forceX = 10;
    [SerializeField]
    private float _forceY = 10;

    [Header("References")]
    [SerializeField]
    private EnemyRotationWeapon _bonePrefab;

    private bool _isDead;

    protected override void Awake()
    {
        base.Awake();

        GetAnimator().SetLayerWeight(2, 1);
    }

    private void FixedUpdate()
    {
        if (!_isDead && CanAttack())
        {
            Attack();
            EnemyRotationWeapon bone = Instantiate(_bonePrefab, GetAttackPosition(), this.transform.rotation);

            bone.SetDamage(_amountDamage);

            float randomX = Random.Range(_forceX, _forceX / 2) * -1;
            float randomY = Random.Range(_forceY, _forceY / 2);
            Vector2 direction = Vector2.right * randomX + Vector2.up * randomY;

            bone.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        }
    }

    public override void OnDead()
    {
        GetAnimator().SetBool("isDead", true);
        this.GetComponent<Collider2D>().enabled = false;
        DropRandomItem();
        _isDead = true;
        Destroy(this.gameObject, 1);
    }
}
