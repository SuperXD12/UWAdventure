using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying_blood : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("blood_shot_flying");
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
