using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletCount : MonoBehaviour {

	[SerializeField] private BulletFlying bulletFlying;

	void Update () {
		GetComponent<Text> ().text = bulletFlying.BulletCount+"/"+bulletFlying.BulletMaxCount;
	}
}
