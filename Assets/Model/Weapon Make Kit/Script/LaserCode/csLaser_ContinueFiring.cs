using UnityEngine;
using System.Collections;

public class csLaser_ContinueFiring : csLaserBase
{
    public float ShrinkValue = 0;

    [HideInInspector]
    public bool LaserFireLoop = false;

    private float NowLength; // if Raycast Hit Something, Save Length Information Between this transform , RacastHit's hit point.
    float CurrentWidth = 0.0f;

    public AudioSource LaserFireSound;

    bool isHit; //if Raycast Hit Something, Make this Effect to here.

    void Awake()
    {
        if (!(LineRenderer)transform.gameObject.GetComponent("LineRenderer")) // if LineRenderer is not attach, attach LineRenderer
            this.gameObject.AddComponent<LineRenderer>();

        Transform MakedEffect = Instantiate(LaserHitEffect, transform.position, transform.rotation) as Transform; // Make Effect
        _Effect = MakedEffect.gameObject; // Set MakeEffect Information To _Effect
        
        _LineRenderer = GetComponent<LineRenderer>(); // Set this GameObject LineRenderer Value 
        _LineRenderer.material = _Material; // Set Material
        _LineRenderer.SetWidth(0, 0); // Set Width
        _LineRenderer.SetVertexCount(2);
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void Update()
    {
        _LineRenderer.SetPosition(0, transform.position); //Set this GameObject's Position

        if (DimissionType == DimissionState._2D)
            RaycastCheck2D();
        if (DimissionType == DimissionState._3D)
            RaycastCheck3D();
      
        ChildParticleCheck(); // Contol Child of this gmaeobject's ParticleSystem
        LaserFiringCheck();

        Vector3 NewPos = this.transform.position + new Vector3(this.transform.forward.x * (NowLength)
            , this.transform.forward.y * (NowLength), this.transform.forward.z * (NowLength)); //Set LineRenderer Length throught NowLength's information.                                                                         
        _LineRenderer.SetPosition(1, NewPos); //Set Next SetPosition use NewPos information
        _LineRenderer.material.SetTextureOffset("_MainTex",
            new Vector2(-Time.time * 30f * Offset, 0.0f)); //Because of Movement of Laser, Change x Offset throught Offset Value.
        _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(NowLength/10, _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale.y);
    }

    void RaycastCheck2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , transform.forward, MaxLength);

        if (hit) // if hit do Catch Something, Executing this Function
        {
            if (LaserFireLoop)
                isHit = true; // for Particle Effect Execute, Set ishit to true
            else
                isHit = false;

            NowLength = (new Vector2(transform.position.x, transform.position.y) - hit.point).magnitude;

            _Effect.transform.position = hit.point;
            _Effect.transform.rotation = hit.transform.rotation;
        }
        else // if hit don't Catch Something, Particle Effect didn't Execute. so Set is hit to false , Set NowLength to MaxLength
        {
            isHit = false;
            NowLength = MaxLength;
        }
    }

    void RaycastCheck3D()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, MaxLength)) // if hit do Catch Something, Executing this Function
        {
            if (LaserFireLoop)
            {
                if (_LaserKind == BulletKind.Explosive)
                    ApplyForce(TargetTag, RigidBodyHitForce, RigidbodyHitRadious, hit.point);

                isHit = true; // for Particle Effect Execute, Set ishit to true
            }
            else
                isHit = false;

            NowLength = hit.distance;

            _Effect.transform.position = hit.point;
            _Effect.transform.rotation = hit.transform.rotation;
        }
        else // if hit don't Catch Something, Particle Effect didn't Execute. so Set is hit to false , Set NowLength to MaxLength
        {
            isHit = false;
            NowLength = MaxLength;
        }
    }

    void LaserFiringCheck()
    {
        if (LaserFireLoop == true)
        {
            CurrentWidth += ShrinkValue;
            if (CurrentWidth >= Width)
                CurrentWidth = Width;
        }
        else if (LaserFireLoop == false)
        {
            CurrentWidth -= ShrinkValue;
            if (CurrentWidth <= 0)
            {
                CurrentWidth = 0;
                ChildParticleCheck();
            }
        }

        if (LaserFireSound)
        {
            if (LaserFireSound.pitch > 0)
                LaserFireSound.mute = false;
            else
                LaserFireSound.mute = true;

            LaserFireSound.pitch = CurrentWidth / Width;
        }

        _LineRenderer.SetWidth(CurrentWidth, CurrentWidth);
    }

    void ChildParticleCheck()
    {
        ParticleSystem[] ParticleSystems = _Effect.GetComponentsInChildren<ParticleSystem>(); //Scan All Shuriken Particle inside of _Effect
        for (int i = 0; i < ParticleSystems.Length; i++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
        {
            if (isHit && CurrentWidth > 0)
                ParticleSystems[i].Play();
            else
                ParticleSystems[i].Stop();
        }
    }
}