using UI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenBase {
    Button buttonSave;
    InputField inputName;

	void Awake () {
        buttonSave = transform.Find("UserPanel/ButtonSave").GetComponent<Button>();
        inputName = transform.Find("UserPanel/InputName").GetComponent<InputField>();
        transform.Find("ButtonLobby").GetComponent<Button>().onClick.AddListener(LobbyClicked);
        transform.Find("ButtonSettings").GetComponent<Button>().onClick.AddListener(SettingsClicked);
        inputName.text = MainUser.Instance.Name;
        buttonSave.onClick.AddListener(OnSave);
	}

	public void LobbyClicked() {
		MenusScene.GoTo(EScreenType.Lobby);
	}

	public void SettingsClicked() {
        MenusScene.GoTo(EScreenType.Settings);
	}

	public void OnSave() {
        MainUser.Instance.Name = inputName.text;
	}
	
}
