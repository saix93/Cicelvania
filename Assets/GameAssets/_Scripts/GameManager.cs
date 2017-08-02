using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("HUD")]
    [SerializeField]
    private HUD _hud;
    [SerializeField]
    private Simon _simon;

    private static GameManager _instance;

    private void Awake()
    {
        _instance = this;
        // DontDestroyOnLoad(this);
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
}
