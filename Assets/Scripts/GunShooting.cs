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
	bool isCrounching = false;
	bool isSprinting = false;
	bool isReloading = false;

	void Update () {

		if (!isSprinting && Input.GetMouseButtonDown (1)) {
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
			isCrounching = !isCrounching;
			envAnim.SetBool("crouch", isCrounching);
			chrCtrler.height = (isCrounching)?.6f:1.8f;
		}

		if (!isReloading && Input.GetKeyDown (KeyCode.R)) {
			isReloading = true;
			envAnim.SetBool("aim", false);
			gunAnim.SetTrigger ("reload");
			gunTop.SendMessage("PlayReloadSound");
		}

		if (!isAiming && !isCrounching && Input.GetKey (KeyCode.LeftShift)) {
			if (
				Input.GetAxis ("Vertical") < 0f ||
				(
					Input.GetAxis ("Horizontal") == 0f && Input.GetAxis ("Vertical") == 0f
				)
			) {
				isSprinting = false;
				fpsCtrler.RunSpeed = 5f;
				gunAnim.SetBool("sprint", false);
			}
			else {
				if (!chrCtrler.isGrounded) {
					isSprinting = false;
					gunAnim.SetBool("sprint", false);
				}
				else {
					isSprinting = true;
					fpsCtrler.RunSpeed = 10f;
					gunAnim.SetBool("sprint", true);
				}
			}
		}
		else {
			isSprinting = false;
			fpsCtrler.RunSpeed = 5f;
			gunAnim.SetBool("sprint", false);
		}
		
		if (!isSprinting && !isReloading && Input.GetMouseButton (0))
			if (holdShoot == false)
				StartCoroutine("ShootBullet");
	}

	IEnumerator ShootBullet () {
		holdShoot = true;
		if (gunTop.GetComponent<BulletFlying>().BulletCount > 0)
			gunAnim.SetTrigger("shoot");
		gunTop.SendMessage ("BulletHit");
		yield return new WaitForSeconds (gunSpeed);
		holdShoot = false;
	}

	public void ClipReloaded () {
		isReloading = false;
		gunTop.SendMessage ("ClipReload", 30);
	}
}
