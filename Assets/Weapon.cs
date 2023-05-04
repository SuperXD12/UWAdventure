using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
//using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

/*[Serializable]
public class VotingsO {
    public string id;
    public int votes;
} */



public class Weapon : MonoBehaviour
{
    

    public GameObject text;

    public GameObject bloodbulletPrefab;
    public GameObject lightbulletPrefab;
    public GameObject waterbulletPrefab;
    public Transform firepoint_right;
    public aim rightfirepoint;
    public aim leftfirepoint;
    public Transform firepoint_left;
    public GameObject player;

    public int spell; //0=blood, 1=light, >1=water
    public float firerate;
    public bool allowfire = true;
    public bool voted = true;
    public playermovement movescript;
    public GameObject datext;
    public GameObject astext;

    public float bulletForce = 10f;
    public int votinglength = 30000;
    public int currentdamage = 500;
    public int basedamage = 500;
    private System.Random rnd;

    private Transform currfirepoint;
    void Start() {
        firerate = 1f;
        spell = 0;
        bulletForce = 10f;
        votinglength = 30000;
        currentdamage = 500;
        basedamage = 500;
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        float helpspeed = movescript.helpspeed;
        float helpvertical = movescript.helpvertical;
        if (helpspeed > 0.01f)
        {
            if (helpvertical <= -1f) // moving left
            {
                currfirepoint = firepoint_left;
                leftfirepoint.ChosenFirepoint();
                rightfirepoint.NotChosenFirepoint();
            }
            else
            {
                if (helpvertical >= 1f) // moving right
                {
                    currfirepoint = firepoint_right;
                    rightfirepoint.ChosenFirepoint();
                    leftfirepoint.NotChosenFirepoint();
                }
                else
                { // moving upwards/downwards
                    currfirepoint = firepoint_left;
                    leftfirepoint.ChosenFirepoint();
                    rightfirepoint.NotChosenFirepoint();
                }
            }
        }
        else
        { //standing
            currfirepoint = firepoint_left;
            leftfirepoint.ChosenFirepoint();
            rightfirepoint.NotChosenFirepoint();
        }



        if (Input.GetButtonDown("1")) {
            spell = 0;
            //text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";
        }
        if (Input.GetButtonDown("2"))
        {
            spell = 1;
            //text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";
        }
        if (Input.GetButtonDown("3"))
        {
            spell = 2;
            //text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";

        }

        

       

        if (allowfire)
        {
            
            StartCoroutine(ShootingCoroutine());
            
        }
    }

    public void ExecuteAction_ChangeWeapon() {
        int chosen = rnd.Next(1, 3);
        if (spell == 0)
        {
            spell = chosen;
        }
        else {
            if (spell == 1)
            {
                if (chosen == 1)
                {
                    spell = 0;
                }
                else
                {
                    spell = 2;
                }
            }
            else { //spell==2
                if (chosen == 1)
                {
                    spell = 0;
                }
                else {
                    spell = 1;
                }
            
            }
        }
        Debug.Log("Action Executed: Changed weapon to " + spell + " with rnd=" + rnd);
    }

    public void IncreaseDamage(int amount) {
        this.basedamage += amount;
        switch (spell)
        {
            case 0:
                currentdamage = basedamage;
                break;
            case 1:
                currentdamage =basedamage / 2;
                break;
            default:
                currentdamage = basedamage / 5;
                break;
        }
    }

    public void IncreaseAttackSpeed(int amount)
    {
        float help = 5f * (amount / 100f);
        //Debug.Log(help);
        if (this.firerate >= 0.05)
        {
            this.firerate += help;
        }
        else {
            Debug.Log("MAX AS REACHED");
        }
        
    }

    

    IEnumerator ShootingCoroutine()
    {
        float currentfirerate;
        allowfire = false;
        float helpvertical = movescript.helpvertical;
        float helpspeed = movescript.helpspeed;
        GameObject bullettype;
        switch (spell) {
            case 0:
                bullettype = bloodbulletPrefab;
                currentdamage = basedamage;
                currentfirerate = firerate * 0.7f;
                if (!(currentfirerate >= 0.05))
                {
                    Debug.Log("MAX AS REACHED");
                    currentfirerate = firerate;
                }
                float help2 = (1f / currentfirerate);
                astext.GetComponent<TMPro.TextMeshProUGUI>().text = help2.ToString();
                datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().currentdamage.ToString();
                break;
            case 1:
                bullettype = lightbulletPrefab;
                currentdamage = basedamage / 2;
                currentfirerate = firerate;
                float help3 = (1f / currentfirerate);
                astext.GetComponent<TMPro.TextMeshProUGUI>().text = help3.ToString();
                datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().currentdamage.ToString();
                break;
            default:
                bullettype = waterbulletPrefab;
                currentdamage =basedamage / 5;
                currentfirerate = firerate;
                float help4 = (1f / currentfirerate);
                astext.GetComponent<TMPro.TextMeshProUGUI>().text = help4.ToString();
                datext.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Weapon>().currentdamage.ToString();
                break;
        }
        /*Transform currfirepoint;
        if (helpspeed > 0.01f)
        {
            if (helpvertical <= -1f) // moving left
            {
                currfirepoint = firepoint_left;
                leftfirepoint.ChosenFirepoint();
                rightfirepoint.NotChosenFirepoint();
            }
            else
            {
                if (helpvertical >= 1f) // moving right
                {
                    currfirepoint = firepoint_right;
                    rightfirepoint.ChosenFirepoint();
                    leftfirepoint.NotChosenFirepoint();
                }
                else
                { // moving upwards/downwards
                    currfirepoint = firepoint_right;
                    rightfirepoint.ChosenFirepoint();
                    leftfirepoint.NotChosenFirepoint();
                }
            }
        }
        else { //standing
            currfirepoint = firepoint_left;
            leftfirepoint.ChosenFirepoint();
            rightfirepoint.NotChosenFirepoint();
        } */

        if (spell != 2)
        {
            GameObject tempbullet = Instantiate(bullettype, currfirepoint.position, currfirepoint.rotation);
            Rigidbody2D brb = tempbullet.GetComponent<Rigidbody2D>();
            brb.AddForce(currfirepoint.up * bulletForce, ForceMode2D.Impulse);
        }
        else {

            for (int i = 0; i < 3; i++) {
                Vector3 rotated = currfirepoint.rotation.eulerAngles;
                rotated = new Vector3(rotated.x,rotated.y , rotated.z + (15 * i));
                GameObject tempbullet = Instantiate(bullettype, currfirepoint.position,Quaternion.Euler(rotated));
                Rigidbody2D brb = tempbullet.GetComponent<Rigidbody2D>();
               
                brb.AddForce(tempbullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }

            for (int i = -1; i > -3; i--)
            {
                Vector3 rotated = currfirepoint.rotation.eulerAngles;
                rotated = new Vector3(rotated.x, rotated.y, rotated.z + (15 * i));
                GameObject tempbullet = Instantiate(bullettype, currfirepoint.position, Quaternion.Euler(rotated));
                Rigidbody2D brb = tempbullet.GetComponent<Rigidbody2D>();

                brb.AddForce(tempbullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }

        }
        yield return new WaitForSeconds(currentfirerate);
        allowfire = true;
    }

}
