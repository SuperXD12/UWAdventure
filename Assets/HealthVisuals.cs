using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisuals : MonoBehaviour
{
    public Sprite hp100;
    public Sprite hp90;
    public Sprite hp80;
    public Sprite hp70;
    public Sprite hp60;
    public Sprite hp50;
    public Sprite hp40;
    public Sprite hp30;
    public Sprite hp20;
    public Sprite hp10;
    public Image healthbackground;


    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        healthbackground.sprite = hp100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int newamount) {
        switch (newamount / 1000) {
            case 9:
                healthbackground.sprite = hp90;
                break;
            case 8:
                healthbackground.sprite = hp80;
                break;
            case 7:
                healthbackground.sprite = hp70;
                break;
            case 6:
                healthbackground.sprite = hp60;
                break;
            case 5:
                healthbackground.sprite = hp50;
                break;
            case 4:
                healthbackground.sprite = hp40;
                break;
            case 3:
                healthbackground.sprite = hp30;
                break;
            case 2:
                healthbackground.sprite = hp20;
                break;
            case 1:
                healthbackground.sprite = hp10;
                break;
            default:
                break;
        }
        text.GetComponent<TMPro.TextMeshProUGUI>().text = "Health: " + (newamount/100).ToString();
    }
}
