using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoney : PickableItem
{
    [Header("ItemMoney properties")]
    [SerializeField]
    private int _minAmountMoney;
    [SerializeField]
    private int _maxAmountMoney;

    protected override void PickItem(Simon simon)
    {
        base.PickItem(simon);

        int money = Random.Range(_minAmountMoney, _maxAmountMoney + 1);

        simon.GetInventory().AddMoney(money);
        
        child.GetComponent<PickableItemGUI>().UpdateText(money);
        Destroy(this.gameObject);
    }
}
