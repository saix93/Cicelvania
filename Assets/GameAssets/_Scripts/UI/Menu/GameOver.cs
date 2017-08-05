using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        GameManager.GetInstance().AudioGameOver().Play();
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Restart()
    {
        MainMenu.LoadLevelWithSavedData();
    }
}
