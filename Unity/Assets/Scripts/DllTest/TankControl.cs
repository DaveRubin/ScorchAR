using DG.Tweening;
using DllTest;
using ScorchEngine;
using ScorchEngine.Server;
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
    public Player PlayerStats {get;private set;}
    public float force;
    public bool active = true;

    void Awake()
    {
        body = transform;
        Sides = transform.FindChild("YAxis");
        UpDwn = transform.FindChild("YAxis/ZAxis");
        Barrel = transform.FindChild("YAxis/ZAxis/Barrel");
        BarrelsEnd = transform.FindChild("YAxis/ZAxis/Barrel/Tip");
        Debug.Log("Found everything");
    }

    public void SetPlayer(Player player) {
        Local = false;
        this.PlayerStats = player;
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
        PlayerStats.OnUpdate -= OnPLayerUpdate;
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

    /// <summary>
    /// Player update controls remote tanks
    /// </summary>
    /// <param name="updatedPlayer"></param>
    public void OnPLayerUpdate(Player updatedPlayer) {
        onLeftRightChanged(updatedPlayer.ControlledTank.AngleHorizontal);
        onUpDownChanged(updatedPlayer.ControlledTank.AngleVertical);
        onForceChange(updatedPlayer.ControlledTank.Force);

        if (updatedPlayer.ControlledTank.IsReady) Shoot();
    }

    /// <summary>
    /// horizontal aim
    /// </summary>
    /// <param name="angle"></param>
    public void onLeftRightChanged(float angle) {
        Vector3 yRotation = Sides.localRotation.eulerAngles;
        yRotation.y = angle;
        PlayerStats.ControlledTank.AngleHorizontal = angle;
        Sides.DOLocalRotate(yRotation,0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// set force
    /// </summary>
    /// <param name="force"></param>
    public void onForceChange(float force) {
        this.force = force;
        PlayerStats.ControlledTank.Force = force;
    }

    /// <summary>
    /// vertical aim
    /// </summary>
    /// <param name="angle"></param>
    public void onUpDownChanged(float angle) {
        Vector3 zRotation = UpDwn.localRotation.eulerAngles;
        zRotation.z = angle;
        PlayerStats.ControlledTank.AngleVertical = angle;
        UpDwn.DOLocalRotate(zRotation,0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// Shoot projectile
    /// </summary>
    public void Shoot() {
        if (PlayerStats.ControlledTank.IsReady) return;
        // set up projectile type
        // shoot it
        GameObject bullet = PrefabManager.InstantiatePrefab("Projectile");
        float addition = 0;
        float angle = UpDwn.eulerAngles.z + addition;
        float fy = Mathf.Sin(angle * Mathf.Deg2Rad) * force;
        float fxMain = Mathf.Cos(angle * Mathf.Deg2Rad) * force;
        Debug.LogFormat("Shooting with force {0}",force);

        //separate xForce and Zforce
        float addition2 = 0;
        float angle2 = Sides.eulerAngles.y + addition2;
        float fz = Mathf.Sin(angle2 * Mathf.Deg2Rad) * fxMain;
        float fx = Mathf.Cos(angle2 * Mathf.Deg2Rad) * fxMain;

        bullet.transform.position = BarrelsEnd.position;
        bullet.GetComponent<ProjectileControl>().SetForce(Barrel,new Vector3(fx, fy, -fz));

        PlayerStats.ControlledTank.IsReady = true;
    }



}
