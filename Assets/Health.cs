using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
    public int health = 100;
    private int MAX_HEALTH = 100;
    public GameObject gameLogic;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int maxHealth, int health)
    {
        this.MAX_HEALTH = maxHealth;
        this.health = health;
    }

    // Added for Visual Indicators
    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        
        this.health -= amount;
        //Debug.Log("Dealt " + amount + " damage to " + gameObject.tag + "\n New Health of Object is " + this.health.ToString());
        if (gameObject.tag == "Player") {
            gameObject.GetComponent<HealthVisuals>().UpdateHealth(this.health);
        }
        StartCoroutine(VisualIndicator(Color.red)); // Added for Visual Indicators

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;
        StartCoroutine(VisualIndicator(Color.green)); // Added for Visual Indicators

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        //Debug.Log("I am Dead!");
        if (gameObject.tag == "Enemy") 
        {
            gameObject.GetComponent<Enemy>().EnemyDied();
        }
        
        if (gameObject.tag == "Player")
        {


            Debug.Log(gameLogic.GetComponent<PointSystem>().GetPoints());
            int currentpoints = gameLogic.GetComponent<PointSystem>().GetPoints();
            int currentwaves = gameLogic.GetComponent<GameLogic>().GetWave();
            if (PlayerPrefs.HasKey("highscore_points"))
            {
                if (PlayerPrefs.GetInt("highscore_points") < currentpoints)
                {
                    PlayerPrefs.SetInt("highscore_points", currentpoints);
                    PlayerPrefs.Save();
                }
            }
            else {
                PlayerPrefs.SetInt("highscore_points", currentpoints);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.HasKey("highscore_waves"))
            {
                if (PlayerPrefs.GetInt("highscore_waves") < currentwaves)
                {
                    PlayerPrefs.SetInt("highscore_waves", currentwaves);
                    PlayerPrefs.Save();
                }
            }
            else {

                PlayerPrefs.SetInt("highscore_waves", currentwaves);
                PlayerPrefs.Save();
            }

            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else {
            Destroy(gameObject);
        }
        

    }
}
