using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBossManager : MonoBehaviour
{
    [Header("Medusa Boss Manager")]
    [Header("Properties")]
    [SerializeField]
    private float _timeToWaitUntilActivation;
    [SerializeField]
    private Vector3 _cameraPosition;

    [Header("References")]
    [SerializeField]
    private Medusa _medusa;
    [SerializeField]
    private Door _door;

    private bool _isMedusaAlive = true;

    private void Update()
    {
        if (_isMedusaAlive && !_medusa)
        {
            _isMedusaAlive = false;
            _door.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            GameManager.GetInstance().AudioMedusaStart().Play();
            Camera.main.GetComponent<CameraFollow>().enabled = false;
            Camera.main.transform.position = _cameraPosition;
            Invoke("EnableMedusa", _timeToWaitUntilActivation);
        }
    }

    private void EnableMedusa()
    {
        _medusa.enabled = true;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
