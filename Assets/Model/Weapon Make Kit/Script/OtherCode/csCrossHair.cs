using UnityEngine;
using System.Collections;

public class csCrossHair : MonoBehaviour {

    public Transform PointEffect;
    public GUITexture CrossHair;
    GUITexture _CrossHair;

	void Update ()
    {
        Ray Ray = new Ray(transform.position, transform.forward);
        Vector3 CrossHairPosition = Camera.main.WorldToViewportPoint(Ray.GetPoint(50));
        if (_CrossHair == null)
            _CrossHair = Instantiate(CrossHair, CrossHairPosition, Quaternion.identity) as GUITexture;
        else
            _CrossHair.transform.position = CrossHairPosition;
	}

    public void DestroyCrossHair()
    {
        if (_CrossHair)
            Destroy(_CrossHair.gameObject);
    }
}