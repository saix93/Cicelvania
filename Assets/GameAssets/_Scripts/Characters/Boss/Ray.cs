using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    [Header("Ray")]
    [SerializeField]
    private float _lifeTime = 0.5f;
    [SerializeField]
    private SpriteRenderer _rayStart;
    [SerializeField]
    private SpriteRenderer _rayMiddle;
    [SerializeField]
    private SpriteRenderer _rayEnd;

    private Vector3 direction;
    private float distance;

    private void Start()
    {
        Destroy(this.gameObject, _lifeTime);

        Vector3 rayMiddlePosition = _rayMiddle.transform.localPosition;
        rayMiddlePosition.x += _rayStart.bounds.size.x - .01f;
        _rayMiddle.transform.localPosition = rayMiddlePosition;

        Vector3 rayMiddleScale = _rayMiddle.transform.localScale;
        rayMiddleScale.x = distance - _rayStart.bounds.size.x - _rayEnd.bounds.size.x;
        _rayMiddle.transform.localScale = rayMiddleScale;

        Vector3 rayEndPosition = _rayEnd.transform.localPosition;
        rayEndPosition.x += _rayStart.bounds.extents.x + _rayMiddle.bounds.size.x - .01f;
        _rayEnd.transform.localPosition = rayEndPosition;
        
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    public void SetDistance(float newDistance)
    {
        distance = newDistance;
    }
}
