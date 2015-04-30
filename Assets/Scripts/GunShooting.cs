using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GunShooting : MonoBehaviour {

	public CharacterController chrCtrler;
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
			envAnim.SetBool("aim", true);
			gunAnim.SetBool("aim", true);
		}
		if (Input.GetMouseButtonUp (1)) {
			isAiming = false;
			envAnim.SetBool("aim", false);
			gunAnim.SetBool("aim", false);
		}

		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			chrCtrler.height = .9f;
			envAnim.SetBool("crouch", true);
		}
		if (Input.GetKeyUp (KeyCode.LeftControl)) {
			chrCtrler.height = 1.8f;
			envAnim.SetBool("crouch", false);
		}

		if (!isAiming && !Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetAxis ("Vertical") <= 0f && Input.GetAxis ("Horizontal") == 0f) {
				fpsCtrler.RunSpeed = 5f;
				gunAnim.SetBool("sprint", false);
			}
			else {
				fpsCtrler.RunSpeed = 10f;
				gunAnim.SetBool("sprint", true);
				if (isAiming)
					gunAnim.SetBool("aim", false);
			}
		}
		else {
			fpsCtrler.RunSpeed = 5f;
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
