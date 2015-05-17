using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerNetworkMover : PlayerBase {

	public float posSpeed = 10f;
	public float rotSpeed = 10f;

	Vector3 m_NetworkedPosition;
	Quaternion m_NetworkedRotation;

	void Start () {
		if (GetComponent<PhotonView>().isMine) {
			GetComponent<FirstPersonController> ().enabled = true;
			GetComponentInChildren<Camera> ().enabled = true;
			GetComponentInChildren<AudioListener> ().enabled = true;
			GetComponentInChildren<GunShooting>().enabled = true;
		}
	}

	void Update () {
		if (!GetComponent<PhotonView>().isMine) {
			UpdateNetworkedPosition ();
			UpdateNetworkedRotation ();
		}
	}

	void UpdateNetworkedPosition () {
		transform.position = Vector3.Lerp (transform.position, m_NetworkedPosition, Time.deltaTime * posSpeed);
	}

	void UpdateNetworkedRotation () {
		transform.rotation = Quaternion.Lerp (transform.rotation, m_NetworkedRotation, Time.deltaTime * rotSpeed);
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		SerializeState (stream, info);
		Backpack.SerializeState (stream, info);
		GunShooting.SerializeState (stream, info);
	}

	void SerializeState (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		}
		else {
			m_NetworkedPosition = (Vector3)stream.ReceiveNext ();
			m_NetworkedRotation = (Quaternion)stream.ReceiveNext ();
		}
	}
}
