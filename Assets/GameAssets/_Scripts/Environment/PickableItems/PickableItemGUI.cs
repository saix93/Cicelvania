using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableItemGUI : MonoBehaviour
{
    [Header("PickableItemGUI properties")]
    [SerializeField]
    private float _floatingTextSpeed = 1;

    private Text _text;

    private void Awake()
    {
        _text = this.transform.Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
        this.transform.position += Vector3.up * _floatingTextSpeed * Time.deltaTime;
    }

    public void UpdateText(int amount)
    {
        _text.text = "+" + amount;
    }

    public void UpdateText(string newText)
    {
        _text.text = newText;
    }
}
