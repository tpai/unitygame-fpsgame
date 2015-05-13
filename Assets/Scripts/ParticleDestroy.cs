using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour {

	public float duration = 1f;

	void Start () {
		Invoke ("SelfDestruction", duration);
	}
	
	void SelfDestruction () {
		Destroy (gameObject);
	}
}
