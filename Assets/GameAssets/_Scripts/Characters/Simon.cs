using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Inventory
{
    private Dictionary<BaseWeapon, bool> _items;

    public enum EWeaponList
    {
        Whip,
        Dagger,
        Axe,
        Boomerang,
        Ball
    }

    private int _currentIndex;

    public int currentMoney;

    public int InventoryLength
    {
        get; private set;
    }

    public void Initialize()
    {
        _items = new Dictionary<BaseWeapon, bool>();
        _currentIndex = 0;

        BaseWeapon[] weaponArray = Resources.LoadAll<BaseWeapon>("Weapons");
        int i = 0;
        foreach (BaseWeapon weapon in weaponArray)
        {
            _items.Add(weapon, i == 0);
            i++;
            InventoryLength++;
        }
    }

    public void AddMoney(int money)
    {
        currentMoney += money;
    }

    public void SubstractMoney(int money)
    {
        currentMoney -= money;
    }

    public void SetMoney(int money)
    {
        currentMoney = money;
    }

    public void AddWeapon(BaseWeapon weaponToAdd)
    {
        _items[weaponToAdd] = true;
    }

    public void RemoveWeapon(BaseWeapon weaponToAdd)
    {
        _items[weaponToAdd] = false;
    }

    public bool GetWeapon(EWeaponList weaponName, out BaseWeapon weapon, bool forceGive = false)
    {
        KeyValuePair<BaseWeapon, bool> result = _items.ElementAt((int)weaponName);
        weapon = result.Key;

        return result.Value || forceGive;
    }
}

public class Simon : BaseCharacter
{
    public bool ALL_WEAPONS;
    public bool GOD_MODE;

    [Header("Simon")]
    [SerializeField]
    [Range(1, 10)]
    private float _runSpeedMultiplier = 2;
    [SerializeField]
    private GameObject _invincibilityShield;
    [SerializeField]
    private float _timeToChangeWeapon = .5f;

    private Inventory _inventory;
    private bool _canMove;
    private BaseWeapon _currentWeapon;
    private float _horizontalAxis;
    private bool _isChangingWeapon;
    private BackgroundParallax _backgroundParallax;

    private GameObject _canvas;

    protected override void Awake()
    {
        base.Awake();

        if (GameManager.GetInventory() != null)
        {
            _inventory = GameManager.GetInventory();
        }
        else
        {
            _inventory = new Inventory();
            _inventory.Initialize();
        }

        _canvas = this.transform.Find("Canvas").gameObject;

        _inventory.GetWeapon(Inventory.EWeaponList.Whip, out _currentWeapon);
    }

    private void Start()
    {
        _backgroundParallax = FindObjectOfType<BackgroundParallax>();
        SetCanMove(true);
    }

    private void OnGUI()
    {
        int character = Event.current.character;

        // Del 1 al 5
        if (character >= 49 && character <= 53)
        {
            int number = character - 49;
            StartCoroutine(ChangeWeapon(number));
        }
    }

    private IEnumerator ChangeWeapon(int index)
    {
        if (_isChangingWeapon) yield return null;

        Inventory.EWeaponList weaponName = (Inventory.EWeaponList)index;

        BaseWeapon weapon = null;
        if (_inventory.GetWeapon(weaponName, out weapon, ALL_WEAPONS))
        {
            _isChangingWeapon = true;

            yield return new WaitForSeconds(_timeToChangeWeapon);

            _currentWeapon = weapon;
            _isChangingWeapon = false;

            GameManager.GetInstance().GetHud().ChangeCurrentItem(_currentWeapon.GetSprite());
        }
    }

    private void Update()
    {
        GameManager.GetInstance().GetHud().UpdatePlayerMoney(_inventory.currentMoney);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            Settings.ShowSettings(Settings.MODE_GAME);
        }

        if (!_canMove)
        {
            return;
        }

        _horizontalAxis = Input.GetAxis("Horizontal");

        _horizontalAxis = Input.GetKey(KeyCode.LeftShift) ? _horizontalAxis * _runSpeedMultiplier : _horizontalAxis;

        if (_backgroundParallax)
        {
            _backgroundParallax.speedMultiplier = _horizontalAxis;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            int index = _currentWeapon.GetType().Equals(typeof(Whip)) ? 0 : 1;
            Attack(index);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.right * _horizontalAxis;
        
        if (_horizontalAxis != 0)
        {
            SimpleMovement(velocity);
        }
        else
        {
            StopMovement();
        }
    }

    protected override void Attack(int weaponIndex = 0)
    {
        if (_isChangingWeapon || !CanAttack()) return;

        base.Attack(weaponIndex);

        BaseWeapon weapon = Instantiate(_currentWeapon);
        float direction = LookingRight() ? 1 : -1;
        // float width = GetComponent<SpriteRenderer>().bounds.center.x + Mathf.Abs(direction);

        Vector3 weaponPosition = GetAttackPosition();

        weapon.Initialize(this.GetComponent<CapsuleCollider2D>());
        weapon.transform.position = weaponPosition;
        weapon.Attack(direction);
    }

    public override void OnDead()
    {
        if (GOD_MODE) return;
        
        SceneManager.LoadScene("GameOver");
    }

    public bool GetCanMove()
    {
        return _canMove;
    }

    public void SetCanMove(bool b)
    {
        _canMove = b;
        _horizontalAxis = 0;
    }

    public void SetInvincibilityShield(bool active)
    {
        _invincibilityShield.SetActive(active);
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public void SetActiveCanvas(bool active)
    {
        _canvas.SetActive(active);
    }
}
