using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreGUI : MonoBehaviour
{
    [System.Serializable]
    public struct FStoreInfo
    {
        public string Name;
        public int Value;
        public Sprite Icon;
        public bool Owned;

        public FStoreInfo(string newName, int newValue, Sprite newIcon, bool newOwned)
        {
            Name = newName;
            Value = newValue;
            Icon = newIcon;
            Owned = newOwned;
        }
    }

    [Header("Asset's References")]
    [SerializeField]
    private StoreItemGUI _storeItem;

    [Header("References")]
    [SerializeField]
    private RectTransform _verticalLayout;
    [SerializeField]
    private Text _sellerNameText;
    [SerializeField]
    private Text _simonMoney;

    private static bool _isShowing;

    private static string _sellerName;
    private static Simon _simon;
    private static FStoreInfo[] _items;
    private static System.Action<int, StoreItemGUI> _onSell;

    public static void Initialize(string sellerName, Simon simon, FStoreInfo[] items, System.Action<int, StoreItemGUI> OnSell)
    {
        if (_isShowing) return;

        _isShowing = true;

        _sellerName = sellerName;
        _simon = simon;
        _items = items;
        _onSell = OnSell;

        _simon.SetCanMove(false);

        SceneManager.LoadScene("Store", LoadSceneMode.Additive);
    }

    private void Start()
    {
        _sellerNameText.text = _sellerName;

        for (int i = 0; i < _items.Length; i++)
        {
            StoreItemGUI reference = GameObject.Instantiate(_storeItem);

            reference.transform.SetParent(_verticalLayout);
            reference.transform.localScale = Vector3.one;

            FStoreInfo item = _items[i];
            reference.Initialize(i, item.Name, item.Value, item.Icon, item.Owned, _onSell);
        }
    }

    private void Update()
    {
        _simonMoney.text = _simon.GetInventory().currentMoney.ToString();
    }

    public void CloseStore()
    {
        Scene scene = SceneManager.GetSceneByName("Store");
        SceneManager.UnloadSceneAsync(scene);

        _isShowing = false;
        _simon.SetCanMove(true);
    }
}
