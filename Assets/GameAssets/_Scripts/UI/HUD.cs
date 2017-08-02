using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Window
{
    [Header("References")]
    [SerializeField]
    private Text _lblMoney;
    [SerializeField]
    private Text _lblTime;
    [SerializeField]
    private Text _lblLevel;
    [SerializeField]
    private Image _currentItem;

    [Header("HUD Lifes")]
    [SerializeField]
    private HUDLife _playerLife;
    [SerializeField]
    private HUDLife _enemyLife;

    [Header("Variables")]
    [SerializeField] [Range(0, 1000)]
    private float _levelTime;
    
    private float _startTime;

    private Simon _simon;

    private void Start()
    {
        _simon = GameManager.GetInstance().GetSimon();

        ApplyLevel(0);
    }

    private void OnGUI()
    {
        if (!base.GetCanvas().enabled) return;

        float remainTimeFloat = _levelTime - (Time.time - _startTime);
        int remainTimeInt = (int)remainTimeFloat;

        remainTimeInt = Mathf.Clamp(remainTimeInt, 0, 9999);

        string timeToShow = "";

        for (int i = 0; i < 4 - remainTimeInt.ToString().Length; i++)
        {
            timeToShow += "0";
        }

        timeToShow += remainTimeInt.ToString();

        _lblTime.text = "TIME " + timeToShow;
    }

    public void ApplyLevel(int indexLevel)
    {
        //STAGE - 01

        indexLevel++;

        indexLevel = Mathf.Clamp(indexLevel, 1, 99);

        string level = indexLevel.ToString();

        if (level.Length < 2)
        {
            level = "0" + level;
        }

        _lblLevel.text = "STAGE - " + level;
        _startTime = Time.time;
    }

    public void ChangeCurrentItem(Sprite newSprite)
    {
        _currentItem.sprite = newSprite;
    }

    public void UpdatePlayerLife(int currentLife)
    {
        _playerLife.SetCurrentLife(currentLife);
    }

    public void UpdateEnemyLife(int currentLife)
    {
        _enemyLife.SetCurrentLife(currentLife);
    }

    public void UpdatePlayerMoney(int money)
    {
        _lblMoney.text = "MONEY-" + money;
    }
}
