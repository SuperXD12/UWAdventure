using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying_lightbullet : MonoBehaviour
{

    public GameObject lightbulletprefab;
    public Transform firepoint;

    public float bulletForce = 10f;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("light_shot");
        StartCoroutine(SplitPhysic());
    }

    

    IEnumerator SplitPhysic() {

        
        yield return new WaitForSeconds(1);

        GameObject tempbullet_left = Instantiate(lightbulletprefab, firepoint.position, firepoint.rotation);
        GameObject tempbullet_right = Instantiate(lightbulletprefab, firepoint.position, firepoint.rotation);
        Rigidbody2D rbl = tempbullet_left.GetComponent<Rigidbody2D>();
        Rigidbody2D rbr = tempbullet_right.GetComponent<Rigidbody2D>();
        rbl.AddForce(firepoint.right * -1 * bulletForce, ForceMode2D.Impulse);
        rbr.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse);
        
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
