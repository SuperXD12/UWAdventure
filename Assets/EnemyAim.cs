using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{


    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = target.position - gameObject.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ-90f);
        //gameObject.transform.LookAt(target);
        //gameObject.transform.Rotate(0, -90, 0);
        //Quaternion currentrotation = gameObject.transform.rotation;

    }
}
