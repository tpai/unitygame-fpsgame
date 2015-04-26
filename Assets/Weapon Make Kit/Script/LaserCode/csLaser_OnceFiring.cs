using UnityEngine;
using System.Collections;


public class csLaser_OnceFiring : csLaserBase
{
    public float AlphaSpeed;
    float AlphaValue = 1.0f;

    public Color LaserColor;

    private float NowLength; // if Raycast Hit Something, Save Length Information Between this transform , RacastHit's hit point.

    void Start()
    {
        _LineRenderer = GetComponent<LineRenderer>(); //LineRenderer Set

        _LineRenderer.material = _Material;
        _LineRenderer.SetWidth(Width, Width);
        _LineRenderer.SetColors(LaserColor, LaserColor);
        _LineRenderer.SetVertexCount(2);
        _LineRenderer.SetPosition(0, transform.position);

        if (DimissionType == DimissionState._2D)
            RaycastCheck2D();
        if (DimissionType == DimissionState._3D)
            RaycastCheck3D();

        Vector3 NewPos = this.transform.position + new Vector3(transform.forward.x * (NowLength)
           , transform.forward.y * (NowLength), transform.forward.z * (NowLength)); //Set Next Position Use the NowLength

        _LineRenderer.SetPosition(1, NewPos); //LineRenderer 2 Position Set.

        Transform Obj = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as Transform; // Make Effect.
        Obj.transform.position = NewPos;
        //Obj.transform.rotation = hit.collider.transform.rotation;
        Obj.transform.parent = this.transform;
        Destroy(gameObject, 2f);
    }

    void RaycastCheck2D()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position , transform.forward, MaxLength); //Raycast Value Set

        if (hit) //Check the Raycast Hit Object
            NowLength = (new Vector2(transform.position.x, transform.position.y) - hit.point).magnitude;
        else
            NowLength = MaxLength;
    }

    void RaycastCheck3D()
    {
        RaycastHit hit; //Raycast Value Set

        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength)) //Check the Raycast Hit Object
        {
            if (_LaserKind == BulletKind.Explosive)
                ApplyForce(TargetTag, RigidBodyHitForce, RigidbodyHitRadious, hit.point);

            NowLength = hit.distance;
        }
        else
            NowLength = MaxLength;
    }

    void Update()
    {
        _LineRenderer.material.SetTextureOffset("_MainTex",
            new Vector2(-Time.time * 30f * Offset, 0.0f)); //Because of Movement of Laser, Change x Offset throught Offset Value.

        AlphaValue -= Time.deltaTime * AlphaSpeed; // For disapperaing LineRenderer Texture, Alpha Value decreace.

        _LineRenderer.GetComponent<Renderer>().material.SetColor("_TintColor",
            new Color(LaserColor.r,LaserColor.g,LaserColor.b,AlphaValue)); // color or alpha value set.
    }
}