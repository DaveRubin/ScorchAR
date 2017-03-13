using System;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;

public class MenusScene : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject settings;
    private GameObject lobby;
    private MenuType _current = MenuType.MainMenu;
    private Dictionary<MenuType, GameObject> menusTypeMap;
    private static MenusScene instance;

    public MenuType Current {
        get {
            return _current;
        }
        set {
            _current = value;
            UpdateItemsVisibility();
        }
    }

    public static void BackToMain() {
        instance.Current = MenuType.MainMenu;
    }

    /// <summary>
    /// Initialize
    /// </summary>
    void Awake() {
        if (instance == null) {
            instance = this;
            menusTypeMap = new Dictionary<MenuType, GameObject>() {
                {MenuType.MainMenu,transform.Find("Main Menu").gameObject},
                {MenuType.Lobby,transform.Find("Lobby").gameObject},
                {MenuType.Settings,transform.Find("Settings").gameObject},
            };
            Current = MenuType.MainMenu;
        }
        else {
            throw new Exception("Menu Scene object is already exist");
        }

        DOVirtual.DelayedCall(2,()=>Current  = MenuType.Lobby);
        DOVirtual.DelayedCall(3,()=>Current  = MenuType.Settings);
        DOVirtual.DelayedCall(4,()=>Current  = MenuType.MainMenu);
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
        foreach (KeyValuePair<MenuType, GameObject> pair in menusTypeMap) {
            bool isCurrent = pair.Key == Current;
            pair.Value.SetActive(isCurrent);
        }
    }

    public static void Goto(MenuType menu) {
        instance.Current = menu;
    }

}
