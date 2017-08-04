using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Platform")]
    [SerializeField]
    private float _timeUntilEnableCollider = 0.4f;

    private BoxCollider2D _ownCollider;
    private bool _simonOnPlatform;
    private float _buttonCooler = .5f;
    private int _buttonCount;

    private void Awake()
    {
        _ownCollider = this.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Simon simon = collision.transform.GetComponent<Simon>();

        if (simon)
        {
            _simonOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Simon simon = collision.transform.GetComponent<Simon>();

        if (simon)
        {
            _simonOnPlatform = false;
        }
    }

    private void Update()
    {
        if (_simonOnPlatform && Input.GetKeyDown(KeyCode.S))
        {

            if (_buttonCooler > 0 && _buttonCount == 1)
            {
                _ownCollider.enabled = false;
                Invoke("EnableCollider", _timeUntilEnableCollider);
            }
            else
            {
                _buttonCooler = .5f;
                _buttonCount += 1;
            }
        }

        if (_buttonCooler > 0)
        {
            _buttonCooler -= 1 * Time.deltaTime;
        }
        else
        {
            _buttonCount = 0;
        }
    }

    private void EnableCollider()
    {
        _ownCollider.enabled = true;
    }
}
