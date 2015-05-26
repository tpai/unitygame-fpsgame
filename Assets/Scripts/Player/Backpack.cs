using UnityEngine;
using System.Collections;

public class Backpack : PlayerBase {

	[SerializeField] private AudioSource audioSource;
	public enum Weapon { Melee, Main, Secondary };
	public Weapon holdingWeapon;

	Weapon m_NetworkedHoldingWeapon;

	void Start () {
		WeaponCheck ();
	}

	void FixedUpdate () {

		if (!PhotonView.isMine)
			return;

		if (GunShooting.isReloading)
			return;

		if (holdingWeapon != Weapon.Melee && Input.GetKeyDown (KeyCode.Alpha1)) {
			holdingWeapon = Weapon.Melee;
			WeaponCheck ();
		}
		if (holdingWeapon != Weapon.Secondary && Input.GetKeyDown (KeyCode.Alpha2)) {
			holdingWeapon = Weapon.Secondary;
			WeaponCheck ();
		}
		if (holdingWeapon != Weapon.Main && Input.GetKeyDown (KeyCode.Alpha3)) {
			holdingWeapon = Weapon.Main;
			WeaponCheck ();
		}
	}

	void WeaponCheck () {
		PutDownAllWeapons ();

		bool combat = false;
		string type = "";
		float spd = 0f;

		switch (holdingWeapon) {
		case Weapon.Melee:
			combat = true;
			type = "MeleeWeapon";
			spd = .3f;
			break;
		case Weapon.Main:
			type = "MainWeapon";
			spd = .1f;
			break;
		case Weapon.Secondary:
			type = "SecondaryWeapon";
			spd = .5f;
			break;
		}

		Transform weapon = transform.Find (type);
		weapon.gameObject.SetActive(true);
		GunShooting.ArmWeapon(
			combat, 
			weapon.GetChild(0).GetComponent<Animator>(), 
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

	public void SerializeState (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (holdingWeapon.ToString ());
		} else {
			m_NetworkedHoldingWeapon = FromStringToEnum((string)stream.ReceiveNext());

			if (m_NetworkedHoldingWeapon != holdingWeapon) {
				holdingWeapon = m_NetworkedHoldingWeapon;
				GunShooting.isArming = true;
				WeaponCheck ();
			}
		}
	}


	Weapon FromStringToEnum (string str) {
		System.Array values = System.Enum.GetValues(typeof(Weapon));
		foreach( Weapon val in values )
		{
			if (str == val.ToString ()) {
				return val;
			}
		}
		return Weapon.Melee;
	}
}
