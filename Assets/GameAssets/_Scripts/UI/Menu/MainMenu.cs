using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    private Text[] _options;
    [SerializeField]
    private Color _selectedColor;
    [SerializeField]
    private Color _unselectedColor;
    [SerializeField]
    private Color _blinkColor;
    [SerializeField]
    private float _timeBetweenBlinks;

    private int _currentIndex;
    private Text _selectedText;
    private float _blinkTime;

    public static void LoadLevelWithSavedData()
    {
        string sceneName = PlayerPrefs.GetString(Checkpoint.KEY_CHECKPOINT + Checkpoint.KEY_SCENE, "NULL");

        if (sceneName == "NULL")
        {
            //TODO: Feedback "No existe la escena"
            Debug.Log("La escena indicada no existe");
        }
        else
        {
            string keyX = Checkpoint.KEY_CHECKPOINT + Checkpoint.KEY_POSITION + Checkpoint.KEY_X;
            string keyY = Checkpoint.KEY_CHECKPOINT + Checkpoint.KEY_POSITION + Checkpoint.KEY_Y;
            string keyWeaponValues = Checkpoint.KEY_CHECKPOINT + Checkpoint.KEY_INVENTORY + Checkpoint.KEY_WEAPONS;

            Vector2 position = new Vector2()
            {
                x = PlayerPrefs.GetFloat(keyX),
                y = PlayerPrefs.GetFloat(keyY)
            };

            // Load Weapons
            string weaponValues = PlayerPrefs.GetString(keyWeaponValues);
            int[] weapons = new int[weaponValues.Length];

            for (int i = 0; i < weaponValues.Length; i++)
            {
                weapons[i] = int.Parse(weaponValues[i].ToString());
            }

            // Money
            int money = PlayerPrefs.GetInt(Checkpoint.KEY_CHECKPOINT + Checkpoint.KEY_INVENTORY + Checkpoint.KEY_MONEY);

            LoaderManager.LoadLevel(sceneName, position, weapons, null, money);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (--_currentIndex < 0) _currentIndex = 3;
            MoveCursor();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (++_currentIndex > 3) _currentIndex = 0;
            MoveCursor();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            switch(_currentIndex)
            {
                case 0: SceneManager.LoadScene("Level1"); break;
                // case 0: Map.ShowMap(0, 1); break;
                case 1: MainMenu.LoadLevelWithSavedData(); break;
                case 2: Settings.ShowSettings(Settings.MODE_MAINMENU); break;
                case 3: QuitGame(); break;
            }
        }

        if (_selectedText && Time.time > _blinkTime)
        {
            _blinkTime = Time.time + _timeBetweenBlinks;
            _selectedText.color = _selectedText.color == _selectedColor ? _blinkColor : _selectedColor;
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    private void MoveCursor()
    {
        for (int i = 0; i < _options.Length; i++)
        {
            if (i == _currentIndex)
            {
                _blinkTime = Time.time;
                _selectedText = _options[i];
                _selectedText.transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                _options[i].color = _unselectedColor;
                _options[i].transform.localScale = Vector3.one;
            }
        }
    }
}
