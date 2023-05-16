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

    public Font font;
    public GameObject pollnametext;
    public GameObject player;
    public Upgradehandling uh;
    private int Numberofenemiestospawn;
    private int numberofstrongenemies;
    public GameObject blueslime;
    public GameObject fishenemy;
    public GameObject strongerenemy;
    public EnemyData fishenemystats;
    public EnemyData strongerenemystats;
    public EnemyData darkbatstats;
    public EnemyData pinkSkullstats;
    private float currentenemyrate;
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
    private int currentvotesforboss;
    private int currentviewers;
    private System.Random rnd;
    public GameObject fishenemylabeled;
    public GameObject pinkSkullBoss;
    public GameObject darkbatenemy;
    

    private GameObject currenttobespawned;
    private int currenttobespawnednumber;
    private int numberofdarkbats;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        Numberofenemiestospawn = 3;
        numberofstrongenemies = 0;
        numberofdarkbats=0;
        rnd = new System.Random();
        currenttobespawned = darkbatenemy;
        currenttobespawnednumber = 0;
        font.material.mainTexture.filterMode = FilterMode.Point;
        currentenemyrate = 10f;
        currentviewers = 1;
        currentvotesforboss = 0;
        if ((PlayerPrefs.HasKey("highscore_points") )&& (PlayerPrefs.HasKey("highscore_waves"))){
            highwtext.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("highscore_waves").ToString();
            highptext.GetComponent<TMPro.TextMeshProUGUI>().text= PlayerPrefs.GetInt("highscore_points").ToString();

        }

        Cursor.lockState = CursorLockMode.Confined;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
        coroutine = SpawnEnemy(currentenemyrate, fishenemy,strongerenemy,darkbatenemy);
        StartCoroutine(coroutine);
        StartCoroutine(NextWave());
        float help = (1f / player.GetComponent<Weapon>().firerate);
        astext.GetComponent<TMPro.TextMeshProUGUI>().text = help.ToString();
        datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().getCurrentDamage().ToString();
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

    

    private void FixedUpdate()
    {
        string currentpollname =gameObject.GetComponent<VotingLogic>().GetCurrentPollName();
        pollnametext.GetComponent<TMPro.TextMeshProUGUI>().text =currentpollname;
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
            //fishenemystats.speed += 0.25f;
            if (numberofstrongenemies <= 100)
            numberofstrongenemies += 1;
            fishenemystats.hp += 100;
            strongerenemystats.hp += 100;
            darkbatstats.hp += 100;
            pinkSkullstats.hp += 200;
            
        }
        int u = wave % 3;
        if (u == 0) {
            if (numberofdarkbats <=100)
            numberofdarkbats += 1;
        }
        int l = wave % 10;
        if ((l == 0) && (currentenemyrate>3.5f)) {
            fishenemystats.speed += 0.5f;
            darkbatstats.speed += 0.5f;
            pinkSkullstats.speed += 0.5f;
            currentenemyrate -= 0.5f;
            Debug.Log("HIGHER ENEMY RATE");
        }

        wavetext.GetComponent<TMPro.TextMeshProUGUI>().text = "Wave: " + wave.ToString();
        //StopCoroutine(coroutine);
        int p = wave % 2;
        if ((Numberofenemiestospawn <= 100) && (p==0))
        {
            Numberofenemiestospawn = (Numberofenemiestospawn + 1);
        }
        else {
            Debug.Log("MAX ENEMIES REACHED");
        }


        //coroutine=SpawnEnemy(currentenemyrate, enemyPrefab);
        //StartCoroutine(coroutine);
        if (currentvotesforboss >= 2) { // Mathf.RoundToInt(currentviewers / 5))) { 
            currentvotesforboss = 0;
            SpawnBossMonster();
        }

        
        Debug.Log("Next Wave");
        StartCoroutine(NextWave());

    }

    public void Action_SpawnMonsterCrowd(int viewercount) {
        for (int i = 1; i < 4; i++) {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1, 360);
            Vector3 pos = RandomCircle(center, 20, a);
            GameObject tempenemy = Instantiate(currenttobespawned, pos, Quaternion.identity);
            tempenemy.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
    }

    public void Action_ChangeMonsterType(int viewercount) {
        int chosen = rnd.Next(1, 3);//1,2 //0=fish 1=seahorse 2=slime
        if (currenttobespawnednumber == 0)
        {
            
            currenttobespawnednumber = chosen;
            if (chosen == 1)
            {
                currenttobespawned = strongerenemy;
            }
            else {
                currenttobespawned = blueslime;
            }
        }
        else
        {
            if (currenttobespawnednumber == 1)
            {
                if (chosen == 1)
                {
                    currenttobespawned = darkbatenemy;
                    currenttobespawnednumber = 0;
                }
                else
                {
                    currenttobespawned = blueslime;
                    currenttobespawnednumber = 2;
                }
            }
            else
            { 
                if (chosen == 1)
                {
                    currenttobespawnednumber = 0;
                    currenttobespawned = darkbatenemy;
                }
                else
                {
                    currenttobespawnednumber = 1;
                    currenttobespawned = strongerenemy;
                }

            }
        }
    }

    public void Action_VoteforBoss(int viewercount) {
        currentvotesforboss += 1;
        currentviewers = viewercount;
    }

    public void SpawnLabeledEnemy(string name, Color color) {
        Vector3 center = player.transform.position;
        //Debug.Log("SpawnEnemy Player Center: " + center);
        int a = Random.Range(1, 360);
        Vector3 pos = RandomCircle(center, 20, a);
        GameObject tempenemy = Instantiate(fishenemylabeled, pos, Quaternion.identity);
        tempenemy.GetComponent<UiFollowEnemy>().SetName(name, color);
    }

    private void SpawnBossMonster() {

        Vector3 center = player.transform.position;
        //Debug.Log("SpawnEnemy Player Center: " + center);
        int a = Random.Range(1, 360);
        Vector3 pos = RandomCircle(center, 20, a);
        GameObject tempenemy = Instantiate(pinkSkullBoss, pos, Quaternion.identity);
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy, GameObject strongenemy, GameObject darkbat) {
        
        //the actual spawning:
        for (int x = 0; x < Numberofenemiestospawn; x++) {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1,360);
            Vector3 pos = RandomCircle(center, 15, a);
            GameObject tempenemy = Instantiate(enemy, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(interval/3f);
        for (int x = 0; x < numberofstrongenemies; x++) {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1, 360);
            Vector3 pos = RandomCircle(center, 20, a);
            GameObject tempenemy = Instantiate(strongenemy, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(interval/3f);
        for (int x = 0; x < numberofdarkbats; x++)
        {
            Vector3 center = player.transform.position;
            //Debug.Log("SpawnEnemy Player Center: " + center);
            int a = Random.Range(1, 360);
            Vector3 pos = RandomCircle(center, 20, a);
            GameObject tempenemy = Instantiate(darkbat, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(interval/3f);

        StartCoroutine(SpawnEnemy(interval, enemy, strongenemy,darkbat));
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
