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
using System.Text;
using Newtonsoft.Json.Linq;

[Serializable]
public class VotingsO
{
    public string id;
    public int votes;
}

[Serializable]
public class PutSetting
{
    public string votername;
    public int newcolor;
}


    [Serializable]
public class GetColorSetting
{
    public string votername;
}

[Serializable]
public class VotingsP
{
    public string id;
    public int votes;
    public string voterid;
    public string votername;
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

    public GameObject voteannouncementobject;
    public Toggle toggle;
    public GameObject votingdisplay;

    private GameObject gamelogic;
    
    private bool voted;
    private const string URL = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod";
    private const string URLID = "https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/votings/{0}";

    static readonly HttpClient client = new HttpClient();
    private int changeweapon_votes;
    private int spawnmonster_votes;
    private int changemonster_votes;
    public int healplayer_votes;
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
    private int currentTime;
    private bool timerstarted;

    private bool allowLeaderboard;

    // Start is called before the first frame update
    void Start()
    {
        timerstarted = false;
        allowLeaderboard = true;
        currentPollNumber = 0;
        currentPollName = currentPollNumber.ToString();
        votingsetting = false;
        gamelogic = GameObject.FindGameObjectWithTag("GameLogic");
        votinglength = 15000;
        voted = true;
        changeweapon_votes = 0;
        voteboss_votes = 0;
        tempbuffdmgplayer_votes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(timerWave());
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetUids();
        }

        if (votingsetting)
        {
            if (voted)
            {
                if (!timerstarted) {
                    StartCoroutine(timerWave());
                    timerstarted = true;
                }

                VotingStopTheCount();
                ResetUids();
                //gamelogic.GetComponent<Action_CommandList>().CanVoteAgain();
            }
        }
        

