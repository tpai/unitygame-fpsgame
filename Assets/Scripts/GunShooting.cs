using UnityEngine;
using System.Collections;

public class GunShooting : MonoBehaviour {

	public GameObject bulletPrefab;
	public float gunSpeed = .2f;
	bool holdShoot = false;

	void Update () {
		if (Input.GetMouseButton (0)) {
			if (holdShoot == false)
				StartCoroutine("ShootBullet");
		}
	}

	IEnumerator ShootBullet () {
		holdShoot = true;
		Instantiate(bulletPrefab, transform.Find ("Top").position, transform.Find ("Top").rotation);
		yield return new WaitForSeconds (gunSpeed);
		holdShoot = false;
	}
}
