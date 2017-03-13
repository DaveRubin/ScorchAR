using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        transform.Find("ButtonLobby").GetComponent<Button>().onClick.AddListener(LobbyClicked);
        transform.Find("ButtonSettings").GetComponent<Button>().onClick.AddListener(SettingsClicked);

	}

	public void LobbyClicked() {
		MenusScene.Goto(MenuType.Lobby);
	}

	public void SettingsClicked() {
        MenusScene.Goto(MenuType.Settings);
	}
	
}
