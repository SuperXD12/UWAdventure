using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

public class Action_CommandList : MonoBehaviour
{
    private Dictionary<string,Color> namedplayers;
    private GameObject player;
    private GameObject gamelogic;
    private ClientWebSocket ws;
    private bool allownamed;
    private bool nextnamed;
    // Start is called before the first frame update
    void Start()
    {
        nextnamed = true;
        namedplayers = new Dictionary<string,Color>();
        ws = new ClientWebSocket();
        player = GameObject.FindGameObjectWithTag("Player");
        gamelogic = GameObject.FindGameObjectWithTag("GameLogic");
        WebsocketRL();
        allownamed = false;
    }

    private void Update()
    {
        if (nextnamed) {
            nextnamed = false;
            Task.Run(donamedenemysspawn);
        }
    }

    void OnDestroy()
    {
        ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Game Closed", CancellationToken.None);
    }

    public void AllowNamedEnemy() {
        allownamed = true;
    }

    public void DisallowNamedEnemy() {
        allownamed = false;
    }

    private async void WebsocketRL() {
        await ws.ConnectAsync(new System.Uri("wss://rj347v6u7h.execute-api.eu-central-1.amazonaws.com/production"), CancellationToken.None);
        byte[] buf = new byte[1056];

        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(buf, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                Debug.Log("CLOSED");
                //await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);


            }
            else
            {
                string answer = Encoding.ASCII.GetString(buf, 0, result.Count);
                string[] codeandtext = answer.Split(":");
                if (codeandtext[0] == "1")
                {
                    //oldlabelenemy
                    Debug.Log("old: "+codeandtext[1]);
                    //Color currentcolor = await gamelogic.GetComponent<VotingLogic>().GetViewerColor(codeandtext[1]);
                    //SpawnNamedEnemy(codeandtext[1], currentcolor);

                }
                else {
                    if (codeandtext[0] == "3")
                    {
                        Debug.Log(codeandtext[1]);
                        string[] nameandcolor = codeandtext[1].Split(",");
                        if (namedplayers.ContainsKey(nameandcolor[0])) {
                            namedplayers[nameandcolor[0]] = gamelogic.GetComponent<VotingLogic>().parseColor(int.Parse(nameandcolor[1]));
                        }
                        gamelogic.GetComponent<VotingLogic>().setColor(nameandcolor[0], nameandcolor[1]);
                    }
                    else {
                        if (codeandtext[0] == "5") {  //newconnected
                            int currenttime = gamelogic.GetComponent<VotingLogic>().getTime();
                            Debug.Log("new connected " + currenttime);
                            NewTimer(currenttime);
                        }
                        else
                        {
                            if (codeandtext[0] == "6")
                            {  //labelenemy
                                Debug.Log("Please label: " + codeandtext[1]);
                                string cname = codeandtext[1];
                                if (!(namedplayers.ContainsKey(cname))) {
                                    Color currentcolor = await gamelogic.GetComponent<VotingLogic>().GetViewerColor(cname);
                                    namedplayers.Add(cname, currentcolor);
                                    
                                }
                            }
                            else
                            {
                                if (codeandtext[0] == "7")
                                {  //dontlabelenemy
                                    Debug.Log("Dont label: " + codeandtext[1]);
                                    if (namedplayers.ContainsKey(codeandtext[1]))
                                    {
                                        namedplayers.Remove(codeandtext[1]);
                                    }
                                }
                            }
                        }
                    }

                }


            }
        }
    }

    private async Task donamedenemysspawn() { //TODO: BUILD IENUMERATOR AGAIN
        Debug.Log("donamedenemyspawn");
        if (allownamed)
        {
            Debug.Log("allowed: donamedenemyspawn");
            foreach (KeyValuePair<string,Color> pairsc in namedplayers)
            {
                string cname = pairsc.Key;
                Color ccolor = pairsc.Value;
                Debug.Log("Trying to spawn: " + cname + " with color: " + ccolor);
                SpawnNamedEnemy(cname, ccolor);
   
            }

        }

        await Task.Delay(5000);
        nextnamed = true;
    }
    

    
    [System.Serializable]
    public class messages
    {
        public string action;
        public string message;
    }
    public async void CanVoteAgain() {
        var obj = new messages
        {
            action = "sendmessage",
            message = "2:a"
        };
        var encoded = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        var buffer = new System.ArraySegment<System.Byte>(encoded, 0, encoded.Length);
        //Debug.Log("Send message: 2:a");
        
        await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async void NewTimer(int x) {
        var obj = new messages
        {
            action = "sendmessage",
            message = "4:"+x
        };
        var encoded = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        var buffer = new System.ArraySegment<System.Byte>(encoded, 0, encoded.Length);
        Debug.Log("Send message: 4:"+x);
        if (ws != null)
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }
    public void Action_ChangeWeapon() {
        player.GetComponent<Weapon>().ExecuteAction_ChangeWeapon();
    }

    public void Action_IncreasePlayerDamage(int viewercount) {
        StartCoroutine(TimedBuffDamage(viewercount));
    }
    private IEnumerator TimedBuffDamage(int viewercount) {
        //int damagebuff = Mathf.RoundToInt(50 * Mathf.Min(1, 10 / viewercount));
        int damagebuff = 500;
        player.GetComponent<Weapon>().IncreaseDamage(damagebuff);
        float time = 10f;//10f * Mathf.Min(1, 10 / viewercount);
        Debug.Log("Temporarily Increased the Damage of the Player by " + damagebuff + " for "+time+" seconds");
        yield return new WaitForSeconds(time);
        player.GetComponent<Weapon>().IncreaseDamage(damagebuff*-1);
        Debug.Log("Temp. buffed Damage of the Player by " + damagebuff + " expired");
    }

    public void Action_VoteSpawnBoss(int viewercount) {
        gamelogic.GetComponent<GameLogic>().Action_VoteforBoss(viewercount);
    }

    public void Action_SpawnMonsterCrowd(int viewercount) {
        gamelogic.GetComponent<GameLogic>().Action_SpawnMonsterCrowd(viewercount);
    }

    public void Action_ChangeMonsterType(int viewercount) {
        gamelogic.GetComponent<GameLogic>().Action_ChangeMonsterType(viewercount);
    }

    public void Action_HealPlayer(int viewercount) {
        int amount;
        int w = gamelogic.GetComponent<GameLogic>().GetWave();
        if (w <= 10)
        {
            amount = 500;
        }
        else {
            amount = 1000;
        }
            player.GetComponent<Health>().Heal(amount);
    }

    private void SpawnNamedEnemy(string name, Color color) {
        Debug.Log("SPAWNNAMED: "+allownamed);
        if (allownamed) {


            try
            {
                gamelogic.GetComponent<GameLogic>().SpawnLabeledEnemy(name, color);
            }
            catch (System.Exception e) {
                Debug.Log("Exception caught: "+ e.Message);
            }
        }
        
    }

}
