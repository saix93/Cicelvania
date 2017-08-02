using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimation : MonoBehaviour
{
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
            Invoke("ThrowAnim", 1);
        }
    }

    private void ThrowAnim()
    {
        _anim.SetTrigger("ThrowAnim");
    }
}
