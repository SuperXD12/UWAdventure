using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayability : MonoBehaviour
{

    public GameObject player;
    public Sprite lightshot;
    public Sprite watershot;
    public Sprite bloodshot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Weapon weapon = player.GetComponent<Weapon>();
        int spell = weapon.spell;
        switch (spell) {

            case 0:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = bloodshot;
                break;

            case 1:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = lightshot;
                break;

            default:
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = watershot;
                break;
        }
    }
}
