using DG.Tweening;
using ScorchEngine.Models;
using Server;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainMenu : ScreenBase {
    Button buttonSave;
    InputField inputName;

	void Awake () {
        base.Awake();
        buttonSave = transform.Find("UserPanel/ButtonSave").GetComponent<Button>();
        inputName = transform.Find("UserPanel/InputName").GetComponent<InputField>();
        transform.Find("ButtonLobby").GetComponent<Button>().onClick.AddListener(LobbyClicked);
        transform.Find("ButtonSettings").GetComponent<Button>().onClick.AddListener(SettingsClicked);
        inputName.text = MainUser.Instance.Name;
        buttonSave.onClick.AddListener(OnSave);
        //CHECK ID, if no id then get one from
        DOVirtual.DelayedCall(0.1f,()=> {

            if (string.IsNullOrEmpty(MainUser.Instance.Id)) {
                Debug.Log("no id  present, getting ");
                UnityServerWrapper.Instance.CreateUser(MainUser.Instance.Name,onPlayerUpdated);
            }
        });
	}

	public void LobbyClicked() {
		MenusScene.GoTo(EScreenType.Lobby);
	}

	public void SettingsClicked() {
        MenusScene.GoTo(EScreenType.Settings);
	}

	public void OnSave() {
        MainUser.Instance.Name = inputName.text;
        UnityServerWrapper.Instance.UpdateUser(MainUser.Instance.GetPLayerInfo(),onPlayerUpdated);
	}

    public void UpdateText() {
        inputName.text = MainUser.Instance.Name;
    }

    public void onPlayerUpdated(PlayerInfo playerInfo) {
        Debug.Log("Updating");
        UpdateText();
        MainUser.Instance.UpdateFromPlayerInfo(playerInfo);
    }
	
}
