using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
using UnityEngine;
//using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    public GameObject player;
    public Upgradehandling uh;
    private int Numberofenemiestospawn = 5;
    private int numberofstrongenemies = 0;
    public GameObject blueslime;
    public GameObject fishenemy;
    public GameObject strongerenemy;
    public EnemyData fishenemystats;
    public EnemyData strongerenemystats;
    private float currentenemyrate = 10f;
    private int wave = 1;
    public GameObject wavetext;
    public GameObject astext;
    public GameObject datext;
    public GameObject mstext;
    private bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject overlay;
    public GameObject upgradeMenu;
    public GameObject highwtext;
    public GameObject highptext;

    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {

        if ((PlayerPrefs.HasKey("highscore_points") )&& (PlayerPrefs.HasKey("highscore_waves"))){
            highwtext.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("highscore_waves").ToString();
            highptext.GetComponent<TMPro.TextMeshProUGUI>().text= PlayerPrefs.GetInt("highscore_points").ToString();

        }

        Cursor.lockState = CursorLockMode.Confined;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        coroutine = SpawnEnemy(currentenemyrate, fishenemy,strongerenemy);
        StartCoroutine(coroutine);
        StartCoroutine(NextWave());
        float help = (1f / player.GetComponent<Weapon>().firerate);
        astext.GetComponent<TMPro.TextMeshProUGUI>().text = help.ToString();
        datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().currentdamage.ToString();
        mstext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<playermovement>().mSpeed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public int GetWave() {
        return wave;
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        overlay.SetActive(false);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        overlay.SetActive(true);
        upgradeMenu.SetActive(false);
    }



    private IEnumerator NextWave() {
        yield return new WaitForSeconds(30f);
        //Next Wave:
        wave += 1;
        uh.GainUpgradePoint();
        int k = wave % 5;
        if (k == 0) {
            fishenemystats.speed += 0.25f;
            numberofstrongenemies = wave / 5;
        }
        int l = wave % 10;
        if ((l == 0) && (currentenemyrate>3.5f)) {
            fishenemystats.hp += 100;
            strongerenemystats.hp += 100;
            currentenemyrate -= 0.5f;
            Debug.Log("HIGHER ENEMY RATE");
        }

        wavetext.GetComponent<TMPro.TextMeshProUGUI>().text = "Wave: " + wave.ToString();
        //StopCoroutine(coroutine);
        if (Numberofenemiestospawn <= 100)
        {
            Numberofenemiestospawn = (Numberofenemiestospawn + 1);
        }
        else {
            Debug.Log("MAX ENEMIES REACHED");
        }

        //coroutine=SpawnEnemy(currentenemyrate, enemyPrefab);
        //StartCoroutine(coroutine);
        Debug.Log("Next Wave");
        StartCoroutine(NextWave());

    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy, GameObject strongenemy) {
        
        //the actual spawning:
        for (int x = 0; x < Numberofenemiestospawn; x++) {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1,360);
            Vector3 pos = RandomCircle(center, 20, a);
            GameObject tempenemy = Instantiate(enemy, pos, Quaternion.identity);
        }

        for (int x = 0; x < numberofstrongenemies; x++) {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1, 360);
            Vector3 pos = RandomCircle(center, 10, a);
            GameObject tempenemy = Instantiate(strongenemy, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(interval);
        StartCoroutine(SpawnEnemy(interval, enemy, strongenemy));
    }

    private Vector3 RandomCircle(Vector3 center, float radius, int a) {

        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = 0;
        return pos;
    }
}
