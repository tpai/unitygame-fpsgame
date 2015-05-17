using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	Backpack m_Backpack;
	public Backpack Backpack {
		get {
			if (m_Backpack == null) {
				m_Backpack = GetComponentInChildren<Backpack> ();
			}
			return m_Backpack;
		}
	}

	GunShooting m_GunShooting;
	public GunShooting GunShooting {
		get {
			if (m_GunShooting == null) {
				m_GunShooting = GetComponentInParent<GunShooting>();
			}
			if (m_GunShooting == null) {
				m_GunShooting = GetComponentInChildren<GunShooting>();
			}
			return m_GunShooting;
		}
	}

	PhotonView m_PhotonView;
	public PhotonView PhotonView {
		get {
			if (m_PhotonView == null) {
				m_PhotonView = GetComponentInParent<PhotonView>();
			}
			return m_PhotonView;
		}
	}
}
