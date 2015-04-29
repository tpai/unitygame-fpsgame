using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {

	void Broken (Vector3 point) {

		Debug.Log (point);

		transform.Find ("Barrel").GetComponent<MeshRenderer> ().enabled = false;

		for (int i=1;i<transform.childCount;i++) {
			transform.GetChild(i).gameObject.GetComponent<Rigidbody>().useGravity = true;
			transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = true;
			transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddExplosionForce(500f, point, 100f);
//			transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}
