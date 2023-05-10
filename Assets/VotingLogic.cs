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
using System.Linq;

[Serializable]
public class VotingsO
{
    public string id;
    public int votes;
}

[Serializable]
public class PutPollName
{
    //public string id;
    public string Pollname;
}



[Serializable]
public class JsonResponseType
{
    public int statusCode;
    public string headers;
    public string body;
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
    private string currentPollName;
    private int currentPollNumber;

    public GameObject scoreboardtop1;
    public GameObject scoreboardtop2;
    public GameObject scoreboardtop3;
    public GameObject scoreboardtop4;
    public GameObject scoreboardtop5;
    public GameObject scoreboardtop6;
    public GameObject scoreboardtop7;
    public GameObject scoreboardtop8;
    public GameObject scoreboardtop9;
    public GameObject scoreboardtop10;
    public GameObject scoreboardtop11;
    public GameObject scoreboardtop12;
    public GameObject scoreboardtop13;
    public GameObject scoreboardtop14;
    public GameObject scoreboardtop15;
    public GameObject scoreboardtop16;
    public GameObject scoreboardtop17;
    public GameObject scoreboardtop18;
    public GameObject scoreboardtop19;
    public GameObject scoreboardtop20;

    public GameObject scorecount1;
    public GameObject scorecount2;
    public GameObject scorecount3;
    public GameObject scorecount4;
    public GameObject scorecount5;
    public GameObject scorecount6;
    public GameObject scorecount7;
    public GameObject scorecount8;
    public GameObject scorecount9;
    public GameObject scorecount10;
    public GameObject scorecount11;
    public GameObject scorecount12;
    public GameObject scorecount13;
    public GameObject scorecount14;
    public GameObject scorecount15;
    public GameObject scorecount16;
    public GameObject scorecount17;
    public GameObject scorecount18;
    public GameObject scorecount19;
    public GameObject scorecount20;


    private bool allowLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        allowLeaderboard = true;
        currentPollNumber = 1;
        currentPollName = currentPollNumber.ToString();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetUids();
        }

        if (votingsetting) {
            if (voted)
            {
                ResetUids();
                VotingStopTheCount();
                ResetUids();
            }
        }

        if (allowLeaderboard) {
            StartCoroutine(LeaderboardRefresh());
        }
        
    }

 

    public void Voting_Setting() {
        votingsetting = toggle.GetComponent<Toggle>().isOn;
        Debug.Log("Changed Voting Setting to " + votingsetting.ToString());
    }

    public async void VotingStopTheCount()
    {
        voted = false;
        ResetVotes("A");
        ResetVotes("B");
        ResetVotes("C");
        ResetUids();
        /*if (currentPollName == "Fish")
        {
            currentPollName = "Seahorse";
        }
        else {
            if (currentPollName == "Seahorse")
            {
                currentPollName = "Snail";
            }
            else {
                currentPollName = "Fish";
            }
        }*/
        currentPollNumber += 1;
        currentPollName = currentPollNumber.ToString();
        PutPollName(currentPollName);
        await Task.Delay(votinglength);

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

            

        }


        voted = true;
    }

    
    
    public async void SetLeaderboard() {
        
        try
        {

            var res = await client.GetAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/viewerinformations");
            res.EnsureSuccessStatusCode();
            var jsonResponse = await res.Content.ReadAsStringAsync();
            //Debug.Log("LEADERBOARD: "+jsonResponse);

            JsonResponseType newresponse = JsonUtility.FromJson<JsonResponseType>(jsonResponse);
            //JsonResponseType responses2 = JsonUtility.FromJson<JsonResponseType>(jsonData);
            var jsonData = JsonConvert.DeserializeObject(newresponse.body);
            //Debug.Log(jsonData);
            string jsonString = JsonConvert.SerializeObject(jsonData);
            //Debug.Log(jsonString);
            int index = jsonString.LastIndexOf(@"{");
            string tobeparsed = jsonString.Substring(index + 1);
            tobeparsed = tobeparsed.Remove(tobeparsed.Length - 2);
            string[] viewernamesandcounts = tobeparsed.Split(",");
            Dictionary<string, int> viewerdata = new Dictionary<string, int>();
            foreach(string curr in viewernamesandcounts){
                string[] temp = curr.Split(":");
                string tempname = temp[0].Substring(1);
                tempname = tempname.Remove(tempname.Length - 1);
                viewerdata.Add(tempname, int.Parse(temp[1]));
            }

            int i = 0;
            foreach (KeyValuePair<string, int> curr in viewerdata) {
                if (i < 20)
                {
                    switch (i) {
                        case 0:
                            scoreboardtop1.GetComponent<TMPro.TextMeshProUGUI>().text =curr.Key;
                            scorecount1.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 1:
                            scoreboardtop2.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount2.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 2:
                            scoreboardtop3.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount3.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 3:
                            scoreboardtop4.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount4.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 4:
                            scoreboardtop5.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount5.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 5:
                            scoreboardtop6.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount6.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 6:
                            scoreboardtop7.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount7.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 7:
                            scoreboardtop8.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount8.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 8:
                            scoreboardtop9.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount9.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 9:
                            scoreboardtop10.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount10.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 10:
                            scoreboardtop11.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount11.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 11:
                            scoreboardtop12.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount12.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 12:
                            scoreboardtop13.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount13.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 13:
                            scoreboardtop14.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount14.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 14:
                            scoreboardtop15.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount15.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 15:
                            scoreboardtop16.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount16.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 16:
                            scoreboardtop17.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount17.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 17:
                            scoreboardtop18.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount18.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 18:
                            scoreboardtop19.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount19.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;
                        case 19:
                            scoreboardtop20.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount20.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            break;

                    }
                }
                else {
                    break;
                }
            }

        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }
       
    }

    IEnumerator LeaderboardRefresh() {
        allowLeaderboard = false;
        SetLeaderboard();
        yield return new WaitForSeconds(1f);
        allowLeaderboard = true;
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

    public async void PutPollName(string PM)
    {

        var pollnameformat = new PutPollName();
        //pollnameformat.id = "CurrentPoll";
        pollnameformat.Pollname = PM;
        string json = JsonConvert.SerializeObject(pollnameformat);
        json = JsonConvert.SerializeObject(json);
        json = "{\"body\": " + json + "}";
        //json = "{\"body\": \"{\\\"Pollname\\\":\\\"G\\\"}\"}";
        var httpContent = new StringContent(json);
        
        try
        {
            Debug.Log(httpContent.ReadAsStringAsync().Result);
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/pollname", httpContent);
            res.EnsureSuccessStatusCode();
            Debug.Log(res.Content.ReadAsStringAsync().Result);
        }
        catch (HttpRequestException e)
        {
            Debug.Log("Exception Caught " + e.Message);
        }

        


    }

    public string GetCurrentPollName() {
        return currentPollName;
    }


}
