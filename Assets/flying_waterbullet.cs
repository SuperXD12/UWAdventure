using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying_waterbullet : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("water_shot");
    }

    // Update is called once per frame
    void Update()
    {
        
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
