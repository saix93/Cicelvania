using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : PickableItem
{
    [Header("ItemLife properties")]
    [SerializeField]
    private int _minAmountLife;
    [SerializeField]
    private int _maxAmountLife;

    protected override void PickItem(Simon simon)
    {
        base.PickItem(simon);

        int life = Random.Range(_minAmountLife, _maxAmountLife + 1);

        Health health = simon.GetComponent<Health>();
        health.AddLife(life);
        
        child.GetComponent<PickableItemGUI>().UpdateText(life);

        Destroy(this.gameObject);
    }
}
