using UnityEngine;
using System.Collections;

public class Backpack : PlayerBase {

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

		bool combat = false;
		string weaponType = "";
		float spd = 0f;

		switch (holdingWeapon) {
			case HandHold.MeleeWeapon:
			combat = true;
			weaponType = "MeleeWeapon";
			spd = .3f;
			break;
			case HandHold.MainWeapon:
			weaponType = "MainWeapon";
			spd = .1f;
			break;
			case HandHold.SecondaryWeapon:
			weaponType = "SecondaryWeapon";
			spd = .5f;
			break;
		}

		Transform weapon = transform.Find (weaponType);
		weapon.gameObject.SetActive(true);
		GunShooting.ArmWeapon(
			combat, 
			weapon.GetChild(0).GetComponentInChildren<Animator>(), 
			weapon.GetChild(0).transform.Find ("Top"), 
			spd
		);

		audioSource.Play ();
	}

	void PutDownAllWeapons () {
		foreach (Transform weapon in transform) {
			weapon.gameObject.SetActive(false);
		}
	}
}
