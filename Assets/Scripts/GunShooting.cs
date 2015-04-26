using UnityEngine;
using System.Collections;

public class GunShooting : MonoBehaviour {

	public Animator envAnim;
	public Animator gunAnim;
	public GameObject bulletPrefab;
	public float gunSpeed = .2f;
	bool holdShoot = false;

	void Update () {
		if (Input.GetMouseButton (0)) {
			if (holdShoot == false)
				StartCoroutine("ShootBullet");
		}

		if (Input.GetMouseButtonDown (1)) {
			envAnim.SetBool("aim", true);
			gunAnim.SetBool("aim", true);
		}
		if (Input.GetMouseButtonUp (1)) {
			envAnim.SetBool("aim", false);
			gunAnim.SetBool("aim", false);
		}
	}

	IEnumerator ShootBullet () {
		holdShoot = true;
		Instantiate(bulletPrefab, transform.Find ("Top").position, transform.Find ("Top").rotation);
		yield return new WaitForSeconds (gunSpeed);
		holdShoot = false;
	}
}
