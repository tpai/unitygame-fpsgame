using UnityEngine;
using System.Collections;

public class csGunBullet : csFunctionGroup
{

    public float LifeTime;
    float _Time;

    public float RigidBodyHitForce = 100;
    public float RigidBodyHitRadious = 100;

    public Transform HitEffect;

    public string[] TargetTag;

    public float Speed;

    public BulletKind _BulletKind = new BulletKind();
    
    void Update()
    {
        _Time += Time.deltaTime;
        if (_Time > LifeTime)
        {
            if (_BulletKind == BulletKind.Explosive)
                ApplyForce(TargetTag, RigidBodyHitForce, RigidBodyHitRadious, transform.position);

            Transform Explosion = Instantiate(HitEffect, transform.position, transform.rotation) as Transform;
            Destroy(Explosion.gameObject, 0.5f);
            Destroy(gameObject);
        }

        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision Col)
    {
        if (_BulletKind == BulletKind.Explosive)
            ApplyForce(TargetTag, RigidBodyHitForce, RigidBodyHitRadious, transform.position);

        for (int i = 0; i < TargetTag.Length; i++)
        {
            if (Col.gameObject.tag == TargetTag[i])
            {
                Destroy(this.gameObject);
                Transform Explosion = Instantiate(HitEffect, transform.position, transform.rotation) as Transform;
                Destroy(Explosion.gameObject, 0.5f);
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        for (int i = 0; i < TargetTag.Length; i++)
        {
            if (Col.gameObject.tag == TargetTag[i])
            {
                Destroy(this.gameObject);
                Transform Explosion = Instantiate(HitEffect, transform.position, transform.rotation) as Transform;
                Destroy(Explosion.gameObject, 0.5f);
                break;
            }
        }
    }

}
