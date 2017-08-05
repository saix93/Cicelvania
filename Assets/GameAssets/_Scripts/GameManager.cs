using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("HUD")]
    [SerializeField]
    private HUD _hud;
    [SerializeField]
    private Simon _simon;

    [Header("References")]
    [SerializeField]
    private AudioSource _audioGameOver;
    [SerializeField]
    private AudioSource _audioGateOpen;
    [SerializeField]
    private AudioSource _audioPickItem;
    [SerializeField]
    private AudioSource _audioMedusaStart;
    [SerializeField]
    private AudioSource _audioMedusaBeamStart;
    [SerializeField]
    private AudioSource _audioMedusaBeam;
    [SerializeField]
    private AudioSource _audioMedusaSnakeThrow;

    [SerializeField]
    private AudioSource[] _audiosSimonHit;
    [SerializeField]
    private AudioSource[] _audiosShieldHit;
    [SerializeField]
    private AudioSource[] _audiosEnemyHit;

    private static GameManager _instance;
    private static Inventory _inventory;

    private void Awake()
    {
        _instance = this;
        // DontDestroyOnLoad(this);
    }

    public AudioSource AudioGameOver()
    {
        return _audioGameOver;
    }
    public AudioSource AudioGateOpen()
    {
        return _audioGateOpen;
    }
    public AudioSource AudioPickItem()
    {
        return _audioPickItem;
    }
    public AudioSource AudioMedusaStart()
    {
        return _audioMedusaStart;
    }
    public AudioSource AudioMedusaBeamStart()
    {
        return _audioMedusaBeamStart;
    }
    public AudioSource AudioMedusaBeam()
    {
        return _audioMedusaBeam;
    }
    public AudioSource AudioMedusaSnakeThrow()
    {
        return _audioMedusaSnakeThrow;
    }
    public AudioSource AudioRandomSimonHit()
    {
        return _audiosSimonHit[Random.Range(0, _audiosSimonHit.Length)];
    }
    public AudioSource AudioRandomShieldHit()
    {
        return _audiosShieldHit[Random.Range(0, _audiosShieldHit.Length)];
    }
    public AudioSource AudioRandomEnemyHit()
    {
        return _audiosEnemyHit[Random.Range(0, _audiosEnemyHit.Length)];
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public HUD GetHud()
    {
        return _hud;
    }

    public Simon GetSimon()
    {
        return _simon;
    }

    public static void SaveInventory(Inventory newInventory)
    {
        _inventory = newInventory;
    }

    public static Inventory GetInventory()
    {
        return _inventory;
    }
}
