﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPController : PlayerBase {

	public delegate void PlayerKilled (string player, string killer);
	public event PlayerKilled PlayerKilledBy;

	[SerializeField] private Slider hpSlider;
	[SerializeField] private Text hpText;

	public int maxHP = 100;
	int nowHP;

	int m_NetworkedNowHP;

	void Start () {
		hpSlider.maxValue = maxHP;
		nowHP = maxHP;
		m_NetworkedNowHP = maxHP;
	}

	void FixedUpdate () {
		if (nowHP <= 0)
			return ;
		if (!PhotonView.isMine) {
			hpSlider.value = m_NetworkedNowHP;
			hpText.text = "";
			return ;
		}

		hpSlider.value = nowHP;
		hpText.text = "HP: " + nowHP + "/" + maxHP;
	}

	public void AddHP (int amt, string killer) {
		nowHP += amt;

		if (nowHP <= 0) {
			nowHP = 0;

			if (PlayerKilledBy != null) {
				PlayerKilledBy (PhotonView.owner.name, killer);
			}

			Destroy (gameObject);
			PhotonView.Destroy(gameObject);
		}
	}

	public void SerializeState (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(nowHP);
		} else {
			m_NetworkedNowHP = (int)stream.ReceiveNext();
			nowHP = m_NetworkedNowHP;
		}
	}
}
