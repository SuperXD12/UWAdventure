using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public float firerate = 1f;
    public bool allowfire = false;
    public Object bullet;

    // Start is called before the first frame update
    void Start()
    {
        allowfire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowfire)
        {
            StartCoroutine(ShootingCoroutine()); 
        }
    }

    IEnumerator ShootingCoroutine() {
        allowfire = false;
        //tempbullet = Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(firerate);
        allowfire = true;
    }

}
