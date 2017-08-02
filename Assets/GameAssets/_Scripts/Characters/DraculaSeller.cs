using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraculaSeller : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private string sellerName;

    [SerializeField]
    private StoreGUI.FStoreInfo[] _items;

    private bool _isSimonAtRange;
    private Simon _simon;
    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        _simon = GameManager.GetInstance().GetSimon();
    }

    private void Update()
    {
        if (_isSimonAtRange && Input.GetKeyDown(KeyCode.E))
        {
            StoreGUI.Initialize(sellerName, _simon, _items, OnSell);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            _isSimonAtRange = true;
            simon.SetActiveCanvas(true);

            _animator.SetBool("IsSimonNear", _isSimonAtRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            _isSimonAtRange = false;
            simon.SetActiveCanvas(false);

            _animator.SetBool("IsSimonNear", _isSimonAtRange);
        }
    }

    private void OnSell(int index, StoreItemGUI referenceItem)
    {
        StoreGUI.FStoreInfo item = _items[index];

        if (_simon.GetInventory().currentMoney >= item.Value)
        {
            BaseWeapon weapon = null;
            if (!_simon.GetInventory().GetWeapon((Inventory.EWeaponList)index, out weapon))
            {
                _simon.GetInventory().SubstractMoney(item.Value);
                _simon.GetInventory().AddWeapon(weapon);

                _items[index].Owned = true;
                referenceItem.tick.gameObject.SetActive(true);
                referenceItem.buyButton.interactable = false;
            }
        }
    }
}
