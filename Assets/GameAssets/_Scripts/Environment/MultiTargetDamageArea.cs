using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultiTargetDamageArea : MonoBehaviour
{

    [SerializeField]
    private Health.EDamageTypes _damageType;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _damageRate;
    
    private Dictionary<BaseCharacter, float> _characterDic = new Dictionary<BaseCharacter, float>();

    private SpriteRenderer _renderer;
    private Vector3 pointA;
    private Vector3 pointB;

    private void Awake()
    {
        _renderer = this.GetComponent<SpriteRenderer>();

        Vector3 extents = _renderer.bounds.extents;

        pointA = this.transform.position + (Vector3.right * extents.x) + (Vector3.up * extents.y);
        pointB = this.transform.position + (-Vector3.right * extents.x) + (-Vector3.up * extents.y);
    }

    private void Update()
    {
        List<BaseCharacter> currentFrameCharacterList = new List<BaseCharacter>();

        foreach (Collider2D c in Physics2D.OverlapAreaAll(pointA, pointB))
        {
            BaseCharacter character = c.GetComponent<BaseCharacter>();

            if (character)
            {
                currentFrameCharacterList.Add(character);

                if (!_characterDic.ContainsKey(character))
                {
                    _characterDic.Add(character, Time.time + _damageRate);
                }
            }
        }

        // Utiliza System.Linq para poder utilizar la función ElementAt
        for (int i = 0; i < _characterDic.Count; i++)
        {
            KeyValuePair<BaseCharacter, float> c = _characterDic.ElementAt(i);

            if (!currentFrameCharacterList.Contains(c.Key))
            {
                _characterDic.Remove(c.Key);
                continue;
            }

            if (Time.time > c.Value)
            {
                Health characterHealth = c.Key.GetHealth();

                characterHealth.ReduceLife(_damage, _damageType);

                int currentLife = characterHealth.GetCurrentLife();

                if (currentLife > 0)
                {
                    _characterDic[c.Key] = Time.time + _damageRate;
                }
                else
                {
                    _characterDic.Remove(c.Key);
                }
            }
        }
    }
}
