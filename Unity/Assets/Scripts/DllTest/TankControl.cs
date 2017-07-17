﻿using DG.Tweening;
using DllTest;
using ScorchEngine;
using UI;
using UnityEngine;
using Utils;
using View;

public class TankControl : MonoBehaviour {

    public bool Local{get;private set;}
    bool updatePathDirtyFlag = false;
    Transform body;
    Transform Sides;
    Transform UpDwn;
    Transform BarrelsEnd;
    Transform Barrel;
    PathTracer Tracer;
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
        Tracer = transform.FindChild("Path").GetComponent<PathTracer>();
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

        Sides.localRotation = Quaternion.Euler(yRotation);
        UpDwn.localRotation = Quaternion.Euler(zRotation);
        body.localPosition = pos;
        if (updatePathDirtyFlag) {
            Tracer.transform.position = BarrelsEnd.position;
            Tracer.SetPath(GetForceVector());
            updatePathDirtyFlag = false;
        }
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
        if (PlayerStats.ControlledTank.AngleHorizontal != angle) {
            updatePathDirtyFlag = true;
        }
        Vector3 yRotation = Sides.localRotation.eulerAngles;
        yRotation.y = angle;
        PlayerStats.ControlledTank.AngleHorizontal = angle;
        Sides.DOLocalRotate(yRotation,0.5f).SetEase(Ease.Linear).OnUpdate(()=>updatePathDirtyFlag = true);

    }

    /// <summary>
    /// set force
    /// </summary>
    /// <param name="force"></param>
    public void onForceChange(float force) {
        if (PlayerStats.ControlledTank.Force != force) {
            updatePathDirtyFlag = true;
        }
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
        UpDwn.DOLocalRotate(zRotation,0.5f).SetEase(Ease.Linear).OnUpdate(()=>updatePathDirtyFlag = true);

    }

    /// <summary>
    /// Shoot projectile
    /// </summary>
    public void Shoot() {
        if (PlayerStats.ControlledTank.IsReady) return;
        // set up projectile type
        GameObject bullet = PrefabManager.InstantiatePrefab("Projectile");
        //...

        // shoot it
        bullet.transform.position = BarrelsEnd.position;
        Vector3 forceVector = GetForceVector();
        Debug.LogFormat("Shooting with force {0}",force);
        bullet.GetComponent<ProjectileControl>().SetForce(Barrel,forceVector);
        PlayerStats.ControlledTank.IsReady = true;
    }

    public Vector3 GetForceVector() {
        Debug.Log("Getting force vector");
        float addition = 0;
        float angle = UpDwn.eulerAngles.z + addition;
        float fy = Mathf.Sin(angle * Mathf.Deg2Rad) * force;
        float fxMain = Mathf.Cos(angle * Mathf.Deg2Rad) * force;

        //separate xForce and Zforce
        float addition2 = 0;
        float angle2 = Sides.eulerAngles.y + addition2;
        float fz = Mathf.Sin(angle2 * Mathf.Deg2Rad) * fxMain;
        float fx = Mathf.Cos(angle2 * Mathf.Deg2Rad) * fxMain;
        return new Vector3(fx, fy, -fz);
    }



}
