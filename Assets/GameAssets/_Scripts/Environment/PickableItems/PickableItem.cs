using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [Header("PickableItem properties")]
    [SerializeField]
    protected float _frecuency = 4;
    [SerializeField]
    protected float _amplitude = .1f;

    protected float _originalYAxis;
    protected Transform child;

    protected void Start()
    {
        _originalYAxis = this.transform.position.y;
    }

    protected void Update()
    {
        float value = Mathf.Sin(Time.time * _frecuency) * _amplitude;
        float newYAxis = _originalYAxis + value;
        this.transform.position = new Vector3(this.transform.position.x, newYAxis);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();
        if (simon)
        {
            GameManager.GetInstance().AudioPickItem().Play();
            PickItem(simon);
        }
    }

    protected virtual void PickItem(Simon simon)
    {
        // Implementado en clase heredada

        child = this.transform.GetChild(0);

        child.parent = null;
        child.gameObject.SetActive(true);
        Destroy(child.gameObject, 1);
    }
}
