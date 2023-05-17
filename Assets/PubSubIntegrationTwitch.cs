using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.PubSub.Events;
using System;

public class PubSubIntegrationTwitch : MonoBehaviour
{
	public GameObject gamelogic;
	private PubSub _pubSub;

    private void Start()
    {
		// To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
		// Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
		// This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
		// Application.runInBackground = true;

		// Create new instance of PubSub Client
		_pubSub = new PubSub();

		// Subscribe to Events
		_pubSub.OnWhisper += OnWhisper;
		_pubSub.OnPubSubServiceConnected += OnPubSubServiceConnected;
		_pubSub.OnStreamUp += onStreamUp;
		_pubSub.OnStreamDown += onStreamDown;
		_pubSub.OnChannelPointsRewardRedeemed += MyChannelPointsRewardRedeemed;
		_pubSub.OnListenResponse += OnListenResponse;
		// Connect
		_pubSub.Connect();
	}


	private void onStreamUp(object sender, OnStreamUpArgs e)
	{
		Debug.Log($"Stream just went up! Play delay: {e.PlayDelay}, server time: {e.ServerTime}");
	}

	private void onStreamDown(object sender, OnStreamDownArgs e)
	{
		Debug.Log($"Stream just went down! Server time: {e.ServerTime}");
	}

	private void MyChannelPointsRewardRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
    {
		Debug.Log("Got ChannelPoints Reward");
		if (e.RewardRedeemed.Redemption.Reward.Id == "17") {
			gamelogic.GetComponent<Action_CommandList>().Action_SpawnMonsterCrowd(1);
		}
    }

    /*private void OnDestroy()
    {
		_pubSub.Disconnect();
		Debug.Log("PubSubServiceDisconnected?");
	} */

    private void OnPubSubServiceConnected(object sender, System.EventArgs e)
	{
		Debug.Log("PubSubServiceConnected!");
		
		// On connect listen to Bits evadsent
		// Please note that listening to the whisper events requires the chat_login scope in the OAuth token.
		_pubSub.ListenToWhispers(Secrets.CHANNEL_ID_FROM_OAUTH_TOKEN);
		_pubSub.ListenToChannelPoints(Secrets.CHANNEL_ID_FROM_OAUTH_TOKEN);
		_pubSub.ListenToVideoPlayback(Secrets.CHANNEL_ID_FROM_OAUTH_TOKEN);
		_pubSub.ListenToBitsEventsV2(Secrets.CHANNEL_ID_FROM_OAUTH_TOKEN);
		
		// SendTopics accepts an oauth optionally, which is necessary for some topics, such as bit events.
		_pubSub.SendTopics(Secrets.OAUTH_TOKEN);
	}

	private void OnWhisper(object sender, TwitchLib.PubSub.Events.OnWhisperArgs e)
	{
		Debug.Log($"{e.Whisper.Data}");
		// Do your bits logic here.
	}


	


	private void OnListenResponse(object sender, OnListenResponseArgs e)
	{
		if (e.Successful)
		{
			Console.WriteLine($"Successfully verified listening to topic: {e.Topic}");
		}
		else
		{
			Console.WriteLine($"Failed to listen! Error: {e.Response.Error}");
		}
	}


}
