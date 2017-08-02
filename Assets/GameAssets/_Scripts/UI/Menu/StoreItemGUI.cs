using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemGUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _value;
    [SerializeField]
    private Image _icon;

    public Button buyButton;
    public Transform tick;

    [Header("Money Colors")]
    [SerializeField]
    private Color _availableColor;
    [SerializeField]
    private Color _unavaliableColor;

    private Inventory _inventory;
    private int _itemValue;

    private void Start()
    {
        _inventory = GameManager.GetInstance().GetSimon().GetInventory();
    }

    public void Initialize(int index, string newItemName, int newValue, Sprite newIcon, bool newOwned, System.Action<int, StoreItemGUI> OnSell)
    {
        _name.text = newItemName;
        _itemValue = newValue;
        _value.text = newValue.ToString();
        _icon.sprite = newIcon;

        tick.gameObject.SetActive(newOwned);
        buyButton.interactable = !newOwned;

        this.GetComponentInChildren<Button>().onClick.AddListener(() => OnSell(index, this));
    }

    private void Update()
    {
        _value.color = _inventory.currentMoney < _itemValue ? _unavaliableColor : _availableColor;
    }
}
