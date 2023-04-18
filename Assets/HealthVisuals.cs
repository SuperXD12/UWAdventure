using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVisuals : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int newamount) {

        text.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: " + newamount.ToString();
    }
}
