using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseWeapon : MonoBehaviour
{
    [Header("BaseWeapon")]
    [SerializeField]
    private float _lifeTime;
    [SerializeField]
    private Sprite _spriteWeapon;

    private Rigidbody2D _rb;
    protected Collider2D _ownCollider;

    protected virtual void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }

    public void Initialize(Collider2D ownCollider)
    {
        this._ownCollider = ownCollider;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ownCollider && collision.Equals(_ownCollider))
        {
            return;
        }

        if (collision.CompareTag("Shield"))
        {
            GameManager.GetInstance().AudioRandomShieldHit().Play();
            Destroy(this.gameObject);
            return;
        }

        Health health = collision.GetComponent<Health>();
        
        if (health)
        {
            float hitOnPercentageOfBody = CalcPercentageOfBody(collision, _ownCollider);

            Damage(health, hitOnPercentageOfBody);
        }
    }

    protected float CalcPercentageOfBody(Collider2D target, Collider2D own)
    {
        float targetBoundsCenter = target.bounds.center.y;
        float ownBoundsCenter = own.bounds.center.y;

        float distance = ownBoundsCenter - targetBoundsCenter;

        return ((target.bounds.extents.y + distance) / target.bounds.size.y);
    }

    public Rigidbody2D GetRigidbody()
    {
        return _rb;
    }

    public Collider2D GetOwnCollider()
    {
        return _ownCollider;
    }

    public float GetLifeTime()
    {
        return _lifeTime;
    }

    public Sprite GetSprite()
    {
        return _spriteWeapon;
    }

    protected abstract void Damage(Health health, float boundsCenter);
    public abstract void Attack(float force);
}
