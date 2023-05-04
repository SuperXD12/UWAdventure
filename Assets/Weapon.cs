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

[Serializable]
public class VotingsO {
    public string id;
    public int votes;
}



public class Weapon : MonoBehaviour
{
    private const string URL = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod";
    private const string URLID = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}";

    static readonly HttpClient client = new HttpClient();

    private int a_votes;
    private int b_votes;
    private int c_votes;

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
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";
        }
        if (Input.GetButtonDown("2"))
        {
            spell = 1;
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";
        }
        if (Input.GetButtonDown("3"))
        {
            spell = 2;
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "Voting in progress...";

        }

        if (voted) {

            //VotingStopTheCount();
            
            
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

    public async void VotingStopTheCount() {
        voted = false;

        string jsonResponse = await VotingSubRoutine("A");
        Debug.Log(jsonResponse);
        VotingsO voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        a_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("B");
        Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        b_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("C");
        Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        c_votes = voteson.votes;


        Debug.Log(a_votes);
        Debug.Log(b_votes);
        Debug.Log(c_votes);


        if (a_votes != 0 || b_votes != 0 || c_votes != 0)
        {
            if (a_votes > b_votes)
            {
                if (a_votes > c_votes)//A Spell
                {
                    spell = 0;
                }
                else //C Spell
                {
                    spell = 2;
                }
            }
            else
            {
                if (b_votes > c_votes) //B Spell
                {
                    spell = 1;
                }
                else //C Spell
                {
                    spell = 2;
                }
            }
            string spellletter = "";
            switch (spell)
            {
                case 0:
                    spellletter = "A";
                    break;
                case 1:
                    spellletter = "B";
                    break;
                default:
                    spellletter = "C";
                    break;

            }
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "It was voted for " + spellletter + "\nVotes: A " + a_votes.ToString() + " |B " + b_votes.ToString() + " |C " + c_votes.ToString();

            ResetVotes("A");
            ResetVotes("B");
            ResetVotes("C");

        }



        


        await Task.Delay(votinglength);
        voted = true;
    }


    public async void ResetVotes(string Vid)
    {
        var votingsformat = new VotingsO();
        votingsformat.id = Vid;
        votingsformat.votes = 0;
        string json = JsonConvert.SerializeObject(votingsformat);
        var httpContent = new StringContent(json);
        try
        {
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/",httpContent );
            res.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }
    }

    public async Task<string> VotingSubRoutine(string Vid) {


        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}", Vid));
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            HttpResponseMessage response = await client.GetAsync(String.Format("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}", Vid));
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;

        }
        catch (HttpRequestException e) {
            Debug.Log("Exception Caught " + e.Message);
        }

        return "";

        
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
