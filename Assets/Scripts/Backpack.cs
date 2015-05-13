using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour {

	[SerializeField] private AudioSource audioSource;
	public enum HandHold { MeleeWeapon, MainWeapon, SecondaryWeapon };
	public HandHold holdingWeapon;

	void Start () {
		WeaponCheck ();
	}

	void Update () {

		if (CheckIfReloading())
			return;

		if (holdingWeapon != HandHold.MeleeWeapon && Input.GetKeyDown (KeyCode.Alpha1)) {
			holdingWeapon = HandHold.MeleeWeapon;
			WeaponCheck ();
		}
		if (holdingWeapon != HandHold.SecondaryWeapon && Input.GetKeyDown (KeyCode.Alpha2)) {
			holdingWeapon = HandHold.SecondaryWeapon;
			WeaponCheck ();
		}
		if (holdingWeapon != HandHold.MainWeapon && Input.GetKeyDown (KeyCode.Alpha3)) {
			holdingWeapon = HandHold.MainWeapon;
			WeaponCheck ();
		}
	}

	bool CheckIfReloading () {
		foreach (GunShooting gun in transform.GetComponentsInChildren<GunShooting>()) {
			if (gun.isReloading)return true;
		}
		return false;
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
		audioSource.Play ();
	}

	void PutDownAllWeapons () {
		foreach (Transform weapon in transform) {
			weapon.gameObject.SetActive(false);
		}
	}
}
