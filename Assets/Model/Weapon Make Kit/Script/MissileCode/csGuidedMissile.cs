using UnityEngine;
using System.Collections;

public class csGuidedMissile : csFunctionGroup
{
    public float LiveTime = 10; // Missile Survive Time.

    [HideInInspector]
    public Transform Target; // Guid Target.
    public Transform Booster; // Missile's tail effect.

    public Transform ExplosionEffect; // missile's hit effect.
    public BulletKind _BulletKind = BulletKind.Explosive;

    [HideInInspector]
    public bool Guided; // Check Guided
    Quaternion MissileRotation;

    public bool Predict; // Check Predict Target Position.
    Vector3 OldTargetPosition;
    Vector3 NewTargetPosition;

    public bool RigidBodyMove; // Check RigidBody Move.
    public float Speed = 0; // Initial Speed.
    public float SpeedMax; // Max Speed.
    public float SpeedIndex; // Increacing Index that speed every time 

    public enum MissileShakeState { Shake, NoneShake };
    public MissileShakeState MissileShakeType = MissileShakeState.NoneShake;
    public float MissileShakeScale;
    Vector3 ShakeValue;

    //Explosion force values
    public string[] TargetTag;
    public float RigidBodyHitForce = 100;
    public float RigidBodyHitRadious = 100;
    public float TargetMaxLookAngle = 20;

    public DimissionState DimissionType = DimissionState._3D;

    void Start()
    {
        Destroy(gameObject, LiveTime);
        if(Target)
            OldTargetPosition = Target.position;
    }

    float AngleValue(Vector3 Target , Transform _Transform)
    {
        Vector3 DirectionValue = (Target - _Transform.position).normalized;
        float Angle = Vector3.Angle(DirectionValue, _Transform.forward);
        return Angle;
    }

    void FixedUpdate()
    {
        if (MissileShakeType == MissileShakeState.Shake) // Check Random.
            ShakeValue = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        else if(MissileShakeType == MissileShakeState.NoneShake)
            ShakeValue = new Vector3(0, 0, 0);

        if (RigidBodyMove)
        {
            if (DimissionType == DimissionState._3D)
            {
                Vector3 Velocity = Vector3.zero; //set Velocity value that is Vector3 Information.
                Velocity = (transform.rotation * Vector3.forward + ShakeValue * MissileShakeScale) * Speed; //Set Velocity.
                GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Velocity, Time.fixedDeltaTime); // Change this rigidbody's Velocity to Velocity Value via Time.fixedDeltaTime.
            }
            else if (DimissionType == DimissionState._2D)
            {
                Vector2 Velocity = Vector2.zero; //set Velocity value that is Vector3 Information.
                Velocity = (transform.rotation * Vector3.forward + ShakeValue * MissileShakeScale) * Speed;//Set Velocity.
                GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(GetComponent<Rigidbody2D>().velocity, Velocity, Time.fixedDeltaTime);// Change this rigidbody's Velocity to Velocity Value via Time.fixedDeltaTime.
            }
        }
        else if(!RigidBodyMove)
            transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (Target && Guided)
        {
            if (!Predict)
            {
                NewTargetPosition = Target.position; //Set Target Position.

                float Angle = AngleValue(NewTargetPosition,this.transform); // set Angle between NewTargetPosition this transform
                if (Angle > TargetMaxLookAngle) // Check Angle.
                    Target = null; 
            }
            else if (Predict)
            {
                Vector3 AdditionalPosition = Target.position - OldTargetPosition; //Set Vector3 value between Target and OldTargetPosition.
               
                float DistanceValue = (transform.position - Target.position).magnitude / (Speed * Time.smoothDeltaTime); //Set Length between transform.position, Target.position.
               
                NewTargetPosition = Target.position + AdditionalPosition * DistanceValue; //set predicted target position. 
            }

            MissileRotation = Quaternion.LookRotation(NewTargetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, MissileRotation, Time.smoothDeltaTime * 10);
        }

        if (Speed < SpeedMax)
        {
            Speed += SpeedIndex * Time.fixedDeltaTime;
            if (Speed > SpeedMax)
                Speed = SpeedMax;
        }

        if(Predict)
            OldTargetPosition = Target.position;
    }

    void OnCollisionEnter(Collision Col) //Work on 3D collider
    {
        for (int i = 0; i < TargetTag.Length; i++)
        {
            if (Col.gameObject.tag == TargetTag[i])
            {
                if(_BulletKind == BulletKind.Explosive)
                    ApplyForce(TargetTag, RigidBodyHitForce, RigidBodyHitRadious, transform.position);

                Transform Explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation) as Transform;

                Booster.parent = null;
                Destroy(Booster.gameObject, 2);
                Destroy(Explosion.gameObject, 3);
                Destroy(this.gameObject);
                return;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D Col) //Work on 2D collider.
    {
        for (int i = 0; i < TargetTag.Length; i++)
        {
            if (Col.gameObject.tag == TargetTag[i])
            {
                Transform Explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation) as Transform;

                Booster.parent = null;
                Destroy(Booster.gameObject, 2);
                Destroy(Explosion.gameObject, 3);
                Destroy(this.gameObject);

                return;
            }
        }
    }
}