using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class BaseCharacter : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private Health _health;

    [Header("BaseCharacter")]
    [SerializeField]
    protected float _speedMovement = 2;
    [SerializeField]
    protected float _jumpForce = 2;
    [SerializeField]
    protected int maxJumps = 1;
    [SerializeField]
    protected float _attackRate;
    [SerializeField]
    private Transform _attackTransform;
    [SerializeField]
    private Transform _chestTransform;
    [SerializeField]
    private PickableItem[] _items;

    private float _nextTimeAttack;

    private int currentJumps = 0;

    protected virtual void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _renderer = this.GetComponent<SpriteRenderer>();

        _health = this.GetComponent<Health>();

        _health.SetKill(OnDead);
    }

    protected Animator GetAnimator()
    {
        return _animator;
    }

    public Health GetHealth()
    {
        return _health;
    }
    
    protected float GetAttackRate()
    {
        return _attackRate;
    }

    protected void DropRandomItem()
    {
        if (_items.Length == 0) return;
        
        int index = Random.Range(0, _items.Length);
        PickableItem item = Instantiate(_items[index]);
        item.transform.parent = null;
        item.transform.position = this.transform.position + Vector3.up;
    }

    public bool LookingRight()
    {
        return _renderer.flipX;
    }

    public Vector3 GetAttackPosition()
    {
        Vector3 position = _attackTransform.localPosition;

        if (LookingRight())
        {
            position.x = -position.x;
        }

        return this.transform.TransformPoint(position);
    }

    public Vector3 GetChestPosition()
    {
        return _chestTransform.position;
    }

    protected Rigidbody2D GetRigidbody()
    {
        return _rb;
    }

    protected void SimpleMovement(Vector2 axis)
    {
        // _rb.MovePosition(_rb.position + axis * Time.fixedDeltaTime);
        // _rb.AddForce(axis * _speedMovement, ForceMode2D.Force);
        _rb.velocity = new Vector2(axis.x * _speedMovement, _rb.velocity.y);

        _renderer.flipX = axis.x > 0;
        _animator.SetFloat("speed", 1);
    }

    /// <summary>
    /// Función que define el movimiento del personaje
    /// </summary>
    /// <param name="axis"></param>
    protected void Movement(Vector2 axis)
    {
        Movement(axis, axis.x > 0, 1);
    }
    protected void Movement(Vector2 axis, bool flipX)
    {
        Movement(axis, flipX, 1);
    }
    protected void Movement(Vector2 axis, bool flipX, float speedAnim)
    {
        _rb.velocity = axis * _speedMovement;

        _renderer.flipX = flipX;
        _animator.SetFloat("speed", speedAnim);
    }

    protected void StopMovement()
    {
        _animator.SetFloat("speed", 0);
    }

    protected void Jump()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Floor") | 1 << LayerMask.NameToLayer("Platform");
        bool isGrounded = Physics2D.OverlapCircle(_rb.position, 0.5f, layerMask);

        if (isGrounded || currentJumps < maxJumps)
        {
            _rb.velocity = Vector3.zero;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            if (isGrounded)
            {
                currentJumps = 0;
            }

            currentJumps++;
        }
    }

    protected virtual void Attack(int weaponIndex = 0)
    {
        _animator.SetTrigger("attack");
        _animator.SetInteger("attackIndex", weaponIndex);

        _nextTimeAttack = Time.time + _attackRate;
    }

    protected bool CanAttack()
    {
        return Time.time >= _nextTimeAttack;
    }

    public abstract void OnDead();
}
