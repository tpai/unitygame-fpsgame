using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePanel : MonoBehaviour {

	Animator anim;
	bool shown = false;

	[SerializeField] private Text playerName;
	[SerializeField] private Text kills;
	[SerializeField] private Text death;


	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	void Update () {
		if (!shown && Input.GetKeyDown (KeyCode.Tab)) {
			shown = true;
			playerName.text = "";
			kills.text = "";
			death.text = "";
			foreach (PhotonPlayer player in PhotonNetwork.playerList){
				playerName.text += player.name.ToString()+"\n";
				kills.text += player.customProperties["Kills"]+"\n";
				death.text += player.customProperties["Death"]+"\n";
			}
			anim.SetBool ("show", true);
		}
		if (shown && Input.GetKeyUp (KeyCode.Tab)) {
			shown = false;
			anim.SetBool ("show", false);
		}
	}
}
