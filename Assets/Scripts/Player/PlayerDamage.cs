using UnityEngine;
using System.Collections;

public class PlayerDamage : PlayerBase {

	[SerializeField] Animator anim;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip[] clips;

	public void GotHurt () {
		anim.SetTrigger ("hurt");
		audioSource.clip = clips[Random.Range(0, clips.Length)];
		audioSource.Play ();
	}
}
