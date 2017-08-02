using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Slider _generalVolumeSlider;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Selectable _firstSelect;

    [Space(20)]
    [Header("Camera")]
    [SerializeField]
    private Camera _selfCamera;

    public const int MODE_MAINMENU = 0x0000;
    public const int MODE_GAME = 0x0001;

    private static int Mode;

    private static bool _isShowing;

    private void Awake()
    {
        _generalVolumeSlider.value = AudioListener.volume;
        _generalVolumeSlider.onValueChanged.AddListener((float newVolume) => { AudioListener.volume = newVolume; });

        _exitButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        _firstSelect.Select();

        if (FindObjectsOfType<Camera>().Length == 0)
        {
            _selfCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public static void ShowSettings(int mode)
    {
        if (_isShowing) return;

        Mode = mode;
        _isShowing = true;

        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    private void Close()
    {
        _isShowing = false;
        Time.timeScale = 1;

        if (Mode == MODE_GAME)
        {
            StartCoroutine(Close_Coroutine());
        }
        else if (Mode == MODE_MAINMENU)
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator Close_Coroutine()
    {
        yield return SceneManager.UnloadSceneAsync(2);
    }
}
