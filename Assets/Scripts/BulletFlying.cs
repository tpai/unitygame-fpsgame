using UnityEngine;
using System.Collections;

public class BulletFlying : MonoBehaviour {

	[SerializeField] private GameObject hitPrefab;
	[SerializeField] private GameObject muzzleFlash;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] bulletSound;
	[SerializeField] private AudioClip reloadSound;
	[SerializeField] private AudioClip noAmmoSound;
	[SerializeField] private float _fireDistance = 100f;
	[SerializeField] private int bulletCount = 30;
	public int BulletCount { get { return bulletCount; } }
	[SerializeField] private int bulletMaxCount = 30;
	public int BulletMaxCount { get { return bulletMaxCount; } }

	void Update () {
		Debug.DrawRay(transform.position, transform.forward * _fireDistance, Color.green);
	}

	void BulletHit () {
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray (transform.position, transform.forward);
		if (bulletCount > 0) {
			bulletCount --;
			Instantiate (muzzleFlash, transform.position, transform.rotation);

			if (hitPrefab != null && Physics.Raycast (ray, out hit, _fireDistance)){

				if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
					GameObject obj = (GameObject)Instantiate(hitPrefab, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
					StartCoroutine ( DestroyBulletHole( obj ) );
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

	IEnumerator DestroyBulletHole (GameObject obj) {
		yield return new WaitForSeconds (2f);
		Destroy (obj);
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
