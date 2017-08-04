using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : BaseCharacter
{
    [Header("Medusa")]
    [Header("Properties")]
    [SerializeField]
    private int _amountDamage;
    [SerializeField]
    private int _timeForTeleport;
    [SerializeField]
    private float _weaponForce = 10;
    [SerializeField]
    private float _timeForNextInvincibility;
    [SerializeField]
    private int _invincibilityDurationTime;
    [SerializeField]
    private int _timeToChangePhase;
    [SerializeField]
    private int _timeToShootRay;
    [SerializeField]
    private int _damageToSelfWithMirror;

    [Header("References")]
    [SerializeField]
    private Vector3[] _teleportLocations;
    [SerializeField]
    private EnemyRotationWeapon _weapon;
    [SerializeField]
    private Transform _aimPoint;
    [SerializeField]
    private GameObject _invincibilityShield;
    [SerializeField]
    private Ray _ray;

    private enum EBossPhases
    {
        Teleport,
        Pursue,
        Ray
    }

    private bool _isDead;
    private bool _canTeleport = true;
    private int _lastLife;
    private float _nextInvincibilityTime;
    private bool _checkInvincibilityTime = true;
    private EBossPhases _currentPhase;
    private float _nextPhaseChange;
    private float _nextRayAttack;
    private bool _shootingRay;
    private Vector3 _originalPosition;

    private void Start()
    {
        _lastLife = GetHealth().GetCurrentLife();
        _nextInvincibilityTime = Time.time + _timeForNextInvincibility;
        _currentPhase = EBossPhases.Teleport;
        _nextPhaseChange = Time.time + _timeToChangePhase;
        _nextRayAttack = Time.time + _timeToShootRay;
        _originalPosition = this.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        for (int i = 0; i < _teleportLocations.Length; i++)
        {
            Gizmos.DrawWireSphere(_teleportLocations[i], 0.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.transform.GetComponent<Health>();

        if (_currentPhase.Equals(EBossPhases.Pursue) && health)
        {
            health.ReduceLife(999, Health.EDamageTypes.None);
        }
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        if (Time.time > _nextPhaseChange)
        {
            ChangePhase();
        }

        if (_currentPhase.Equals(EBossPhases.Teleport))
        {
            PhaseTeleport();
        }
        else if (_currentPhase.Equals(EBossPhases.Pursue))
        {
            PhasePursue();
        }
        else if (_currentPhase.Equals(EBossPhases.Ray))
        {
            PhaseRay();
        }

        if (_checkInvincibilityTime && Time.time > _nextInvincibilityTime)
        {
            StartCoroutine(InvincibilityCoroutine());
        }

        if (_lastLife > GetHealth().GetCurrentLife())
        {
            GetAnimator().SetTrigger("Hit");
            _lastLife = GetHealth().GetCurrentLife();
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_invincibilityDurationTime);
        _checkInvincibilityTime = false;

        _invincibilityShield.SetActive(true);
        GetHealth().GiveInvincibility(_invincibilityDurationTime);

        yield return wait;

        _invincibilityShield.SetActive(false);
        _checkInvincibilityTime = true;
        _nextInvincibilityTime = Time.time + _timeForNextInvincibility;
    }

    private void ChangePhase()
    {
        int phaseIndex = Random.Range(0, System.Enum.GetNames(typeof(EBossPhases)).Length);

        if (phaseIndex == (int)_currentPhase) return;

        _currentPhase = (EBossPhases)phaseIndex;
        _nextPhaseChange = Time.time + _timeToChangePhase;
        _nextRayAttack = Time.time + _timeToShootRay;

        Quaternion qu = this.transform.rotation;
        qu.eulerAngles = Vector3.zero;
        this.transform.rotation = qu;

        GetRigidbody().velocity = Vector3.zero;
    }

    private void PhaseTeleport()
    {
        if (_canTeleport)
        {
            int index = Random.Range(0, _teleportLocations.Length);

            if (Vector3.Distance(this.transform.position, _teleportLocations[index]) < 0.1f) return;

            StartCoroutine(TeleportCoroutine(index));
        }
    }

    private void PhasePursue()
    {
        Vector3 direction = (_aimPoint.position - this.transform.position).normalized;

        Movement(direction);
    }

    private void PhaseRay()
    {
        this.transform.position = _originalPosition;

        if (!_shootingRay && Time.time > _nextRayAttack)
        {
            _shootingRay = true;
            StartCoroutine(ShootRay());
        }
    }

    private IEnumerator ShootRay()
    {
        Vector3 direction = (_aimPoint.position - this.transform.position).normalized;
        Debug.DrawRay(this.transform.position, direction, Color.red, .1f);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        yield return new WaitForSeconds(2);

        if (!_currentPhase.Equals(EBossPhases.Ray)) yield return null;

        _nextRayAttack = Time.time + _timeToShootRay;
        Debug.DrawRay(this.transform.position, direction * 20, Color.yellow, .1f);

        foreach (RaycastHit2D hit in Physics2D.RaycastAll(this.transform.position, direction, float.PositiveInfinity))
        {
            if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("BossRoomLimits") || hit.transform.CompareTag("Mirror"))
            {
                Ray ray = Instantiate(_ray, this.transform.position, this.transform.rotation);

                float distance = hit.distance;

                ray.SetDirection(direction);
                ray.SetDistance(distance);

                if (hit.transform.CompareTag("Player"))
                {
                    GameManager.GetInstance().GetSimon().GetHealth().ReduceLife(999, Health.EDamageTypes.None);
                }
                else if(hit.transform.CompareTag("Mirror"))
                {
                    yield return new WaitForSeconds(0.5f);

                    Ray backRay = Instantiate(_ray, hit.point, this.transform.rotation);

                    backRay.SetDirection((this.transform.position - (Vector3)hit.point).normalized);
                    backRay.SetDistance(distance);

                    GetHealth().ReduceLife(_damageToSelfWithMirror, Health.EDamageTypes.None);
                }

                break;
            }
        }

        _shootingRay = false;
    }

    private IEnumerator TeleportCoroutine(int index)
    {
        WaitForSeconds wait = new WaitForSeconds(_timeForTeleport / 2);

        _canTeleport = false;
        this.transform.position = _teleportLocations[index];

        yield return wait;

        AttackAfterTeleport();

        yield return wait;

        _canTeleport = true;
    }

    private void AttackAfterTeleport()
    {
        if (_isDead) return;

        EnemyRotationWeapon weapon = Instantiate(_weapon, this.transform.position, this.transform.rotation);

        weapon.SetDamage(_amountDamage);

        Vector2 direction = (_aimPoint.position - this.transform.position).normalized;

        Debug.DrawRay(this.transform.position, direction * 50, Color.red, .5f);

        weapon.GetComponent<Rigidbody2D>().AddForce(direction * _weaponForce, ForceMode2D.Impulse);
    }

    public override void OnDead()
    {
        GetRigidbody().velocity = Vector3.zero;
        _isDead = true;

        StartCoroutine(OnDeadCoroutine());
    }

    public IEnumerator OnDeadCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            yield return wait;
            GetAnimator().SetTrigger("Hit");
        }

        yield return wait;

        // Autodestrucción, también mata al player

        Destroy(this.gameObject);
    }
}
