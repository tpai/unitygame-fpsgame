using UnityEngine;
using System.Collections;

public class csCameraSmoothMove : MonoBehaviour {

    public GameObject Target;
    public string Tag;
    public float Distance = 10.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

	void LateUpdate ()
    {
        if (!Target)
        {
            GameObject _Target = GameObject.FindGameObjectWithTag(Tag);
            if (_Target)
                Target = _Target;
            else                                                                                                
                return;
        }

        float wantedRotationAngle = Target.transform.eulerAngles.y;
        float wantedHeight = Target.transform.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = Target.transform.position;
        transform.position -= currentRotation * Vector3.forward * Distance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z)                                                                                         ;

        // Always look at the target
        transform.LookAt(Target.transform);


	}
}