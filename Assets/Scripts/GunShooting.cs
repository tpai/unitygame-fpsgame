using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GunShooting : MonoBehaviour {

	public CharacterController chrCtrler;
	public FirstPersonController fpsCtrler;
	public Animator envAnim;
	public Animator gunAnim;
	public Transform gunTop;
	public float gunSpeed = .2f;
	bool holdShoot = false;
	bool isAiming = false;
	bool isReloading = false;

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

		if (!isReloading && Input.GetKeyDown (KeyCode.R)) {
			isReloading = true;
			envAnim.SetBool("aim", false);
			gunAnim.SetTrigger ("reload");
			gunTop.SendMessage("PlayReloadSound");
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
		gunTop.SendMessage ("BulletHit");
		if (gunTop.GetComponent<BulletFlying>().BulletCount > 0)
			gunAnim.SetTrigger("shoot");
		yield return new WaitForSeconds (gunSpeed);
		holdShoot = false;
	}

	public void ClipReloaded () {
		isReloading = false;
		gunTop.SendMessage ("ClipReload", 30);
	}
}
