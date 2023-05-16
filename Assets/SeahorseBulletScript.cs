using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeahorseBulletScript : MonoBehaviour
{
    public Animator animator;
    private GameObject seahorser;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Green_projectile");
    }

    public void setSeahorse(GameObject seahorse) {
        seahorser = seahorse;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.CompareTag("Player"))
        {
            //Debug.Log("ENEMY HIT PLAYER");
            collider.GetComponent<Weapon>().gotHit(seahorser.GetComponent<EnemyShooting>().getCurrentDamage());
            Destroy(gameObject);

        }


    }
}
