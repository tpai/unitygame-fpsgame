using UnityEngine;
using System.Collections;

public class csLaser_MirrorReflect : csFunctionGroup
{
    public Material LaserMaterial;

    //Mirror Object Hit Check Value.
    public string[] MirrorTag;
    public Transform MirrorHitEffect;
    //

    //Not Mirror Object Hit Check Value.
    public Transform OtherHitEffect;
    Transform _OtherHitEffect;

    //Making Laser Value.
    public int MaxLaserMakeCount;
    int LaserMakeCount = 0;
    //

    //Able to Check Object Value..
    public float LaserMaxLength = 100;
    float NowLength;
    public float LaserWidth;


    [HideInInspector]
    public bool isLaserFire;

    //Fire Raycast Position and Fire Direction Value.
    Vector3 LaserFirePos;
    Vector3 LaserFireDirt;

    public AudioSource LaserFireSound;

    bool isHit;

    ArrayList Lasers = new ArrayList();
    ArrayList Effects = new ArrayList();

    void Start()
    {
        isHit = false;

        NowLength = LaserMaxLength;

        Transform HitEffect = Instantiate(OtherHitEffect, transform.position, Quaternion.identity) as Transform;
        _OtherHitEffect = HitEffect;
        OtherEffectPlayCheck();
    }

    void Update()
    {
        OtherEffectPlayCheck();

        if (isLaserFire)
        {
            LaserFirePos = transform.position; //set this initial transform's position value.
            LaserFireDirt = transform.forward; // set this initial transform's forward vector value.
            LaserMaterial.SetTextureOffset("_MainTex", new Vector2(-Time.time * 30f * 0.1f, 0.0f)); // Update LaserMaterial texture EveryTime.

            ClearLaserAndEffect(); //Clear laser and mirror hit effect everyframe.

            RaycastCheck();//Make laser and mirror hit effect everyframe

            if (LaserFireSound)
               LaserFireSound.mute = false;
        }
        else
        {
            if (LaserFireSound)
                LaserFireSound.mute = true;

            isHit = false;
            ClearLaserAndEffect(); //Clear laser and mirror hit effect everyframe.

        }

    }

    void RaycastCheck()
    {
        LaserMakeCount = 0;

        NowLength = LaserMaxLength; // set MaxLaserLength.

        while (LaserMakeCount < MaxLaserMakeCount)
        {
            RaycastHit hit;

            if (Physics.Raycast(LaserFirePos, LaserFireDirt, out hit, NowLength)) ///Fire at Saved LaserFirePos , Direction.
            {
                LaserMakeCount++;

                for (int i = 0; i < MirrorTag.Length; i++)//Check Mirror Tag
                {
                    if (hit.collider.tag == MirrorTag[i])
                    {
                        NowLength -= (LaserFirePos - hit.point).magnitude; //Decrease NowLength Value that between LaserFirePos,hit.point's value.

                        CreateLaser(LaserFirePos, hit.point, true);

                        LaserFireDirt = Vector3.Reflect((hit.point - LaserFirePos).normalized, hit.normal); //if hitted Object is Mirror, Change Direction Value to reflection direction.
                        LaserFirePos = hit.point;//if hitted Object is Mirror , Change LaserFirePos to hit.point.
                      
                        isHit = false;

                        break;
                    }
                    else
                    {
                        CreateLaser(LaserFirePos, hit.point, false);
                        _OtherHitEffect.transform.position = hit.point;
                        isHit = true;
                    }
                }
            }
            else
            {
                isHit = false;
                CreateLaser(LaserFirePos, LaserFireDirt * NowLength, true);
                break;
            }
        }
    }

    void CreateLaser(Vector3 StartPos, Vector3 EndPos, bool Mirror)
    {
        GameObject ReflectLaser = new GameObject("Reflect_Laser");

        GameObject LaserObject = Instantiate(ReflectLaser, transform.position, transform.rotation) as GameObject; // Make laser effect.
        LaserObject.transform.parent = transform;
        Lasers.Add(LaserObject); // Add to List.

        Destroy(ReflectLaser);

        LineRenderer laserLine;

        if(LaserObject.GetComponent<LineRenderer>())
            laserLine = LaserObject.GetComponent<LineRenderer>(); //Receive LineRenderer. and Setting that.
        else
        {
            LaserObject.gameObject.AddComponent<LineRenderer>(); //set LineRenderer to LaserObject.
            laserLine = LaserObject.GetComponent<LineRenderer>(); //set LineRenderer Value to LaserLine.
        }
            
        laserLine.material = LaserMaterial;//set Material
        laserLine.SetVertexCount(2); //set Laser Make Point Count
        laserLine.SetPosition(0, StartPos); //set StartPosition.
        laserLine.SetPosition(1, EndPos); //set EndPosition
        laserLine.SetWidth(LaserWidth, LaserWidth); //set Width.
        LaserObject.transform.parent = this.transform; //set Parent Object to this object.

        if (Mirror)
        {
            Transform Effect = Instantiate(MirrorHitEffect) as Transform;
            Effect.transform.position = EndPos;
            Effects.Add(Effect.gameObject);
        }
    }

    void OtherEffectPlayCheck()
    {
        ParticleSystem[] ParticleSystems = _OtherHitEffect.GetComponentsInChildren<ParticleSystem>(); //Scan All Shuriken Particle inside of _Effect
        
        for (int i = 0; i < ParticleSystems.Length; i++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
        {
            if (isHit)
                ParticleSystems[i].Play();
            else
                ParticleSystems[i].Stop();
        }
    }

    void ClearLaserAndEffect()
    {
        foreach (GameObject laser in Lasers)
            Destroy(laser);

        Lasers.Clear();

        foreach (GameObject effect in Effects)
            Destroy(effect);

        Effects.Clear();
    }
}