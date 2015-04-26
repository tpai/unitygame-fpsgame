using UnityEngine;
using System.Collections;

public class csFunctionGroup : MonoBehaviour
{
    #region EnumSetting

    public enum BulletKind { Normal, Explosive };

    public enum DimissionState { _2D,_3D };

    #endregion

    #region Function

    public void ApplyForce(string[] TargetTag, float RigidBodyHitForce, float RigidBodyHitRadious , Vector3 Position)
    {
        for (int j = 0; j < TargetTag.Length; j++)
        {

            GameObject[] Objects = GameObject.FindObjectsOfType<GameObject>();

            for (int i = 0; i < Objects.Length; i++)
            {
                if (Objects[i].tag == TargetTag[j])
                {
                    if(Objects[i].GetComponent<Rigidbody>())
                         Objects[i].GetComponent<Rigidbody>().AddExplosionForce(RigidBodyHitForce, Position, RigidBodyHitRadious, 3.0f);
                }
            }
        }
    }

    public void BarrelRecoil(int RecoilValueOrder)
    {
        if ((csBarrelFunction)transform.gameObject.GetComponent("csBarrelFunction"))
        {
            csBarrelFunction BarrelFunction = this.transform.GetComponent<csBarrelFunction>();
            BarrelFunction.RecoilValue[RecoilValueOrder] += BarrelFunction.RecoilForce;
        }
    }

    public void BarrelRotateKeyInput(KeyCode AttackKey)
    {
        if ((csBarrelFunction)transform.gameObject.GetComponent("csBarrelFunction"))
        {
            csBarrelFunction BarrelFunction = this.transform.GetComponent<csBarrelFunction>();
            BarrelFunction.BarrelRotateKeyCode = AttackKey;
        }
    }

    #endregion
}