//
// Main user
// handles all user changes and modifications
// saves the data on the device and retrieves it on load
using ScorchEngine.Models;
using UnityEngine;

public class MainUser {
    //key used to save user name locally
    private const string KEY_NAME = "MAINUSERKEY";
    private const string KEY_INDEX = "MAINUSERKEY_IDX";
    public static MainUser Instance;
    public GameInfo CurrentGame;

    //once accessed, create the static instance
    static MainUser() {
        Instance = new MainUser();
        if (PlayerPrefs.HasKey(KEY_NAME)) {
            Instance._name = PlayerPrefs.GetString(KEY_NAME);
            Instance._index = PlayerPrefs.GetInt(KEY_INDEX);
        }
        else {
            Instance.Name = "UserXXX";
            Instance.Index = 0;
        }
    }

    private string _name;
    public string Name {
        get { return _name;}
        set {
            _name = value;
            Save();
        }
    }

    private int _index;
    public int Index { get{return _index;} set{
        _index = value;
        Save();
    } }

    /// <summary>
    /// Saves info to local memory
    /// </summary>
    public void Save() {
        PlayerPrefs.SetString(KEY_NAME,_name);
        PlayerPrefs.SetInt(KEY_INDEX,_index);
    }


}
