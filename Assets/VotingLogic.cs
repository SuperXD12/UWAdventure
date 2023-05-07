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
using UnityEngine.UI;

[Serializable]
public class VotingsO
{
    public string id;
    public int votes;
}



public class VotingLogic : MonoBehaviour
{
    public GameObject text_voteannouncement;
    public Toggle toggle;

    private GameObject gamelogic;
    
    private bool voted;
    private const string URL = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod";
    private const string URLID = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}";

    static readonly HttpClient client = new HttpClient();
    private int changeweapon_votes;
    private int voteboss_votes;
    private int tempbuffdmgplayer_votes;
    private int votinglength;
    private bool votingsetting;

    // Start is called before the first frame update
    void Start()
    {
        votingsetting = false;
        gamelogic = GameObject.FindGameObjectWithTag("GameLogic");
        votinglength = 10000;
        voted = true;
        changeweapon_votes = 0;
        voteboss_votes = 0;
        tempbuffdmgplayer_votes = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (votingsetting) {
            if (voted)
            {
                VotingStopTheCount();
            }
        }
        
    }

    public void Voting_Setting() {
        votingsetting = toggle.GetComponent<Toggle>().isOn;
        Debug.Log("Changed Voting Setting to " + votingsetting.ToString());
    }

    public async void VotingStopTheCount()
    {
        voted = false;
        
        string jsonResponse = await VotingSubRoutine("A");
        Debug.Log(jsonResponse);
        VotingsO voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        changeweapon_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("B");
        Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        voteboss_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("C");
        Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        tempbuffdmgplayer_votes = voteson.votes;

        //int viewercount = await GetCurrentViewercount();

        Debug.Log(changeweapon_votes);
        Debug.Log(voteboss_votes);
        Debug.Log(tempbuffdmgplayer_votes);
        string voteresult = "";

        if (changeweapon_votes != 0 || voteboss_votes != 0 || tempbuffdmgplayer_votes != 0)
        {
            if (changeweapon_votes > voteboss_votes)
            {
                if (changeweapon_votes > tempbuffdmgplayer_votes)//changeweapon_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_ChangeWeapon();
                    voteresult = "Change to a random weapon";
                }
                else //tempbuffdmgplayer_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_IncreasePlayerDamage(1);
                    voteresult = "Temporary buff the players damage";
                }
            }
            else
            {
                if (voteboss_votes > tempbuffdmgplayer_votes) //voteboss_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_VoteSpawnBoss(1);
                    voteresult = "One vote for a boss monster has been added";
                }
                else //tempbuffdmgplayer_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_IncreasePlayerDamage(1);
                    voteresult = "Temporary buff the players damage";
                }
            }
            
            text_voteannouncement.GetComponent<TMPro.TextMeshProUGUI>().text = "It was voted for " + voteresult + "\nVotes: A " + changeweapon_votes.ToString() + " |B " + voteboss_votes.ToString() + " |C " + tempbuffdmgplayer_votes.ToString();

            ResetVotes("A");
            ResetVotes("B");
            ResetVotes("C");
            ResetUids();

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
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/", httpContent);
            res.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }
    }

    public async void ResetUids()
    {
        
        try
        {
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/uidreset", null);
            res.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }
    }

    public async Task<string> VotingSubRoutine(string Vid)
    {


        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}", Vid));
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            HttpResponseMessage response = await client.GetAsync(String.Format("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}", Vid));
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;

        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }

        return "";


    }
}
