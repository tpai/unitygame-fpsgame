using UnityEngine;
using System.Collections;

public class csCanon : csGunBase {

    public KeyCode AttackKey = KeyCode.A; 

    public Transform[] RaycastPoint;

    Vector3[] AttackPos; //Receive RaycastPoint.

    public Transform PointEffect;

    Transform[] _PointEffect;

    RaycastHit Hit;

    float RaycastHitLength;

    void Awake()
    {
        FireDelay = MaxFireDelayTime;
        MakePointLaser();
        BarrelRotateKeyInput(AttackKey);
    }

    public void MakePointLaser()
    {
        _PointEffect = new Transform[RaycastPoint.Length]; //Set PointEffect.

        for (int i = 0; i < RaycastPoint.Length; i++) //Make Point Laser Effect
        {
            Transform Obj = Instantiate(PointEffect, transform.position, Quaternion.identity) as Transform;
            _PointEffect[i] = Obj;
        }
    }

    public void DestroyPointLaser()
    {
        for (int i = 0; i < _PointEffect.Length; i++)
        {
            if (_PointEffect[i])
                Destroy(_PointEffect[i].gameObject);
        }
    }

    void RacasyHitCheck()
    {
        for (int i = 0; i < RaycastPoint.Length; i++)
        {
            if (DimissionType == DimissionState._3D)
            {
                if (Physics.Raycast(RaycastPoint[i].position, RaycastPoint[i].forward, out Hit, RaycastMaxLength)) //Check the Raycast Hit Object
                {
                    _PointEffect[i].transform.position = Hit.point;
                    RaycastHitLength = Hit.distance;
                }
            }

            if (DimissionType == DimissionState._2D)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPoint[i].position, RaycastPoint[i].forward, RaycastMaxLength); //Check the Raycast Hit Object
                if (hit)
                {
                    _PointEffect[i].transform.position = hit.point;
                    RaycastHitLength = (new Vector2(transform.position.x, transform.position.y) - hit.point).magnitude;
                }
            }
        }
    }

    void Update()
    {
        FireDelay += Time.deltaTime;
        ReloadDelay -= Time.deltaTime;

        RacasyHitCheck();

        if (Input.GetKey(AttackKey)) //Fire Weapon
        {
            if (ReloadDelay < 0)
            {
                if (FireDelay > MaxFireDelayTime)
                {
                    FireDelay = 0;
                    for (int i = 0; i < FirePoint.Length; i++)
                    {
                        AttackPosSet();
                        StartCoroutine(Fire(i, RaycastHitLength / 20));
                    }
                }
            }
        }
	}

    void AttackPosSet()
    {
        AttackPos = new Vector3[RaycastPoint.Length];

        for (int i = 0; i < RaycastPoint.Length; i++)
        {
            Vector3 RandomValue = new Vector3(Random.Range(2, -2), Random.Range(2, -2), Random.Range(2, -2)) / 100;

            if (DimissionType == DimissionState._3D)
            {
                if (Physics.Raycast(RaycastPoint[i].position, RaycastPoint[i].forward + RandomValue, out Hit, RaycastMaxLength)) //Check the Raycast Hit Object
                    AttackPos[i] = Hit.point;
            }

            if (DimissionType == DimissionState._2D)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPoint[i].position, RaycastPoint[i].forward + RandomValue, RaycastMaxLength);
                if (hit)
                    AttackPos[i] = hit.point;
            }
        }
    }

    public IEnumerator Fire(int FirePointOrder ,float _Time)
    {
        BulletAmmo++;
        if (BulletAmmo >= MaxBulletAmmo)
        {
            BulletAmmo = 0;
            ReloadDelay = MaxReloadDelayTime;
        }

        if (RaycastFireSound)
        {
            GameObject Sound = Instantiate(RaycastFireSound, FirePoint[FirePointOrder].position, FirePoint[FirePointOrder].rotation) as GameObject;
            Destroy(Sound.gameObject, 2);
        }

        if (isMuzzle)
        {
            Transform _Muzzle = Instantiate(Muzzle, FirePoint[FirePointOrder].position, FirePoint[FirePointOrder].rotation) as Transform;
            Destroy(_Muzzle.gameObject, 0.2f);
        }

        if (isCatridge)
        {
            Transform CatridgeObject = Instantiate(CatridgeCase, CatridgeCasePoint[FirePointOrder].position, CatridgeCasePoint[FirePointOrder].rotation) as Transform;
            Destroy(CatridgeObject.gameObject, 4);
        }

        BarrelRecoil(FirePointOrder);
       
        yield return new WaitForSeconds(_Time); // Delay time that is decided Distance

        if(_BulletKind == BulletKind.Explosive)
            ApplyForce(TargetTag, RigidBodyHitForce, RigidBodyHitRadious, AttackPos[FirePointOrder]);

        GameObject Exp = Instantiate(RaycastHitEffect, AttackPos[FirePointOrder], Quaternion.identity) as GameObject;
        Destroy(Exp, 2);
        
    }
}