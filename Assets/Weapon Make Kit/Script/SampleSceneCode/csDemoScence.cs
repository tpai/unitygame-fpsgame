using UnityEngine;
using System.Collections;

public class csDemoScence : MonoBehaviour
{

    enum STATE
    {
        Wp1,
        Wp2,
        Wp3
    };

    public string[] stWeapon1;
    public string[] stWeapon2;
    public string[] stWeapon3;

    public Transform[] Weapon1;
    public Transform[] Weapon2;
    public Transform[] Weapon3;

    public Transform TurretMakePoint;
    public Transform CubeMakePoint;

    public GUIText Text;
    public GUIText Text2;

    public GameObject Cube;
    public GameObject Cube2;
    public GameObject Cube3;

    GameObject _Cube;
    Transform _Weapon;

    int Weapon1Length;
    int Weapon2Length;
    int Weapon3Length;
    int i;
    int Sum;

    STATE _State = new STATE();

    public bool is3D = true;

    void Start()
    {
        i = 1;
        Weapon1Length = Weapon1.Length;
        Weapon2Length = Weapon2.Length;
        Weapon3Length = Weapon3.Length;

        Sum = Weapon1Length + Weapon2Length + Weapon3Length;

        _State = STATE.Wp1;

        MakeEffect();
        MakeCube();
        //Text2.text = "1.LaserBeam1";
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            if ((i - 1) <= Sum-2)
                i++;
            else
                i = 1;

            ChangeState();
            MakeEffect();
            MakeCube();
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if ((i - 1) > 0)
                i--;
            else
                i = Sum;

            ChangeState();
            MakeEffect();
            MakeCube();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState();
            MakeEffect();
            MakeCube();
        }
    }

    void MakeCube()
    {
        switch (_State)
        {
            case STATE.Wp1:
                if (_Cube)
                    Destroy(_Cube.gameObject);
                _Cube = Instantiate(Cube, CubeMakePoint.transform.position, CubeMakePoint.transform.rotation) as GameObject;
                break;
            case STATE.Wp2:
                if (_Cube)
                    Destroy(_Cube.gameObject);
                _Cube = Instantiate(Cube2, CubeMakePoint.transform.position, CubeMakePoint.transform.rotation) as GameObject;
                break;
            case STATE.Wp3:
                if (_Cube)
                    Destroy(_Cube.gameObject);
                _Cube = Instantiate(Cube3, CubeMakePoint.transform.position, CubeMakePoint.transform.rotation) as GameObject;
                break;
            default:
                break;
        }
    }

    void ChangeState()
    {
        if (i >= 1 && i <= Weapon1Length)
            _State = STATE.Wp1;
        else if
           (i > Weapon1Length && i <= Weapon1Length + Weapon2Length)
            _State = STATE.Wp2;
        else if
           (i > Weapon1Length + Weapon2Length && i <= Weapon1Length + Weapon2Length + Weapon3Length)
            _State = STATE.Wp3;
    }

    void MakeEffect()
    {
        switch (_State)
        {
            case STATE.Wp1:
                if (_Weapon)
                {
                    csCrossHair[] CrossHair = _Weapon.GetComponentsInChildren<csCrossHair>(); //Scan All Shuriken Particle inside of _Effect
                    for (int j = 0; j < CrossHair.Length; j++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
                        CrossHair[j].DestroyCrossHair();

                    csCanon[] Canon = _Weapon.GetComponentsInChildren<csCanon>(); //Scan All Canon Attaced Object in _Weapon
                    for (int j = 0; j < Canon.Length; j++)
                        Canon[j].DestroyPointLaser();
                    Destroy(_Weapon.gameObject);
                }
                _Weapon = Instantiate(Weapon1[i - 1], TurretMakePoint.transform.position, TurretMakePoint.transform.rotation) as Transform;
                Text.text = i + "." + stWeapon1[i - 1];
                Text2.text = "";
                break;
            case STATE.Wp2:
                if (_Weapon)
                {
                    csCrossHair[] CrossHair = _Weapon.GetComponentsInChildren<csCrossHair>(); //Scan All Shuriken Particle inside of _Effect
                    for (int j = 0; j < CrossHair.Length; j++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
                        CrossHair[j].DestroyCrossHair();

                    csCanon[] Canon = _Weapon.GetComponentsInChildren<csCanon>(); //Scan All Canon Attaced Object in _Weapon
                    for (int j = 0; j < Canon.Length; j++)
                        Canon[j].DestroyPointLaser();

                    Destroy(_Weapon.gameObject);
                }
                _Weapon = Instantiate(Weapon2[i - 1 - Weapon1Length], TurretMakePoint.transform.position, TurretMakePoint.transform.rotation) as Transform;
                Text.text = i + "." + stWeapon2[i - 1 - Weapon1Length];
                Text2.text = "";
                break;
            case STATE.Wp3:
                if (_Weapon)
                {
                    csCrossHair[] CrossHair = _Weapon.GetComponentsInChildren<csCrossHair>(); //Scan All Shuriken Particle inside of _Effect
                    for (int j = 0; j < CrossHair.Length; j++) //if isHIt is true, All Child Shuriken Effect Play, if not, All Child Shuriken Effect Stop.
                        CrossHair[j].DestroyCrossHair();

                    csCanon[] Canon = _Weapon.GetComponentsInChildren<csCanon>(); //Scan All Canon Attaced Object in _Weapon
                    for (int j = 0; j < Canon.Length; j++)
                        Canon[j].DestroyPointLaser();// Destroy PointLaser.
                    Destroy(_Weapon.gameObject);
                }
                _Weapon = Instantiate(Weapon3[i - 1 - Weapon1Length - Weapon2Length], TurretMakePoint.transform.position, TurretMakePoint.transform.rotation) as Transform;
                Text.text = i + "." + stWeapon3[i - 1 - Weapon1Length - Weapon2Length];
                if (is3D)
                    Text2.text = "'Yellow'Cubs are Mirror Target";
                else
                    Text2.text = "";
                break;
            default:
                break;
        }
    }
    
}
