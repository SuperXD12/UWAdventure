using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject hwamount;
    public GameObject hpamount;

    public void Start()
    {
        if (PlayerPrefs.HasKey("highscore_waves") && PlayerPrefs.HasKey("highscore_points")) {
            hwamount.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("highscore_waves").ToString();
            hpamount.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("highscore_points").ToString();
        }
        
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("QUITTED");
        Application.Quit();
    }
}
