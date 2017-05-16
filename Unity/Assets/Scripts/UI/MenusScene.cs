using System;
using System.Collections.Generic;
using ScorchEngine.Server;
using UI;
using UnityEngine;

public class MenusScene : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject settings;
    private GameObject lobby;
    private EScreenType _current = EScreenType.Lobby;
    private Dictionary<EScreenType, GameObject> menusTypeMap;
    private static MenusScene instance;

    public EScreenType Current {
        get {
            return _current;
        }
        set {
            _current = value;
            UpdateItemsVisibility();
        }
    }

    public static void BackToMain() {
        instance.Current = EScreenType.MainMenu;
    }

    /// <summary>
    /// Initialize
    /// </summary>
    void Awake() {
        ScorchEngine.Game.debugLog += s => Debug.LogError(s);
        //Debug.LogError("before Test()");
        //ServerWrapper.Test();
        //Debug.LogError("After Test()");
        _current = EScreenType.Lobby;
        if (instance == null) {
            instance = this;
            menusTypeMap = new Dictionary<EScreenType, GameObject>() {
                {EScreenType.MainMenu,transform.Find("Main Menu").gameObject},
                {EScreenType.Lobby,transform.Find("Lobby").gameObject},
                {EScreenType.Settings,transform.Find("Settings").gameObject},
            };
            Current = EScreenType.MainMenu;
            UpdateItemsVisibility();
        }
        else {
            throw new Exception("Menu Scene object is already exist");
        }
    }

    /// <summary>
    /// When object destroy, check if its the current static instance
    /// </summary>
    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }

    /// <summary>
    /// Set the current menu item true and turn off all the others
    /// </summary>
    private void UpdateItemsVisibility() {
        foreach (KeyValuePair<EScreenType, GameObject> pair in menusTypeMap) {
            bool isCurrent = pair.Key == Current;
            pair.Value.SetActive(isCurrent);
        }
    }

    public static void GoTo(EScreenType menu) {
        instance.Current = menu;
    }

}
