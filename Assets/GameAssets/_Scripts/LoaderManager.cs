using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    private static Vector2 _positionLoader;
    private static int[] _weaponsLoader;
    private static int[] _powerupsLoader;
    private static int _money;
    private static bool _mustLoad;

    private void Start()
    {
        if (!_mustLoad) return;

        Simon simon = GameManager.GetInstance().GetSimon();
        Inventory inventory = simon.GetInventory();

        simon.transform.position = new Vector2(_positionLoader.x, _positionLoader.y + 0.1f);
        inventory.SetMoney(_money);

        BaseWeapon weapon = null;
        for (int i = 0; i < _weaponsLoader.Length; i++)
        {
            int value = _weaponsLoader[i];

            if (value == 1)
            {
                inventory.GetWeapon((Inventory.EWeaponList)i, out weapon, true);
                inventory.AddWeapon(weapon);
            }
        }
    }

    public static void LoadLevel(string sceneName, Vector2 position, int[] weapons, int[] powerups, int money)
    {
        _positionLoader = position;
        _weaponsLoader = weapons;
        _powerupsLoader = powerups;
        _money = money;

        _mustLoad = true;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
