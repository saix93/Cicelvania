using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _canvas;
    [SerializeField]
    private Text _text;

    [Header("Properties")]
    [SerializeField]
    private string _signText;

    private bool _isSimonAtRange;

    private void Start()
    {
        _text.text = _signText;
    }

    private void Update()
    {
        if (_isSimonAtRange && Input.GetKeyDown(KeyCode.E))
        {
            _canvas.SetActive(!_canvas.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();
        if (simon)
        {
            _isSimonAtRange = true;
            simon.SetActiveCanvas(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();
        if (simon)
        {
            _isSimonAtRange = false;
            _canvas.SetActive(false);
            simon.SetActiveCanvas(false);
        }
    }
}
