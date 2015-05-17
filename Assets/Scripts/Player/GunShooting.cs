using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class GunShooting : PlayerBase {

	[SerializeField] private CharacterController chrCtrler;
	[SerializeField] private FirstPersonController fpsCtrler;
	[SerializeField] private Animator envAnim;

	bool combatWeapon = false;
	Animator gunAnim;
	Transform gunTop;
	float gunSpeed = .2f;

	bool holdFire = false;
	public bool isArming = false;
	public bool isAiming = false;
	public bool isCrouching = false;
	public bool isSprinting = false;
	public bool isReloading = false;

	bool m_NetworkedIsAiming = false;
	bool m_NetworkedIsCrouching = false;
	bool m_NetworkedIsSprinting = false;
	bool m_NetworkedIsReloading = false;
	Quaternion m_NetworkedLocalRotation;

	void OnEnable () {
		holdFire = false;
		isArming = false;
		isAiming = false;
		isCrouching = false;
		isSprinting = false;
		isReloading = false;
	}

	public void ArmWeapon (bool combat, Animator anim, Transform top, float spd) {
		isArming = false;
		combatWeapon = combat;
		gunAnim = anim;
		gunTop = top;
		gunSpeed = spd;
	}

	void Update () {

		if (!PhotonView.isMine) {
			transform.localRotation = Quaternion.Slerp (transform.localRotation, m_NetworkedLocalRotation, Time.deltaTime * 10f);
			return;
		}

		if (isArming)
			return;

		if (Input.GetMouseButtonDown (1) && !isSprinting) {
			PlayerAiming (true);
		}
		if (Input.GetMouseButtonUp (1)) {
			PlayerAiming (false);
		}

		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			PlayerCrouching ();
		}

		if (Input.GetKeyDown (KeyCode.R) && !isReloading && !combatWeapon) {
			PlayerReloading ();
		}

		if (!isAiming && !isCrouching && Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetAxis ("Vertical") < 0f || (Input.GetAxis ("Horizontal") == 0f && Input.GetAxis ("Vertical") == 0f) || !chrCtrler.isGrounded) {
				PlayerSprinting (false);
			}
			else {
				PlayerSprinting (true);
			}
		}
		else {
			PlayerSprinting (false);
		}
		
		if (Input.GetMouseButton (0) && !holdFire && !isSprinting && !isReloading) {
			StartCoroutine ("ShootBullet");
		}
	}

	IEnumerator ShootBullet () {
		holdFire = true;

		if (gunTop.GetComponent<BulletFlying> ().BulletCount > 0) {
			gunAnim.SetTrigger ("shoot");
		}
		if (!combatWeapon) {
			gunTop.SendMessage ("BulletHit");
		}

		yield return new WaitForSeconds (gunSpeed);
		holdFire = false;
	}

	public void KnifeHit () {
		gunTop.SendMessage ("BulletHit");
	}

	// ---------------------

	void PlayerAiming (bool b) {
		isAiming = b;
		if (!combatWeapon)
			envAnim.SetBool("aim", b);
		gunAnim.SetBool("aim", b);
	}
	
	void PlayerCrouching () {
		isCrouching = !isCrouching;
		envAnim.SetBool("crouch", isCrouching);
		chrCtrler.height = (isCrouching)?.6f:1.8f;
	}
	
	void PlayerReloading () {
		isReloading = true;
		envAnim.SetBool("aim", false);
		gunAnim.SetTrigger ("reload");
		gunTop.SendMessage("PlayReloadSound");
	}
	
	public void PlayerReloaded () {
		isReloading = false;
		gunTop.SendMessage ("ClipReload", 30);
	}
	
	void PlayerSprinting (bool b) {
		isSprinting = b;
		fpsCtrler.RunSpeed = (b)?10f:5f;
		gunAnim.SetBool("sprint", b);
	}

	// ---------------------

	public void SerializeState (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (isAiming);
			stream.SendNext (isCrouching);
			stream.SendNext (isSprinting);
			stream.SendNext (isReloading);
			stream.SendNext (transform.localRotation);
		} else {
			m_NetworkedIsAiming = (bool)stream.ReceiveNext();
			m_NetworkedIsCrouching = (bool)stream.ReceiveNext();
			m_NetworkedIsSprinting = (bool)stream.ReceiveNext();
			m_NetworkedIsReloading = (bool)stream.ReceiveNext();
			m_NetworkedLocalRotation = (Quaternion)stream.ReceiveNext();

			if (m_NetworkedIsAiming != isAiming) {
				PlayerAiming (m_NetworkedIsAiming);
			}
			if (m_NetworkedIsCrouching != isCrouching) {
				PlayerCrouching ();
			}
			if (m_NetworkedIsSprinting != isSprinting) {
				PlayerSprinting (m_NetworkedIsSprinting);
			}
			if (m_NetworkedIsReloading && !isReloading) {
				PlayerReloading ();
			}
		}
	}
}
