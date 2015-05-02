using UnityEngine;
using System.Collections;

public class BulletFlying : MonoBehaviour {

	[SerializeField] private GameObject hitPrefab;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] bulletSound;
	[SerializeField] private float _fireDistance = 100f;

	void Update () {
		Debug.DrawRay(transform.position, transform.forward * _fireDistance, Color.green);
	}

	void BulletHit () {
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray (transform.position, transform.forward);
		if (hitPrefab != null && Physics.Raycast (ray, out hit, _fireDistance)){
			GameObject obj = (GameObject)Instantiate(hitPrefab, hit.point, Quaternion.FromToRotation (Vector3.forward, hit.normal));
			StartCoroutine ( DestroyBullet( obj ) );

			audioSource.clip = bulletSound [Random.Range(0, bulletSound.Length)];
			audioSource.Play ();
		}
	}

	IEnumerator DestroyBullet (GameObject obj) {
		yield return new WaitForSeconds (2f);
		Destroy (obj);
	}
}
