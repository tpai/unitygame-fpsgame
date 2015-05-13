using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {

	public float explosionForce = 300f;
	public float disappearDuration = 3f;

	void Broken (Vector3 point) {

		GetComponent<AudioSource> ().Play ();
		transform.GetChild(0).GetComponent<MeshRenderer> ().enabled = false;
		transform.GetChild(0).GetComponent<MeshCollider> ().enabled = false;

		for (int i=1;i<transform.childCount;i++) {
			transform.GetChild(i).gameObject.GetComponent<Rigidbody>().useGravity = true;
			transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = true;
//			transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, point, 100f);
			transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce ((transform.position - point) * explosionForce);
		}

		Invoke ("DestroyFragments", disappearDuration);
	}

	void DestroyFragments () {
		Destroy (transform.gameObject);
	}
}
