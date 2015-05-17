using UnityEngine;
using System.Collections;

public class GunAnimEvent : PlayerBase {

	public void KnifeHit () {
		GunShooting.KnifeHit ();
	}
	
	public void PlayerReloaded () {
		GunShooting.PlayerReloaded ();
	}
}
