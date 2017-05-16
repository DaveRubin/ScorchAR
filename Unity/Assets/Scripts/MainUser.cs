//
// Main user
// handles all user changes and modifications
// saves the data on the device and retrieves it on load
using UnityEngine;

public class MainUser {
    private const string KEY = "MAINUSERKEY";
    public static MainUser Instance;

    static MainUser() {
        Instance = new MainUser();
        if (PlayerPrefs.HasKey(KEY)) {
            Instance._name = PlayerPrefs.GetString(KEY);
        }
        else {
            Instance.Name = "UserXXX";
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

    /// <summary>
    /// Should save the whole object...
    /// </summary>
    public void Save() {
        PlayerPrefs.SetString(KEY,_name);
    }

}
