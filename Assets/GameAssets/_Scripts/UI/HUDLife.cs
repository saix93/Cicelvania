using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDLife : MonoBehaviour
{
    private Text _name;
    private Image[] _imagesArray;

    [SerializeField]
    private Sprite _fullLifeSprite;
    [SerializeField]
    private Sprite _emptyLifeSprite;

    private void Awake()
    {
        _name = this.GetComponent<Text>();
        _imagesArray = this.GetComponentsInChildren<Image>();
    }

    public void SetCurrentName(string newName)
    {
        _name.text = newName;
    }

    public void SetCurrentLife(int life)
    {
        for (int i = 0; i < _imagesArray.Length; i++)
        {
            _imagesArray[i].sprite = i >= life ? _emptyLifeSprite : _fullLifeSprite;
        }
    }
}
