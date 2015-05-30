using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePanel : MonoBehaviour {

	Animator anim;
	bool shown = false;

	[SerializeField] private Text deathCountText;
	[SerializeField] private Text killsCountText;

	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	void Update () {
		if (!shown && Input.GetKeyDown (KeyCode.Tab)) {
			shown = true;
			deathCountText.text = "Death: "+PhotonNetwork.player.customProperties["Death"].ToString ();
			killsCountText.text = "Kills: "+PhotonNetwork.player.customProperties["Kills"].ToString ();
			anim.SetBool ("show", true);
		}
		if (shown && Input.GetKeyUp (KeyCode.Tab)) {
			shown = false;
			anim.SetBool ("show", false);
		}
	}
}
