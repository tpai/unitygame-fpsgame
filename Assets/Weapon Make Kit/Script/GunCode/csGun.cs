using UnityEngine;
using System.Collections;

public class csGun : csGunBase {

    public KeyCode AttackKey = KeyCode.A; 

    public Transform Bullet;

    public enum AttackState { BothAttack, OrderAttack, RandomAttack };
    public AttackState AttackType = AttackState.BothAttack;

    public enum BulletState { Projectile, Raycast };
    public BulletState BulletType = BulletState.Projectile;

    int FirePointOrder = 0;

    void Awake()
    {
        BarrelRotateKeyInput(AttackKey);
    }

    void Update()
    {
        FireDelay += Time.deltaTime;
        ReloadDelay -= Time.deltaTime;

        if (Input.GetKey(AttackKey))
            WeaponFire();

	}

    public void WeaponFire()
    {
        if (ReloadDelay < 0)
        {
            if (FireDelay > MaxFireDelayTime)
            {
                FireDelay = 0;
                GunFireTypeCheck();
            }
        }
    }

    void GunFireTypeCheck()
    {
        BulletAmmo++;
        if (BulletAmmo >= MaxBulletAmmo)
        {
            BulletAmmo = 0;
            ReloadDelay = MaxReloadDelayTime;
        }

        if (AttackType == AttackState.BothAttack)
        {
            for (int i = 0; i < FirePoint.Length; i++)
                BulletFire(i);
        }

        else if (AttackType == AttackState.OrderAttack)
        {
            BulletFire(FirePointOrder);

            if (FirePointOrder <= FirePoint.Length - 2)
                FirePointOrder++;
            else
                FirePointOrder = 0;
        }
        else if (AttackType == AttackState.RandomAttack)
        {
            int PointLength = FirePoint.Length;
            int RandomValue = Random.Range(0, PointLength);
            BulletFire(RandomValue);
        }
    }
    
    void BulletFire(int Point)
    {
        Vector3 RandomValue = new Vector3(Random.Range(2, -2), Random.Range(2, -2), Random.Range(2, -2)) / 50;

        if (BulletType == BulletState.Projectile)
        {
            Transform FireBullet = Instantiate(Bullet, FirePoint[Point].position, FirePoint[Point].rotation) as Transform; // Make Bullet.
            FireBullet.transform.forward += RandomValue; // Shake Bullet forward Direction.
        }

        else if (BulletType == BulletState.Raycast)
        {
            GameObject Exp;
            GameObject Sound;

            if (RaycastFireSound)
            {
                Sound = Instantiate(RaycastFireSound, FirePoint[Point].position, Quaternion.identity) as GameObject;
                Destroy(Sound, 0.5f);
            }

            if (DimissionType == DimissionState._3D)
            {
                RaycastHit hit; //Raycast Value Set
  
                if (Physics.Raycast(FirePoint[Point].position, FirePoint[Point].forward + RandomValue, out hit, RaycastMaxLength)) //Check the Raycast Hit Object
                {
                    if(_BulletKind == BulletKind.Explosive)
                        ApplyForce(TargetTag, RigidBodyHitForce, RigidBodyHitRadious, hit.point);
                   
                    if (RaycastHitEffect)
                    {
                        Exp = Instantiate(RaycastHitEffect, hit.point, Quaternion.identity) as GameObject;
                        Destroy(Exp.gameObject, 0.5f);
                    }
                }
            }
            else if (DimissionType == DimissionState._2D)
            {
                RaycastHit2D hit = Physics2D.Raycast(FirePoint[Point].position, FirePoint[Point].forward + RandomValue, RaycastMaxLength); //Raycast Value Set

                if (hit) //Check the Raycast Hit Object
                {
                    if (RaycastHitEffect)
                    {
                        Exp = Instantiate(RaycastHitEffect, hit.point, Quaternion.identity) as GameObject;
                        Destroy(Exp.gameObject, 0.5f);
                    }
                }
            }
        }
        if (isMuzzle)
        {
            Transform _Muzzle = Instantiate(Muzzle, FirePoint[Point].position, FirePoint[Point].rotation) as Transform;
            Destroy(_Muzzle.gameObject, 0.2f);
        }

        BarrelRecoil(Point);

        if (isCatridge)
        {
            Transform CatridgeObject = Instantiate(CatridgeCase, CatridgeCasePoint[Point].position, CatridgeCasePoint[Point].rotation) as Transform;
            Destroy(CatridgeObject.gameObject, 4);
        }
    }

}
