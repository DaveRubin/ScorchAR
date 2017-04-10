using DG.Tweening;
using DllTest;
using ScorchEngine;
using UI;
using UnityEngine;
using Utils;

public class TankControl : MonoBehaviour {

    public bool Local{get;private set;}
    Transform body;
    Transform Sides;
    Transform UpDwn;
    Transform BarrelsEnd;
    Transform Barrel;
    Player player;
    public float force;
    public bool active = true;

    void Awake() {
        body = transform;
        Sides = transform.FindChild("YAxis");
        UpDwn = transform.FindChild("YAxis/ZAxis");
        Barrel = transform.FindChild("YAxis/ZAxis/Barrel");
        BarrelsEnd = transform.FindChild("YAxis/ZAxis/Barrel/Tip");
        Debug.Log("Found everything");
    }

    public void SetPlayer(Player player) {
        Local = false;
        this.player = player;
        Debug.LogFormat("Setting player id {0}",player.ID);
        player.OnUpdate += OnPLayerUpdate;
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
    /// Link tank to GUI
    /// </summary>
    /// <param name="Gui"></param>
    public void LinkToGUI(CameraGUI Gui) {
        player.OnUpdate -= OnPLayerUpdate;
        Local = true;
        Gui.OnForceChange += onForceChange;
        Gui.OnXAngleChange += onLeftRightChanged;
        Gui.OnYAngleChange += onUpDownChanged;
        Gui.OnShootClicked += Shoot;
    }

    public void UnlinkGUI(CameraGUI Gui) {
        Gui.OnForceChange -= onForceChange;
        Gui.OnXAngleChange -= onLeftRightChanged;
        Gui.OnYAngleChange -= onUpDownChanged;
        Gui.OnShootClicked -= Shoot;
    }


    public void OnPLayerUpdate(Player updatedPlayer) {
        onLeftRightChanged(updatedPlayer.ControlledTank.AngleHorizontal);
        onUpDownChanged(updatedPlayer.ControlledTank.AngleVertical);
        onForceChange(updatedPlayer.ControlledTank.Force);
    }

    /// <summary>
    /// horizontal aim
    /// </summary>
    /// <param name="angle"></param>
    public void onLeftRightChanged(float angle) {
        Vector3 yRotation = Sides.localRotation.eulerAngles;
        yRotation.y = angle;
        Sides.DOLocalRotate(yRotation,0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// set force
    /// </summary>
    /// <param name="force"></param>
    public void onForceChange(float force) {
        this.force = force;
    }

    /// <summary>
    /// vertical aim
    /// </summary>
    /// <param name="angle"></param>
    public void onUpDownChanged(float angle) {
        Vector3 zRotation = UpDwn.localRotation.eulerAngles;
        zRotation.z = angle;
        UpDwn.DOLocalRotate(zRotation,0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// Shoot projectile
    /// </summary>
    public void Shoot() {
// set up projectile type
// shoot it
        GameObject bullet = PrefabManager.InstantiatePrefab("Projectile");
        float addition = 0;
        float angle = UpDwn.eulerAngles.z + addition;
        float fy = Mathf.Sin(angle * Mathf.Deg2Rad) * force;
        float fxMain = Mathf.Cos(angle * Mathf.Deg2Rad) * force;

//separate xForce and Zforce
        float addition2 = 0;
        float angle2 = Sides.eulerAngles.y + addition2;
        float fz = Mathf.Sin(angle2 * Mathf.Deg2Rad) * fxMain;
        float fx = Mathf.Cos(angle2 * Mathf.Deg2Rad) * fxMain;

        bullet.transform.position = BarrelsEnd.position;
        bullet.GetComponent<ProjectileControl>().SetForce(Barrel,new Vector3(fx, fy, -fz));
    }

}
