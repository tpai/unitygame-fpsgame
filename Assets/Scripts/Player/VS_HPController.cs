using UnityEngine;
using System.Collections;

public class VS_HPController : HPController {

	public delegate void PlayerKilled (string player, string killer);
	public new event PlayerKilled PlayerKilledBy;

	public override void AddHP (int amt, string killer) {
		
		if (isDead)return ;

		if (IsMyTeamMate (killer))return ;

		nowHP += amt;
		
		if (amt < 0 && PhotonView.isMine)
			PlayerDamage.GotHurt ();
		
		if (nowHP <= 0) {
			nowHP = 0;
			isDead = true;
			
			if (PlayerKilledBy != null) {
				PlayerKilledBy (PhotonView.owner.name, killer);
			}
			
			PhotonNetwork.Destroy(gameObject);
			Destroy (gameObject);
		}
	}

	bool IsMyTeamMate (string name) {
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.name == name && player.GetTeam() == PhotonNetwork.player.GetTeam()) {
				return true;
			}
		}
		return false;
	}
}
