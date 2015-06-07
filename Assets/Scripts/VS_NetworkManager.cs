using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VS_NetworkManager : NetworkManager {

	public override IEnumerator SpawnPlayer (float respawnTime) {
		yield return new WaitForSeconds(respawnTime);

		PunTeams.Team team = WhichTeamMemberIsLess ();
		PhotonNetwork.player.SetTeam ( team );

		string playerPrefab = "";
		if (team == PunTeams.Team.blue)
			playerPrefab = "FPSPlayer_Blue";
		if (team == PunTeams.Team.red)
			playerPrefab = "FPSPlayer_Red";

		int index = Random.Range (0, spawnPoints.Length);
		GameObject player = (GameObject)PhotonNetwork.Instantiate (
			playerPrefab, 
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0
		);
		player.name = PhotonNetwork.player.name;
		player.GetComponent<VS_HPController> ().PlayerKilledBy += KillPlayer;

		panelCamera.enabled = false;
		panelAudioListener.enabled = false;
		
		AddMessage ("Spawned player: " + PhotonNetwork.player.name);
	}

	PunTeams.Team WhichTeamMemberIsLess () {
		int redTeam = 0;
		int blueTeam = 0;

		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam() == PunTeams.Team.red)redTeam ++;
			if (player.GetTeam() == PunTeams.Team.blue)blueTeam ++;
		}

		return (redTeam <= blueTeam)?PunTeams.Team.red : PunTeams.Team.blue;
	}
}
