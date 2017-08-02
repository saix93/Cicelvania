using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public const string KEY_CHECKPOINT = "KEY_CHECKPOINT";
    public const string KEY_POSITION = "KEY_POSITION";
    public const string KEY_INVENTORY = "KEY_INVENTORY";
    public const string KEY_WEAPONS = "KEY_WEAPONS";
    public const string KEY_MONEY = "KEY_MONEY";
    public const string KEY_X = "KEY_X";
    public const string KEY_Y = "KEY_Y";
    public const string KEY_SCENE = "KEY_SCENE";

    [SerializeField]
    private Vector2 _offsetSpawn;

    private Animator _animator;
    private Simon _simon;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        _simon = GameManager.GetInstance().GetSimon();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector2)this.transform.position + _offsetSpawn, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Simon simon = collision.GetComponent<Simon>();

        if (simon)
        {
            _animator.SetTrigger("ActivateCheckpoint");

            // Position
            PlayerPrefs.SetFloat(KEY_CHECKPOINT + KEY_POSITION + KEY_X, this.transform.position.x + _offsetSpawn.x);
            PlayerPrefs.SetFloat(KEY_CHECKPOINT + KEY_POSITION + KEY_Y, this.transform.position.y + _offsetSpawn.y);

            // Inventory
            Inventory inventory = simon.GetInventory();
            string values = "";
            BaseWeapon weapon = null;
            for (int i = 0; i < System.Enum.GetNames(typeof(Inventory.EWeaponList)).Length; i++)
            {
                values += inventory.GetWeapon((Inventory.EWeaponList)i, out weapon) ? 1 : 0;
            }

            // Weapons
            PlayerPrefs.SetString(KEY_CHECKPOINT + KEY_INVENTORY + KEY_WEAPONS, values);

            // Money
            PlayerPrefs.SetInt(KEY_CHECKPOINT + KEY_INVENTORY + KEY_MONEY, inventory.currentMoney);

            // Scene
            Scene scene = SceneManager.GetActiveScene();
            PlayerPrefs.SetString(KEY_CHECKPOINT + KEY_SCENE, scene.name);
        }
    }
}
