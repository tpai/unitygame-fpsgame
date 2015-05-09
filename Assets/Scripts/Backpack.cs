using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour {

	[SerializeField] private AudioSource audio;
	public enum HandHold { MeleeWeapon, MainWeapon, SecondaryWeapon };
	public HandHold holdingWeapon;

	void Start () {
		WeaponCheck ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			holdingWeapon = HandHold.MeleeWeapon;
			WeaponCheck ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			holdingWeapon = HandHold.SecondaryWeapon;
			WeaponCheck ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			holdingWeapon = HandHold.MainWeapon;
			WeaponCheck ();
		}
	}
	
	void WeaponCheck () {
		PutDownAllWeapons ();
		switch (holdingWeapon) {
			case HandHold.MeleeWeapon:
				transform.Find ("MeleeWeapon").gameObject.SetActive(true);
			break;
			case HandHold.MainWeapon:
				transform.Find ("MainWeapon").gameObject.SetActive(true);
			break;
			case HandHold.SecondaryWeapon:
				transform.Find ("SecondaryWeapon").gameObject.SetActive(true);
			break;
		}
		audio.Play ();
	}

	void PutDownAllWeapons () {
		foreach (Transform weapon in transform) {
			weapon.gameObject.SetActive(false);
		}
	}
}
