using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInvincible : PickableItem
{
    [Header("ItemLife properties")]
    [SerializeField]
    private int _invincibilityTime;

    protected override void PickItem(Simon simon)
    {
        base.PickItem(simon);

        Health health = simon.GetComponent<Health>();
        health.GiveInvincibility(_invincibilityTime);
        simon.SetInvincibilityShield(true);

        child.GetComponent<PickableItemGUI>().UpdateText("+INV");

        Destroy(this.gameObject);
    }
}
