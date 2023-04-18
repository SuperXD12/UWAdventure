using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{

    public float TimeToLive = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeToLive);
    }

   
}
