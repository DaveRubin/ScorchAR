using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    InputField inputName;
    Button buttonSave;

	// Use this for initialization
	void Awake () {
        transform.Find("ButtonLobby").GetComponent<Button>().onClick.AddListener(LobbyClicked);
        transform.Find("ButtonSettings").GetComponent<Button>().onClick.AddListener(SettingsClicked);
        inputName = transform.Find("UserPanel/InputName").GetComponent<InputField>();
        buttonSave = transform.Find("UserPanel/ButtonSave").GetComponent<Button>();
        inputName.text = MainUser.Instance.Name;
        buttonSave.onClick.AddListener(SaveUserName);
	}

	public void LobbyClicked() {
		MenusScene.GoTo(EScreenType.Lobby);
	}

	public void SettingsClicked() {
        MenusScene.GoTo(EScreenType.Settings);
	}

	public void SaveUserName() {
        Debug.LogFormat("Saving name - {0}",inputName.text);
        MainUser.Instance.Name = inputName.text;
	}
	
}
