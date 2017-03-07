using DllTest;
using UnityEngine;

public class TankControl : MonoBehaviour {

    GameObject bulletPrefab;
    Transform body;
    Transform Sides;
    Transform UpDwn;
    Transform BarrelsEnd;
    public float force;
    public bool active = true;

    void Awake() {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile");
        body = transform;
        Sides = transform.FindChild("YAxis");
        UpDwn = transform.FindChild("YAxis/ZAxis");
        BarrelsEnd = transform.FindChild("YAxis/ZAxis/Barrel/Tip");
    }


    void Update() {
        Vector3 pos = body.localPosition;
        Vector3 yRotation = Sides.localRotation.eulerAngles;
        Vector3 zRotation = UpDwn.localRotation.eulerAngles;

//Vector3 rotation = YAxis.localRotation.To
        if (Input.GetKey(KeyCode.A)) {
            pos.z += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            pos.z -= 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            pos.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            pos.x += 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            yRotation.y -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            yRotation.y += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            zRotation.z += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            zRotation.z -= 1;
        }

        Sides.localRotation = Quaternion.Euler(yRotation);
        UpDwn.localRotation = Quaternion.Euler(zRotation);
        body.localPosition = pos;
    }

    /// <summary>
    /// horizontal aim
    /// </summary>
    /// <param name="angle"></param>
    public void onLeftRightChanged(float angle) {
        Vector3 yRotation = Sides.localRotation.eulerAngles;
        yRotation.y = angle;
        Sides.localRotation = Quaternion.Euler(yRotation);
    }

    /// <summary>
    /// vertical aim
    /// </summary>
    /// <param name="angle"></param>
    public void onUpDownChanged(float angle) {
        Vector3 zRotation = UpDwn.localRotation.eulerAngles;
        zRotation.z = angle;
        UpDwn.localRotation = Quaternion.Euler(zRotation);
    }

    /// <summary>
    /// Shoot projectile
    /// </summary>
    public void Shoot() {
        // set up projectile type
        // shoot it
        GameObject bullet= GameObject.Instantiate(bulletPrefab);
        float addition = 0;
        float angle = UpDwn.eulerAngles.z + addition;
        float fy = Mathf.Sin(angle*Mathf.Deg2Rad)*force;
        float fxMain = Mathf.Cos(angle*Mathf.Deg2Rad)*force;

        //separate xForce and Zforce
        float addition2 = 0;
        float angle2 = Sides.eulerAngles.y + addition2;
        float fz = Mathf.Sin(angle2*Mathf.Deg2Rad)*fxMain;
        float fx = Mathf.Cos(angle2*Mathf.Deg2Rad)*fxMain;

        bullet.transform.position = BarrelsEnd.position;
        bullet.GetComponent<ProjectileControl>().SetForce(new Vector3(fx,fy,-fz));
    }
}