        if (allowLeaderboard) {
            StartCoroutine(LeaderboardRefresh());
        }
        
    }

 

    public void Voting_Setting() {
        votingsetting = toggle.GetComponent<Toggle>().isOn;
        votingdisplay.SetActive(votingsetting);
        ClearLeaderboard();
        if (votingsetting)
        {
            gamelogic.GetComponent<Action_CommandList>().AllowNamedEnemy();
        }
        else {
            gamelogic.GetComponent<Action_CommandList>().DisallowNamedEnemy();
        }
        Debug.Log("Changed Voting Setting to " + votingsetting.ToString());
    }
    public int getTime()
    {
        return currentTime;
    }

    private IEnumerator timerWave()
    {
        if (currentTime > 0)
        {
            currentTime--;
        }
        yield return new WaitForSeconds(1f);
    }


    public async void VotingStopTheCount()
    {
        voted = false;
        ResetVotes("A");
        ResetVotes("B");
        ResetVotes("C");
        ResetVotes("D");
        ResetVotes("E");
        ResetVotes("F");
        ResetUids();
        currentTime = votinglength/1000;
        gameObject.GetComponent<Action_CommandList>().NewTimer(votinglength/1000);
        gamelogic.GetComponent<Action_CommandList>().CanVoteAgain();
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
        currentPollName = (currentPollNumber+1).ToString();
        PutPollName(currentPollName);
        await Task.Delay(votinglength);

        string jsonResponse = await VotingSubRoutine("A");
        //Debug.Log(jsonResponse);
        VotingsO voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        changeweapon_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("B");
        //Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        voteboss_votes = voteson.votes;


        jsonResponse = await VotingSubRoutine("C");
        //Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        tempbuffdmgplayer_votes = voteson.votes;

        jsonResponse = await VotingSubRoutine("D");
        //Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        spawnmonster_votes = voteson.votes;

        jsonResponse = await VotingSubRoutine("E");
        //Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        changemonster_votes = voteson.votes;

        jsonResponse = await VotingSubRoutine("F");
        //Debug.Log(jsonResponse);
        voteson = JsonUtility.FromJson<VotingsO>(jsonResponse);
        healplayer_votes = voteson.votes;

        //int viewercount = await GetCurrentViewercount();

        //Debug.Log(changeweapon_votes);
        //Debug.Log(voteboss_votes);
        //Debug.Log(tempbuffdmgplayer_votes);
        //Debug.Log(spawnmonster_votes);
        //Debug.Log(changemonster_votes);
       // Debug.Log(healplayer_votes);
        string voteresult = "";

        if (changeweapon_votes != 0 || voteboss_votes != 0 || tempbuffdmgplayer_votes != 0 || spawnmonster_votes != 0 || changemonster_votes != 0 || healplayer_votes != 0)
        {
            string max =
                 new[] {
                     Tuple.Create(changeweapon_votes, "A"),
                     Tuple.Create(voteboss_votes, "B"),
                     Tuple.Create(tempbuffdmgplayer_votes, "C"),
                     Tuple.Create(spawnmonster_votes, "D"),
                     Tuple.Create(changemonster_votes, "E"),
                     Tuple.Create(healplayer_votes, "F"),
                 }.Max()
                  .Item2;
            switch (max) {
                case "A":
                    gamelogic.GetComponent<Action_CommandList>().Action_ChangeWeapon();
                    voteresult = "Change to a random weapon";
                    break;
                case "B":
                    gamelogic.GetComponent<Action_CommandList>().Action_VoteSpawnBoss(1);
                    voteresult = "A potential boss monster";
                    break;
                case "C":
                    gamelogic.GetComponent<Action_CommandList>().Action_IncreasePlayerDamage(1);
                    voteresult = "Temporary player damage buff";
                    break;
                case "D":
                    gamelogic.GetComponent<Action_CommandList>().Action_SpawnMonsterCrowd(1);
                    voteresult = "Spawn a monster crowd";
                    break;
                case "E":
                    gamelogic.GetComponent<Action_CommandList>().Action_ChangeMonsterType(1);
                    voteresult = "Change the spawned monster type of the viewers";
                    break;
                case "F":
                    gamelogic.GetComponent<Action_CommandList>().Action_HealPlayer(1);
                    voteresult = "Heal the Player by a small amount";
                    break;

            }
            /*if (changeweapon_votes > voteboss_votes) //changeweapon||tempbuffplayer||spawnmonster||changemonster||healplayer
            {
                if (changeweapon_votes > tempbuffdmgplayer_votes)//changeweapon_votes||spawnmonster||changemonster||healplayer
                {
                    if(changeweapon_votes)
                    gamelogic.GetComponent<Action_CommandList>().Action_ChangeWeapon();
                    voteresult = "Change to a random weapon";
                }
                else //tempbuffdmgplayer_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_IncreasePlayerDamage(1);
                    voteresult = "Temporary player damage buff";
                }
            }
            else
            {
                if (voteboss_votes > tempbuffdmgplayer_votes) //voteboss_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_VoteSpawnBoss(1);
                    voteresult = "A potential boss monster";
                }
                else //tempbuffdmgplayer_votes
                {
                    gamelogic.GetComponent<Action_CommandList>().Action_IncreasePlayerDamage(1);
                    voteresult = "Temporary player damage buff";
                }
            }*/

            //text_voteannouncement.GetComponent<TMPro.TextMeshProUGUI>().text = "It was voted for " + voteresult + "\nVotes: A " + changeweapon_votes.ToString() + " |B " + voteboss_votes.ToString() + " |C " + tempbuffdmgplayer_votes.ToString();
            StartCoroutine(VoteAnnouncement("Poll "+currentPollName+" voted for:\n"+voteresult));
            

        }


        voted = true;
    }

    public Color parseColor(int x) { 
        switch (x)
            {
            case 1:
                return Color.blue;
            case 2:
                return Color.green;
            case 3:
                return Color.red;
            case 4:
                return Color.yellow;
            case 5:
                return new Color(146 / 255f, 0f / 255f, 209f / 255f, 1f); //lila
            case 6:
                return new Color(39f / 255f, 215f / 255f, 245f / 255f, 1f); //türkis
            case 7:
                return new Color(244f / 255f, 103f / 255f, 241f / 255f, 1f); //pink
            case 8:
                return new Color(237f / 255f, 133f / 255f, 14f/255f, 1f); //orange

            default:
                return Color.white;
            }
    }

    private void ClearLeaderboard() {

        scoreboardtop1.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount1.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop1.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop2.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount2.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop2.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop3.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount3.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop3.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop4.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount4.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop4.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop5.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount5.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop5.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop6.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount6.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop6.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop7.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount7.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop7.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop8.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount8.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop8.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop9.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount9.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop9.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop10.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount10.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop10.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop11.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount11.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop11.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop12.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount12.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop12.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        
        scoreboardtop13.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount13.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop13.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
       
        scoreboardtop14.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount14.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop14.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop15.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount15.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop15.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop16.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount16.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop16.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop17.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount17.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop17.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop18.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount18.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop18.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop19.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount19.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop19.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        scoreboardtop20.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scorecount20.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        scoreboardtop20.GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

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
            
            bool inarray = false;
            List<int> indexes = new List<int>();
            for (int counter = 0; counter < tobeparsed.Length; counter++)
            {
                //Debug.Log(tobeparsed[counter]);
                char currentchar = tobeparsed[counter];
                if (inarray) {
                    if (currentchar == ',') {
                        //Debug.Log("FOUND COMMA IN ARRAY AT:"+counter);
                        indexes.Add(counter);
                    }
                }
                if (currentchar == '[')
                {
                    //Debug.Log("INARRAY");
                    inarray = true;
                }
                else {
                    if (currentchar == ']') {
                        //Debug.Log("OUTOFARRAY");
                        inarray = false;
                    }
                }
            }
            
            StringBuilder tb = new StringBuilder(tobeparsed);
            foreach (int x in indexes) {
                //Debug.Log(x);
                tb[x] = '~';
            }
            tobeparsed = tb.ToString();
            //Debug.Log(tobeparsed);
            string[] viewernamesandcounts = tobeparsed.Split(",");
            Dictionary<string, Tuple<int, int>> viewerdata = new Dictionary<string, Tuple<int,int>>();
            
            foreach(string curr in viewernamesandcounts){
                string[] temp = curr.Split(":");
                string tempname = temp[0].Substring(1);
                tempname = tempname.Remove(tempname.Length - 1);
                string[] tempv = temp[1].Split("~");
                tempv[0] = tempv[0].Substring(1);
                tempv[1] = tempv[1].Remove(tempv[1].Length - 1);
                //Debug.Log(tempv[1]);
                Tuple<int, int> datanc = new Tuple<int, int>(int.Parse(tempv[0]), int.Parse(tempv[1]));
                viewerdata.Add(tempname,datanc);
            }
            var sortedDict = from entry in viewerdata orderby entry.Value descending select entry;
            int i = 0;
            foreach (KeyValuePair<string, Tuple<int, int>> curr in sortedDict) {
                if (i < 20)
                {
                    switch (i) {
                        case 0:
                            scoreboardtop1.GetComponent<TMPro.TextMeshProUGUI>().text =curr.Key;
                            scorecount1.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop1.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 1:
                            scoreboardtop2.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount2.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop2.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 2:
                            scoreboardtop3.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount3.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop3.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 3:
                            scoreboardtop4.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount4.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop4.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 4:
                            scoreboardtop5.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount5.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop5.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 5:
                            scoreboardtop6.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount6.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop6.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 6:
                            scoreboardtop7.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount7.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop7.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 7:
                            scoreboardtop8.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount8.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop8.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 8:
                            scoreboardtop9.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount9.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop9.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 9:
                            scoreboardtop10.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount10.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop10.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 10:
                            scoreboardtop11.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount11.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.ToString();
                            scoreboardtop11.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 11:
                            scoreboardtop12.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount12.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop12.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 12:
                            scoreboardtop13.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount13.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop13.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 13:
                            scoreboardtop14.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount14.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop14.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 14:
                            scoreboardtop15.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount15.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop15.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 15:
                            scoreboardtop16.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount16.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop16.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 16:
                            scoreboardtop17.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount17.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop17.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 17:
                            scoreboardtop18.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount18.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop18.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 18:
                            scoreboardtop19.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount19.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop19.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;
                        case 19:
                            scoreboardtop20.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Key;
                            scorecount20.GetComponent<TMPro.TextMeshProUGUI>().text = curr.Value.Item1.ToString();
                            scoreboardtop20.GetComponent<TMPro.TextMeshProUGUI>().color = parseColor(curr.Value.Item2);
                            break;

                    }
                    i += 1;
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

    public IEnumerator VoteAnnouncement(string message) {
        text_voteannouncement.GetComponent<TMPro.TextMeshProUGUI>().text = message;
        voteannouncementobject.SetActive(true);
        yield return new WaitForSeconds(5f);
        voteannouncementobject.SetActive(false);
    }

    IEnumerator LeaderboardRefresh() {
        allowLeaderboard = false;
        SetLeaderboard();
        yield return new WaitForSeconds(1f);
        allowLeaderboard = true;
    }

    public async void ResetVotes(string Vid)
    {
        var votingsformat = new VotingsP();
        votingsformat.id = Vid;
        votingsformat.votes = 0;
        votingsformat.voterid = "origin";
        votingsformat.votername = "origin";
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

    public async Task<Color> GetViewerColor(string name) {
        Debug.Log(name + " to be spawned but searching color");
        var votingsformat = new GetColorSetting();
        votingsformat.votername = name;
        string json = JsonConvert.SerializeObject(votingsformat);
        json = JsonConvert.SerializeObject(json);
        json = "{\"body\": " + json + "}";
        Debug.Log("SETCOLOR: "+json);
        var httpContent = new StringContent(json); 

        try
        {
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/viewerinformations/oneviewerinformation", httpContent);
            res.EnsureSuccessStatusCode();
            var jsonResponse = res.Content.ReadAsStringAsync().Result;
            JObject obj = JObject.Parse(jsonResponse);
            Debug.Log(obj["body"].ToString());

            return parseColor(int.Parse(obj["body"].ToString()));

        }
        catch (HttpRequestException e)
        {
            
            Debug.Log("Exception Caught " + e.Message);
            return Color.black;
        }
    }


    public async void setColor(string name, string x)
    {
        var votingsformat = new PutSetting();
        votingsformat.votername = name;
        votingsformat.newcolor= int.Parse(x);
        string json = JsonConvert.SerializeObject(votingsformat);
        json = JsonConvert.SerializeObject(json);
        json = "{\"body\": " + json + "}";
        //Debug.Log("SETCOLOR: "+json);
        var httpContent = new StringContent(json);
        try
        {

            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/viewerinformations/oneviewerinformation/putviewerinformations", httpContent);
            res.EnsureSuccessStatusCode();
            //Debug.Log(res.Content.ReadAsStringAsync().Result);
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
        //Debug.Log("PUTPOLLNAME: " + json);
        var httpContent = new StringContent(json);
        
        try
        {
            //Debug.Log(httpContent.ReadAsStringAsync().Result);
            var res = await client.PostAsync("https://5kl9amlvjh.execute-api.eu-central-1.amazonaws.com/prod/pollname", httpContent);
            res.EnsureSuccessStatusCode();
            //Debug.Log(res.Content.ReadAsStringAsync().Result);
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
