using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Door")]
    [SerializeField]
    private int _id;
    [SerializeField][Range(1,6)]
    private int _connectedLevel = 1;
    [SerializeField]
    private bool _isClosed;

    private Animator _anim;
    private Simon _simon;

    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
    }

    private void Start()
    {
        if (_isClosed)
        {
            _anim.SetBool("IsClosed", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isClosed) return;

        Simon simon = collision.GetComponent<Simon>();
        _simon = simon;

        if (_simon)
        {
            StartCoroutine(OpenDoor());
            Invoke("StopSimon", 0.5f);
        }
    }

    private void StopSimon()
    {
        _simon.SetCanMove(false);
    }

    private IEnumerator OpenDoor()
    {
        _anim.SetTrigger("OpenDoor");

        StopSimon();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Level" + _connectedLevel);

        //TODO: Utilizar ID para colocar el personaje en una puerta concreta al iniciar el nivel
    }
}
