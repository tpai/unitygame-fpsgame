using UnityEngine;
using System.Collections;


public class csGunBase : csFunctionGroup
{
    public DimissionState DimissionType = DimissionState._3D;

    public Transform[] FirePoint;

    //Catriage Value Section.
    public bool isCatridge;
    public Transform[] CatridgeCasePoint;
    public Transform CatridgeCase;
    //

    //Muzzle Value Section.
    public bool isMuzzle;
    public Transform Muzzle;
    //

    //Raycast Value Section. this is only for Raycast Gun System or Canon Gun System.
    public string[] TargetTag;
    public float RigidBodyHitForce;
    public float RigidBodyHitRadious;
    public BulletKind _BulletKind = BulletKind.Explosive;
    public GameObject RaycastHitEffect;
    public GameObject RaycastFireSound;
    public float RaycastMaxLength;
    //

    [HideInInspector]
    public float FireDelay;
    public float MaxFireDelayTime = 0.5f;

    [HideInInspector]
    public float ReloadDelay = 0;
    public float MaxReloadDelayTime = 2;

    [HideInInspector]
    public int BulletAmmo = 0;
    public int MaxBulletAmmo = 30;
}