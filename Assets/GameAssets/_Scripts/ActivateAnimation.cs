using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimation : MonoBehaviour
{
    [Header("Elevator")]
    [SerializeField]
    private GameObject _siblingCollider;

    private Animator _anim;

    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            _siblingCollider.SetActive(true);
            Invoke("ThrowAnim", .5f);
        }
    }

    private void ThrowAnim()
    {
        _anim.SetTrigger("ThrowAnim");
    }
}
