using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
