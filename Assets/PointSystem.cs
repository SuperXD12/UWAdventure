using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public GameObject text;
    private int currentpoints;
    // Start is called before the first frame update
    void Start()
    {
        currentpoints = 0;
    }


    public void Changepoints(int amount) {
        currentpoints += amount;
        text.GetComponent<TMPro.TextMeshProUGUI>().text = "Points: "+currentpoints.ToString();
    }


    public int GetPoints()
    {
        return currentpoints;

    }
}
