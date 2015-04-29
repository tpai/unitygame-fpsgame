using UnityEngine;
using System.Collections;

public class HitObject : MonoBehaviour {

	void OnCollisionEnter (Collision coll) {
		if (coll.collider.name.Contains("Bullet")) {
			transform.parent.SendMessage("Broken", coll.contacts[0].point);
			Destroy (gameObject);
		}
	}
}
