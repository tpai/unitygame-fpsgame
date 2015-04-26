using UnityEngine;
using System.Collections;

public class csBarrelFunction : MonoBehaviour {

    public Transform[] Barrel;

    public enum BarrelAxis { X, Y, Z };

    //Barrel Recoil Section
    public bool isBarrelRecoil;

    [HideInInspector]
    public float[] RecoilValue;

    [HideInInspector]
    public float[] ChangedBarrelPos;

    public float RecoilForce = 0;
    public float PushedSpeed = 0;
    public float RecoilSpeed = 0;
    public BarrelAxis BarrelRecoilAxis = new BarrelAxis();
    float RecoilAxisX = 0;
    float RecoilAxisY = 0;
    float RecoilAxisZ = 0;

    //Barrel Rotate Section
    public bool isBarrelRotate;

    [HideInInspector]
    public float BarrelRotateSpeed;

    [HideInInspector]
    public KeyCode BarrelRotateKeyCode = KeyCode.A;

    public float MaxBarrelRotateSpeed = 30;
    public BarrelAxis BarrelRotateAxis = new BarrelAxis();
    float RotateAxisX = 0;
    float RotateAxisY = 0;
    float RotateAxisZ = 0;

    void Start()
    {
        RecoilValue = new float[Barrel.Length];
        ChangedBarrelPos = new float[Barrel.Length];
    }

    void Update()
    {
        if (isBarrelRecoil)
            BarrelRecoil();
        if (isBarrelRotate)
            BarrelRotate();
    }

    public void BarrelRotate()
    {
        //Changed BarrelRotateSpeed
        if (Input.GetKey(BarrelRotateKeyCode))
        {
            if (BarrelRotateSpeed < MaxBarrelRotateSpeed)
                BarrelRotateSpeed += Time.deltaTime + 4;
        }
        else
        {
            if (BarrelRotateSpeed > 0)
                BarrelRotateSpeed -= Time.deltaTime + 2;
            else if (BarrelRotateSpeed <= 0)
                BarrelRotateSpeed = 0;
        }
        //Set RecoilAxis Value follow BarrelRotateType.
        for (int i = 0; i < Barrel.Length; i++)
        {
            if (BarrelRotateAxis == BarrelAxis.X)
                RotateAxisX = BarrelRotateSpeed;
            else if (BarrelRotateAxis == BarrelAxis.Y)
                RotateAxisY = BarrelRotateSpeed;
            else if (BarrelRotateAxis == BarrelAxis.Z)
                RotateAxisZ = BarrelRotateSpeed;

            Barrel[i].Rotate(RotateAxisX, RotateAxisY, RotateAxisZ);
        }
    }

    public void BarrelRecoil()
    {
        for (int i = 0; i < Barrel.Length; i++)
        {
            ChangedBarrelPos[i] = Mathf.Lerp(ChangedBarrelPos[i], RecoilValue[i], PushedSpeed * Time.deltaTime); // Increase ChangedBarrelPos Value to RecoilValue.
            //Set RecoilAxis Value follow BarrelRecoilType.
            if (BarrelRecoilAxis == BarrelAxis.X)
            {
                RecoilAxisX = -ChangedBarrelPos[i];
                RecoilAxisY = Barrel[i].localPosition.y;
                RecoilAxisZ = Barrel[i].localPosition.z;
            }
            else if (BarrelRecoilAxis == BarrelAxis.Y)
            {
                RecoilAxisX = Barrel[i].localPosition.x;
                RecoilAxisY = -ChangedBarrelPos[i];
                RecoilAxisZ = Barrel[i].localPosition.z;
            }
            else if (BarrelRecoilAxis == BarrelAxis.Z)
            {
                RecoilAxisX = Barrel[i].localPosition.x;
                RecoilAxisY = Barrel[i].localPosition.y;
                RecoilAxisZ = -ChangedBarrelPos[i];
            }

            Barrel[i].localPosition = new Vector3(RecoilAxisX, RecoilAxisY, RecoilAxisZ); //Set ChangedBarrelPos

            RecoilValue[i] = Mathf.Lerp(RecoilValue[i], 0, RecoilSpeed * Time.deltaTime); // Decrease RecoilValue to 0.
        }
    }
}