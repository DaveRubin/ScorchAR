using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScreenLobby : ScreenBase {

        RectTransform content;
        GameObject lobbyItemPrefab;

        void Awake() {
            lobbyItemPrefab = Resources.Load<GameObject>("Prefabs/LobbyGameItem");
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            content = transform.Find("MainPanel/Left/Scroll View/Viewport/Content").GetComponent<RectTransform>();
            float current = content.rect.height/2;
            float height = -1;

            for (int i = 0; i < 10; i++) {
                GameObject tmp = Instantiate(lobbyItemPrefab);
                tmp.name = "Item " + i;
                tmp.transform.SetParent(content);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(0, current);
                current -= tmp.GetComponent<RectTransform>().rect.height;

                if (height == -1) {
                    height = tmp.GetComponent<RectTransform>().rect.height*10;
                }
            }

            content.sizeDelta = new Vector2(content.rect.width,height);
        }
    }
}