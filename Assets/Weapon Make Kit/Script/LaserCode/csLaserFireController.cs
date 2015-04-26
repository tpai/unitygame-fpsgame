using UnityEngine;
using System.Collections;

public class csLaserFireController : csFunctionGroup
{
    public KeyCode AttackKey = KeyCode.A; 

    public csLaser_ContinueFiring[] ContinueLaser;
    
    public csLaser_MirrorReflect[] ReflectLaser;

    void Awake()
    {
        BarrelRotateKeyInput(AttackKey);
    }

	void Update ()
    {
        if (ContinueLaser.Length > 0)
        {
            for (int i = 0; i < ContinueLaser.Length; i++)
            {
                if (Input.GetKey(AttackKey))
                {
                    BarrelRecoil(i);
                    ContinueLaser[i].LaserFireLoop = true;
                }
                else
                    ContinueLaser[i].LaserFireLoop = false;
            }
        }

        if (ReflectLaser.Length >0)
        {
            for (int i = 0; i < ReflectLaser.Length; i++)
            {
                if (Input.GetKey(AttackKey))
                    ReflectLaser[i].isLaserFire = true;
                else
                    ReflectLaser[i].isLaserFire = false;
            }
        }
	}
}