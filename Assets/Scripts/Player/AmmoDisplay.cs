using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoDisplay : PlayerBase {

	[SerializeField] private Text ammoText;

	int nowAmmo;
	int maxAmmo;

	void Start () {
		GetComponentInChildren<Backpack> ().OnWeaponChanged += WeaponChanged;
	}

	void WeaponChanged () {
		BulletFlying = null;
	}

	void FixedUpdate () {
		if (nowAmmo != BulletFlying.BulletCount || maxAmmo != BulletFlying.BulletMaxCount) {
			nowAmmo = BulletFlying.BulletCount;
			maxAmmo = BulletFlying.BulletMaxCount;
			ammoText.text = "Ammo: " + nowAmmo + "/" + BulletFlying.BulletMaxCount;
		}
	}
}
