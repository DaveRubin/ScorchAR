using UI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenBase {

	void Awake () {
        transform.Find("ButtonLobby").GetComponent<Button>().onClick.AddListener(LobbyClicked);
        transform.Find("ButtonSettings").GetComponent<Button>().onClick.AddListener(SettingsClicked);
	}

	public void LobbyClicked() {
		MenusScene.GoTo(EScreenType.Lobby);
	}

	public void SettingsClicked() {
        MenusScene.GoTo(EScreenType.Settings);
	}
	
}
