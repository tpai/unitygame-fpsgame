using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] InputField messageText;

	[SerializeField] Camera panelCamera;
	[SerializeField] AudioListener panelAudioListener;
	[SerializeField] Transform loginPanel;
	[SerializeField] InputField playerName;

	[SerializeField] Transform[] spawnPoints;

	Queue<string> messages;
	const int messageCount = 7;
	PhotonView photonView;

	void Start () {
		photonView = GetComponent<PhotonView> ();
		messages = new Queue<string> (messageCount);
		
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("1.0");
		StartCoroutine ("UpdateConnectionText");
	}

	IEnumerator UpdateConnectionText () {
		while (true) {
			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	}

	void OnJoinedLobby () {
		loginPanel.gameObject.SetActive (true);
	}

	public void JoinRoom () {
		PhotonNetwork.player.name = playerName.text;
		RoomOptions roomOptions = new RoomOptions () { isVisible = true, maxPlayers = 10 };
		PhotonNetwork.JoinOrCreateRoom ("Default", roomOptions, TypedLobby.Default);
	}

	void OnJoinedRoom () {
		StopCoroutine ("UpdateConnectionText");
		connectionText.text = "";
		InitPlayerProperties ();
		StartSpawnProcess (0f);
	}

	void StartSpawnProcess (float respawnTime) {
		panelCamera.enabled = true;
		panelAudioListener.enabled = true;
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	IEnumerator SpawnPlayer (float respawnTime) {
		yield return new WaitForSeconds(respawnTime);
		
		int index = Random.Range (0, spawnPoints.Length);
		GameObject player = (GameObject)PhotonNetwork.Instantiate (
			"FPSPlayer", 
			spawnPoints [index].position, 
			spawnPoints [index].rotation, 
			0
		);

		player.GetComponent<HPController> ().PlayerKilledBy += KillPlayer;

		panelCamera.enabled = false;
		panelAudioListener.enabled = false;
		loginPanel.gameObject.SetActive (false);
		
		AddMessage ("Spawned player: " + PhotonNetwork.player.name);
	}

	void KillPlayer (string player, string killer) {
		photonView.RPC ("KillPlayer_RPC", PhotonTargets.All, player, killer);
	}

	[RPC]
	void KillPlayer_RPC (string player, string killer) {
		if (PhotonNetwork.player.name == player) {
			panelCamera.enabled = true;
			panelAudioListener.enabled = true;
			StartCoroutine ("SpawnPlayer", 3f);
			AddDeathCount ();
			AddMessage_RPC ("You've been killed by " + killer+"!");

//			PhotonNetwork.player.SetTeam (PunTeams.Team.red)
		}
		else if (PhotonNetwork.player.name == killer) {
			AddKillCount ();
			AddMessage_RPC ("You killed " + player+"!");
		}
		else {
			AddMessage (killer+" kill " + player+"!");
		}
	}

	void InitPlayerProperties () {
		ExitGames.Client.Photon.Hashtable PlayerCustomProps = new ExitGames.Client.Photon.Hashtable();
		PlayerCustomProps["Kills"] = 0;
		PlayerCustomProps["Death"] = 0;
		PhotonNetwork.player.SetCustomProperties(PlayerCustomProps);
	}

	void AddKillCount () {
		ExitGames.Client.Photon.Hashtable props = PhotonNetwork.player.customProperties;
		props ["Kills"] = (int)props ["Kills"] + 1;
		PhotonNetwork.player.SetCustomProperties(props);
	}

	void AddDeathCount () {
		ExitGames.Client.Photon.Hashtable props = PhotonNetwork.player.customProperties;
		props ["Death"] = (int)props ["Death"] + 1;
		PhotonNetwork.player.SetCustomProperties(props);
	}
	
	void AddMessage(string message) {
		photonView.RPC ("AddMessage_RPC", PhotonTargets.All, message);
	}

	[RPC]
	void AddMessage_RPC(string message) {
		messages.Enqueue (message);
		if(messages.Count > messageCount) {
			messages.Dequeue();
		}
		messageText.text = "";
		foreach (string m in messages) {
			messageText.text += m + "\n";
		}
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {}
}
