using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int damage = 5;
    private float speed;
    [SerializeField]
    private EnemyData data;


    private GameObject player;
    private GameObject gamelogic;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gamelogic = GameObject.FindGameObjectWithTag("GameLogic");
        SetEnemyValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Swarm();
    }

    public void SetEnemyValues() {
        GetComponent<Health>().SetHealth(data.hp, data.hp);
        damage = data.damage;
        speed = data.speed;
    }

    private void Swarm()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void gotHit() {
        int damagetobedone =player.GetComponent<Weapon>().currentdamage;
        //print("Enemy got hit with " + damagetobedone);
        this.GetComponent<Health>().Damage(damagetobedone);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<Health>() != null)
            {
                collider.GetComponent<Health>().Damage(damage);
                this.GetComponent<Health>().Damage(10000);
            }
        }

        
    }
    public void EnemyDied() 
    {
        gamelogic.GetComponent<PointSystem>().Changepoints(data.points);
    }
}
