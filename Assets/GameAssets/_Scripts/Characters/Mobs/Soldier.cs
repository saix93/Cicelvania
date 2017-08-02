using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCharacter
{
    [Header("Shielded Soldier")]
    [Header("Properties")]
    [SerializeField]
    private float _distanceLeft;
    [SerializeField]
    private float _distanceRight;

    private bool _isLookingLeft;
    private float _leftXPos;
    private float _rightXPos;

    protected override void Awake()
    {
        base.Awake();

        GetAnimator().SetLayerWeight(4, 1);
    }

    private void Start()
    {
        _leftXPos = this.transform.position.x - _distanceLeft;
        _rightXPos = this.transform.position.x + _distanceRight;
    }

    private void FixedUpdate()
    {
        if (this.transform.position.x < _leftXPos || this.transform.position.x > _rightXPos)
        {
            _isLookingLeft = !_isLookingLeft;
        }

        SimpleMovement(_isLookingLeft ? Vector2.right : -Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Simon simon = collision.transform.GetComponent<Simon>();

        if (simon)
        {
            simon.GetHealth().ReduceLife(999, Health.EDamageTypes.None);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(this.transform.position.x - _distanceLeft, 0, 0), .5f);
        Gizmos.DrawSphere(new Vector3(this.transform.position.x + _distanceRight, 0, 0), .5f);
    }

    public override void OnDead()
    {
        DropRandomItem();
        Destroy(this.gameObject);
    }
}
