//
// Main user
// handles all user changes and modifications
// saves the data on the device and retrieves it on load
using UnityEngine;

public class MainUser {
    //key used to save user name locally
    private const string KEY = "MAINUSERKEY";
    public static MainUser Instance;

    //once accessed, create the static instance
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
    /// Saves info to local memory
    /// </summary>
    public void Save() { PlayerPrefs.SetString(KEY,_name);}

}
