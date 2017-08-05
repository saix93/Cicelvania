using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform _target;

    [Header("Values")]
    [SerializeField]
    private float _minDistanceToFollow;
    [SerializeField]
    private Vector2 _speedMovement;
    [SerializeField]
    private Rect _mapBounds;
    [SerializeField]
    private bool _bUseAspectRatio;
    [SerializeField]
    private int _divisions = 4;
    [SerializeField]
    private Vector2 _offset;

    private Vector3 _bottomLeft;
    private Vector3 _topRight;

    private Vector3 _firstLinePoint;

    private Vector3 _secondLinePoint;

    // Componentes
    private Camera _camera;

    private void Awake()
    {
        _camera = this.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 preferedTargetXPosition = new Vector3(_target.position.x + _offset.x, this.transform.position.y, -10);
        Vector3 preferedTargetYPosition = new Vector3(this.transform.position.x, _target.position.y + _offset.y, -10);

        Vector3 preferedPosition = Vector3.Lerp(this.transform.position, preferedTargetYPosition, _speedMovement.y * Time.deltaTime);

        if (_target.position.x < this.transform.position.x - Bounds()
            || _target.position.x > this.transform.position.x + Bounds())
        {
            preferedPosition = Vector3.Lerp(preferedPosition, preferedTargetXPosition, _speedMovement.x * Time.deltaTime);
        }

        _bottomLeft = new Vector3
        (
            this.transform.position.x - _camera.orthographicSize * _camera.aspect,
            this.transform.position.y - _camera.orthographicSize
        );

        _topRight = new Vector3
        (
            this.transform.position.x + _camera.orthographicSize * _camera.aspect,
            this.transform.position.y + _camera.orthographicSize
        );
        
        // Linea de la izquierda | Punto A -> Abajo | Punto B -> Arriba
        _firstLinePoint = _bottomLeft;
        _firstLinePoint.x += _minDistanceToFollow * (_bUseAspectRatio ? _camera.aspect : 1);

        // Linea de la derecha | Punto A -> Abajo | Punto B -> Arriba
        _secondLinePoint = _bottomLeft;
        _secondLinePoint.x = _topRight.x;
        _secondLinePoint.x -= _minDistanceToFollow * (_bUseAspectRatio ? _camera.aspect : 1);

        preferedPosition.x = Mathf.Clamp(preferedPosition.x, _mapBounds.x + CameraWidth(), _mapBounds.width - CameraWidth());
        preferedPosition.y = Mathf.Clamp(preferedPosition.y, _mapBounds.y + CameraHeight(), _mapBounds.height - CameraHeight());

        this.transform.position = preferedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        // Camera
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_bottomLeft, .5f);
        Gizmos.DrawSphere(_topRight, .5f);

        // Map limits
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(new Vector2(_mapBounds.x, _mapBounds.y), .5f);
        Gizmos.DrawSphere(new Vector2(_mapBounds.x, _mapBounds.height), .5f);
        Gizmos.DrawSphere(new Vector2(_mapBounds.width, _mapBounds.y), .5f);
        Gizmos.DrawSphere(new Vector2(_mapBounds.width, _mapBounds.height), .5f);

        // Bounds
        Gizmos.color = Color.blue;

        Vector3 firstLine = _bottomLeft;
        Vector3 secondLine = _bottomLeft;
        secondLine.x = _topRight.x;

        for (int i = 0; i < _divisions; i++)
        {
            float distance = (_firstLinePoint.x - _bottomLeft.x) / _divisions;
            firstLine.x += distance;
            secondLine.x -= distance;

            Gizmos.DrawLine(firstLine, new Vector3(firstLine.x, _topRight.y));
            Gizmos.DrawLine(secondLine, new Vector3(secondLine.x, _topRight.y));
        }

        Gizmos.color = Color.white;

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;

        pointA = this.transform.position + new Vector3(-Bounds(), -CameraHeight());
        pointB = this.transform.position + new Vector3(-Bounds(), CameraHeight());
        Gizmos.DrawLine(pointA, pointB);

        pointA = this.transform.position + new Vector3(Bounds(), CameraHeight());
        pointB = this.transform.position + new Vector3(Bounds(), -CameraHeight());
        Gizmos.DrawLine(pointA, pointB);
    }

    private float CameraWidth()
    {
        return _camera.orthographicSize * _camera.aspect;
    }

    private float CameraHeight()
    {
        return _camera.orthographicSize;
    }

    private float Bounds()
    {
        return CameraWidth() - _minDistanceToFollow * (_bUseAspectRatio ? _camera.aspect : 1);
    }
}
