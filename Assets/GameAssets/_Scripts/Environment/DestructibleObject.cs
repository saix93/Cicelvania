using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField]
    private PickableItem[] _items;

    public void DestroyObject()
    {
        int index = Random.Range(0, _items.Length);
        PickableItem item = Instantiate(_items[index]);
        item.transform.parent = null;
        item.transform.position = this.transform.position + Vector3.up;

        Destroy(this.gameObject);
    }
}
