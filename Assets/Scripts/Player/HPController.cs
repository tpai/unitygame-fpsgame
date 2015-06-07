using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPController : PlayerBase {

	public delegate void PlayerKilled (string player, string killer);
	public event PlayerKilled PlayerKilledBy;

	public Slider hpSlider;
	public Text hpText;

	public int maxHP = 100;
	public int nowHP;
	public bool isDead = false;

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

	public virtual void AddHP (int amt, string killer) {
		
		if (isDead)return ;
		
		nowHP += amt;

		if (amt < 0)
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

	public void SerializeState (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(nowHP);
		} else {
			m_NetworkedNowHP = (int)stream.ReceiveNext();
			nowHP = m_NetworkedNowHP;
		}
	}
}
