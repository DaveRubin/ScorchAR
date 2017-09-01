﻿using System;
using DG.Tweening;
using DllTest;
using ScorchEngine;
using ScorchEngine.Items;
using UI;
using UnityEngine;
using Utils;
using View;

public class TankControl : MonoBehaviour {

    public Action<TankControl> onKill;
    public Action<TankControl> onHit;
    public bool Local{get;private set;}
    public Player PlayerStats;
    public float force;
    public bool active = true;

    private bool updatePathDirtyFlag = false;
    private Transform body;
    private Transform UpDwn;
    private PathTracer ProjectilePath;
    private RectTransform HealthMask;
    private PositionMarker positionMarker;
    private Transform cameraTransform;
    private Transform Sides;
    private Transform BarrelsEnd;

    private Player updatePlayer = null;
    private CameraGUI gui;

    void Awake() {
        body = transform;
        Sides = transform.Find("Top");
        UpDwn = transform.Find("Top/Barrel");
        BarrelsEnd = transform.Find("Top/Barrel/Tip");
        ProjectilePath = transform.Find("Path").GetComponent<PathTracer>();
        HealthMask = transform.Find("Health/Remaining").GetComponent<RectTransform>();
        transform.GetComponentInChildren<AlwaysLookAt>().SetTarget(Camera.main.transform);
        positionMarker = GetComponentInChildren<PositionMarker>();
        Tests();
    }

    void Start() {
        ProjectilePath.SetVisible(false);
    }

    /// <summary>
    /// When gets hit remove damage
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage) {
        //PlayerStats.ControlledTank.Health -= damage;
        if (onHit != null)
        {
            onHit(this); 
        }
            
        Debug.LogFormat("Hit for {0} damage, total health - {1}",damage,PlayerStats.ControlledTank.Health);
        PlayerStats.ControlledTank.Damage(EWeaponType.Regular,(int)damage);
        UpdateHealthBar(PlayerStats.ControlledTank.Health);

    }

    public void SetPlayer(Player player) {
        Local = false;
        this.PlayerStats = player;
        Debug.LogFormat("Setting player id {0}",player.ID);
        player.OnUpdate += OnPLayerUpdate;
    }

    void OnCollisionEnter(Collision collision) {
// Check if collided with cube
        Debug.LogFormat("Collided with {0} {1}", collision.gameObject.name, gameObject.name);
    }

    void Update() {
        if (ProjectilePath.visible && updatePathDirtyFlag) {
            ProjectilePath.transform.position = BarrelsEnd.position;
            ProjectilePath.SetPath(GetForceVector());
            updatePathDirtyFlag = false;
        }

        if (updatePlayer != null) {
            UpdatePlayer(updatePlayer);
            updatePlayer = null;
        }

        //check distance from camera
        if (positionMarker.isEnabled) {
            bool isShown = Vector3.Distance(transform.position ,Camera.main.transform.position) > 30;
            if (isShown != positionMarker.gameObject.activeSelf) {
                //Debug.LogError("!!!!!");
                positionMarker.gameObject.SetActive(isShown);
            }
            else {
                //Debug.LogError(isShown);
            }
        }
    }

    /// <summary>
    /// Link tank to GUI (and set
    /// </summary>
    /// <param name="Gui"></param>
    public void LinkToGUI(CameraGUI Gui) {
        Local = true;
        gui = Gui;
        positionMarker.Enable();
        PlayerStats.OnUpdate -= OnPLayerUpdate;
        Gui.OnForceChange += onForceChange;
        Gui.OnXAngleChange += onLeftRightChanged;
        Gui.OnYAngleChange += onUpDownChanged;
        Gui.OnShootClicked += OnGuiShoot;
        Gui.OnShowPath += OnShowPathToggle;
    }

    public void UnlinkGUI(CameraGUI Gui) {
        Gui.OnForceChange -= onForceChange;
        Gui.OnXAngleChange -= onLeftRightChanged;
        Gui.OnYAngleChange -= onUpDownChanged;
        Gui.OnShootClicked -= OnGuiShoot;
        Gui.OnShowPath -= OnShowPathToggle;
    }

    /// <summary>
    /// Player update controls remote tanks
    /// </summary>
    /// <param name="updatedPlayer"></param>
    public void OnPLayerUpdate(Player updatedPlayer) {
        Debug.Log("OnPlayer update....");
        updatePlayer = updatedPlayer;
    }

    private void UpdatePlayer(Player updatedPlayer) {
        onLeftRightChanged(updatedPlayer.ControlledTank.AngleHorizontal);
        onUpDownChanged(updatedPlayer.ControlledTank.AngleVertical);
        onForceChange(updatedPlayer.ControlledTank.Force);
        //UpdateHealthBar(updatedPlayer.ControlledTank.Health);
        Debug.LogErrorFormat("Player {0} got updated",PlayerStats.ID);
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
        //if (PlayerStats.ControlledTank.IsReady)  {
        Debug.Log("AAAAA");
        print("A");
        // set up projectile type
        GameObject bullet = PrefabManager.InstantiatePrefab("Projectile");
        //...

        // shoot it
        bullet.transform.position = BarrelsEnd.position;
        Vector3 forceVector = GetForceVector();
        Debug.LogFormat("Shooting with force {0}",force);
        bullet.GetComponent<ProjectileControl>().SetForce(UpDwn,forceVector);
    }

    public Vector3 GetForceVector() {
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

    /// <summary>
    /// Gets value between 1 - 100 and updates health bar
    /// </summary>
    /// <param name="tankHealth"></param>
    public void UpdateHealthBar(int tankHealth) {
        float normalizedValue = ((float)tankHealth / 100) * 2;
        HealthMask.sizeDelta = new Vector2(normalizedValue,0.3f);
        if (normalizedValue <=0 ) {
            Kill();
        }
    }

    public void Tests() {

        if (false) {
            //test position marker
            DOVirtual.DelayedCall(1,positionMarker.Enable);
            DOVirtual.DelayedCall(5,positionMarker.Disable);
        }

        if (false) {
            //test tank health
            for (int i = 0; i < 10; i++) {
                int j = i;
                DOVirtual.DelayedCall(j*0.5f,()=>UpdateHealthBar(100 - j*10));
            }
        }
    }

    public void OnShowPathToggle(bool val) {
        ProjectilePath.SetVisible(val);
        if (val) updatePathDirtyFlag = true;
    }

    public void Kill() {
        Debug.Log("Killed");
        //Create explosion
        GameObject fire  = PrefabManager.InstantiatePrefab("ExplosionFX");
        fire.transform.position = transform.position;
        GameObject.Destroy(gameObject);
        DOVirtual.DelayedCall(2,()=>GameObject.Destroy(fire));
        if (onKill != null )
            onKill(this);
    }

    public void OnGuiShoot() {
        Debug.LogWarning("OnGuiShoot");
        PlayerStats.ControlledTank.IsReady = true;
        gui.SetLocked(true);
        Shoot();
    }



}
