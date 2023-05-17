using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentspecialmonstertypedisplay : MonoBehaviour
{
    public GameObject gamelogic;
    public Sprite darkbat; //0
    public Sprite seahorse; //1
    public Sprite blueslime; //2
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        int currentenemy = gamelogic.GetComponent<GameLogic>().getCurrenttobespawned();
        switch (currentenemy)
        {

            case 0:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = darkbat;
                break;

            case 1:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = seahorse;
                break;

            default:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = blueslime;
                break;
        }
    }
}
