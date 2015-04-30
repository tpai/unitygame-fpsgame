using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GunShooting : MonoBehaviour {

	public FirstPersonController fpsCtrler;
	public Animator envAnim;
	public Animator gunAnim;
	public GameObject bulletPrefab;
	public float gunSpeed = .2f;
	bool holdShoot = false;
	bool isAiming = false;

	void Update () {

		if (Input.GetMouseButtonDown (1)) {
			isAiming = true;
			fpsCtrler.RunSpeed = 5f;
			envAnim.SetBool("aim", true);
			gunAnim.SetBool("aim", true);
		}
		if (Input.GetMouseButtonUp (1)) {
			fpsCtrler.RunSpeed = 10f;
			isAiming = false;
			envAnim.SetBool("aim", false);
			gunAnim.SetBool("aim", false);
		}

		if (!isAiming && Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetAxis ("Vertical") < 0f);
			else {
				gunAnim.SetBool("sprint", true);
				if (isAiming)
					gunAnim.SetBool("aim", false);
			}
		}
		else {
			gunAnim.SetBool("sprint", false);

			if (Input.GetMouseButton (0))
				if (holdShoot == false)
					StartCoroutine("ShootBullet");
		}
	}

	IEnumerator ShootBullet () {
		holdShoot = true;
		gunAnim.SetTrigger("shoot");
		Instantiate(bulletPrefab, transform.Find ("Top").position, transform.Find ("Top").rotation);
		yield return new WaitForSeconds (gunSpeed);
		holdShoot = false;
	}
}
