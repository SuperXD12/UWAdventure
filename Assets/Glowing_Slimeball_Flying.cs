using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowing_Slimeball_Flying : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("New State");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Enemy>().gotHit();
            Destroy(gameObject);

        }


    }



}
