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
			GameObject muzObj = (GameObject)Instantiate (muzzleFlash, transform.position, transform.rotation);
			StartCoroutine ( DestroyBulletHole(muzObj, 2f) );
			
			if (hitPrefab != null && Physics.Raycast (ray, out hit, _fireDistance)){

				if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground") {
					GameObject hitObj = (GameObject)Instantiate(hitPrefab, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
					StartCoroutine ( DestroyBulletHole(hitObj, 2f) );
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

	IEnumerator DestroyBulletHole (GameObject obj, float duration) {
		yield return new WaitForSeconds (duration);
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
