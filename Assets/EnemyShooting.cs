using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    private bool allowfire;
    private float firerate;
    private float bulletForce;
    public Transform firepoint;
    
    private int currentdamage;
    public GameObject bullettype;
    // Start is called before the first frame update
    void Start()
    {
        firerate = 2f;
        bulletForce = 10f;
        //votinglength = 30000;
        allowfire = true;
        currentdamage = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowfire)
        {

            StartCoroutine(ShootingCoroutine());

        }
    }

    public int getCurrentDamage() {
        return currentdamage;
    }

    IEnumerator ShootingCoroutine()
    {

        allowfire = false;

        yield return new WaitForSeconds(firerate);
        GameObject tempbullet = Instantiate(bullettype, firepoint.position, firepoint.rotation);
            tempbullet.GetComponent<SeahorseBulletScript>().setSeahorse(gameObject);
            Rigidbody2D brb = tempbullet.GetComponent<Rigidbody2D>();
            brb.AddForce(firepoint.up * bulletForce, ForceMode2D.Impulse);
            
        
        

        
        allowfire = true;
    }
}
