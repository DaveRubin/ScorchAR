using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScreenSettings : ScreenBase {
        void Awake() {
            base.Awake();
            Toggle toggle = transform.Find("Toggle").GetComponent<Toggle>();
            Slider slider = transform.Find("Slider").GetComponent<Slider>();
            transform.Find("ButtonBack").GetComponent<Button>().onClick.AddListener(GoBack);
            int savedMuteVal = PlayerPrefs.GetInt("Mute");
            bool muteValBool = Convert.ToBoolean(savedMuteVal);
            AudioListener.pause = muteValBool;
            float volume = PlayerPrefs.GetFloat("Volume");
            slider.value = volume;
            AudioListener.volume = slider.value;
            toggle.isOn = !muteValBool;
            toggle.onValueChanged.AddListener(OnValueChange);
            slider.onValueChanged.AddListener(OnVolumeChange);
        }

        public void OnValueChange(bool val) {
            AudioListener.pause = !val;
            int muteVal = Convert.ToInt32(!val);
            PlayerPrefs.SetInt("Mute", muteVal);
            PlayerPrefs.Save();
        }

        public void OnVolumeChange(float val)
        {
            AudioListener.volume = val;
            Debug.Log("Volume changed to: " + val);
            PlayerPrefs.SetFloat("Volume", val);
            PlayerPrefs.Save();
        }
    }
}