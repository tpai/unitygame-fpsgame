using UnityEngine;
using System.Collections;

public class BulletFlying : MonoBehaviour {

	public bool combatWeapon = false;
	[SerializeField] private GameObject hitPrefab;
	[SerializeField] private GameObject muzzleFlash;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] bulletSound;
	[SerializeField] private AudioClip reloadSound;
	[SerializeField] private AudioClip noAmmoSound;
	[SerializeField] private float _fireDistance = 100f;
	[SerializeField] private float rayBackwardOffset = 10f;
	[SerializeField] private int bulletCount = 30;
	public int BulletCount { get { return bulletCount; } }
	[SerializeField] private int bulletMaxCount = 30;
	public int BulletMaxCount { get { return bulletMaxCount; } }

	void Update () {
		Debug.DrawRay(transform.TransformPoint(Vector3.back * rayBackwardOffset), transform.forward * _fireDistance, Color.green);
	}

	void BulletHit () {
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray (transform.TransformPoint(Vector3.back * rayBackwardOffset), transform.forward);
		if (bulletCount > 0) {
			if (!combatWeapon) {
				bulletCount --;
				Instantiate (muzzleFlash, transform.position, transform.rotation);
			}
			
			if (hitPrefab != null && Physics.Raycast (ray, out hit, _fireDistance)){

				if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
					Instantiate(hitPrefab, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
				}

				if (hit.collider.tag == "Enemy")
					if (hit.collider.name.Contains ("Barrel"))
						hit.collider.transform.parent.SendMessage ("Broken", hit.point, SendMessageOptions.DontRequireReceiver);
			}

			SoundPlay (bulletSound [Random.Range(0, bulletSound.Length)]);
		}
		else {
			SoundPlay (noAmmoSound);
		}
	}

	void PlayReloadSound () {
		SoundPlay (reloadSound);
	}

	void ClipReload (int amt) {
		bulletCount += amt;
		if (bulletCount > bulletMaxCount)
			bulletCount = bulletMaxCount;
	}

	void SoundPlay (AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play ();
	}
}
