using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating_Shuriken_Split : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D boxcollider;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("New State");
        StartCoroutine(ActivateCollision());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ActivateCollision() {
        yield return new WaitForSeconds(0.1f);
        boxcollider.enabled = true;
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
