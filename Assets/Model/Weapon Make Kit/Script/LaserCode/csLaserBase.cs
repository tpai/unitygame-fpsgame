using UnityEngine;
using System.Collections;

public class csLaserBase : csFunctionGroup
{
    public DimissionState DimissionType = new DimissionState();

    [HideInInspector]
    public LineRenderer _LineRenderer; //LineRenderer Value

    //Raycast Laser Value Section.
    public BulletKind _LaserKind = BulletKind.Explosive;
    public string[] TargetTag;
    public float RigidBodyHitForce;
    public float RigidbodyHitRadious;
    //

    public float Offset = 1.0f; //LineRenderer MainTexture Offset Value 
    public float Width = 1.5f; //LineRenderer Width Value
    public float MaxLength = Mathf.Infinity;
    public Transform LaserHitEffect; //For Laser Hit Effect.
    public Material _Material; //For LineRenderer Material
    [HideInInspector]
    public GameObject _Effect;
}