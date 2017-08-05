using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    const int MAX_LIFE = 16;

    [System.Serializable]
    struct FResistances
    {
        public EDamageTypes Type;
        public float Resistance;
    }

    [SerializeField]
    private FResistances[] _resistancesArray;

    // Datos en referencia a la vida
    // Funciones para manejar la vida
    // Resistencias
    // Evento para la muerte del personaje
    private int _currentLife;

    public enum EDamageTypes
    {
        None,
        Cutting,
        Perforating,
        Acid,
        Fire
    }

    Dictionary<EDamageTypes, float> _resistances;

    private BaseCharacter _character;

    private bool _isDead;
    private System.Action _killEvent;
    private float _invincibilityTime;

    private void Awake()
    {
        // _maxLife = _lifeContainer.transform.childCount;
        _currentLife = MAX_LIFE;
        _character = this.GetComponent<BaseCharacter>();

        _resistances = new Dictionary<EDamageTypes, float>();
        foreach (FResistances resistance in _resistancesArray)
        {
            _resistances.Add(resistance.Type, resistance.Resistance);
        }
    }

    private void Start()
    {
        _invincibilityTime = Time.time;
    }

    private void Update()
    {
        if (this.GetComponent<Simon>() && _invincibilityTime <= Time.time)
        {
            GameManager.GetInstance().GetSimon().SetInvincibilityShield(false);
        }
    }

    public bool GetIsDead()
    {
        return _isDead;
    }

    public int GetCurrentLife()
    {
        return _currentLife;
    }

    /// <summary>
    /// Añade vida al personaje
    /// </summary>
    /// <param name="amount"></param>
    public void AddLife(int amount)
    {
        _currentLife = Mathf.Clamp(_currentLife + amount, 0, MAX_LIFE);

        if (_character.GetType() == typeof(Simon))
        {
            GameManager.GetInstance().GetHud().UpdatePlayerLife(_currentLife);
        }
        else
        {
            GameManager.GetInstance().GetHud().UpdateEnemyLife(_currentLife);
        }
    }

    /// <summary>
    /// Reduce la vida del personaje
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="damageType"></param>
    public void ReduceLife(int amount, EDamageTypes damageType)
    {
        if (_invincibilityTime > Time.time) return;

        if (_currentLife <= 0 && GetIsDead())
        {
            return;
        }

        int damage = Mathf.FloorToInt(ApplyResistance(amount, damageType));

        // Simon ha recibido 10 puntos de daño de tipo perforante (Ha resistido 90 puntos de daño con una resistencia del 10%).
        string name = this.transform.root.name;

        _currentLife -= damage;
        _currentLife = Mathf.Clamp(_currentLife, 0, MAX_LIFE);

        if (_character.GetType() == typeof(Simon))
        {
            GameManager.GetInstance().GetHud().UpdatePlayerLife(_currentLife);
            GameManager.GetInstance().AudioRandomSimonHit().Play();
        }
        else
        {
            GameManager.GetInstance().GetHud().UpdateEnemyLife(_currentLife);
            GameManager.GetInstance().AudioRandomEnemyHit().Play();
        }

        float resistanceText = _resistances.ContainsKey(damageType) ? _resistances[damageType] : 0;

        //string damageString = string.Format("{0} ha recibido {1} puntos de daño de tipo {2} (Ha resistido {3} puntos de daño de un total de {4} con una resistencia del {5}% al daño {2}). Quedandose con un total de {6} puntos de vida",
        //    name,
        //    damage,
        //    damageType.ToString(),
        //    amount - damage,
        //    amount,
        //    resistanceText,
        //    _currentLife);

        //Debug.Log("<size=12><color=red>" + damageString + "</color></size>");

        if (_currentLife <= 0)
        {
            _isDead = true;
            _killEvent();
        }
    }

    public void GiveInvincibility(int seconds)
    {
        _invincibilityTime = Time.time + seconds;
    }

    public void SetKill(System.Action killEvent)
    {
        _killEvent = killEvent;
    }

    private float ApplyResistance(float amount, EDamageTypes type)
    {
        if (!_resistances.ContainsKey(type))
        {
            return amount;
        }

        return amount - (amount * _resistances[type] / 100);
    }
	
}
