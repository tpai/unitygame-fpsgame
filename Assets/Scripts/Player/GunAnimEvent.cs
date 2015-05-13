using UnityEngine;
using System.Collections;

public class GunAnimEvent : PlayerBase {

	public void CombatWeaponHit () {
		GunShooting.CombatWeaponHit ();
	}
	
	public void ClipReloaded () {
		GunShooting.ClipReloaded ();
	}
}
