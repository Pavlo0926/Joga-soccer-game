using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace FastSkillTeam {
    [RequireComponent(typeof(Slider))]
    public class FST_UI_Slider : MonoBehaviour
    {
        public enum Slider_Type { Volume_Master, Volume_Music, Volume_Fx, Brightness }
        public Slider_Type SliderType = Slider_Type.Volume_Master;

        private AudioMixer Mixer { get { return FST_AudioManager.Instance.Mixer; } }
    
        private Slider m_Slider = null;
        private Slider Slider { get { if (m_Slider == null) m_Slider = GetComponent<Slider>(); return m_Slider; } }

        private void Awake()
        {
            switch (SliderType)
            {
                default:
                    Slider.value = FST_SettingsManager.MasterVolume;
                    break;

                case Slider_Type.Volume_Fx:
                    Slider.value = FST_SettingsManager.FxVolume;
                    break;

                case Slider_Type.Volume_Music:
                    Slider.value = FST_SettingsManager.MusicVolume;
                    break;

                case Slider_Type.Brightness:
                    Slider.value = FST_SettingsManager.Brightness;
                    break;
            }

            Slider.onValueChanged.AddListener((float _) => OnValueChanged(_)); //UI classes use unity events, requiring delegates (delegate(float _) { SetVolume(_); }) or lambda expressions ((float _) => SetVolume(_))

         Invoke("Init", 0);//because setting audio volume in mixer do not work in awake
        }

        /// <summary>
        /// sets the values according to saved values on startup
        /// </summary>
        public void Init()
        {
            switch (SliderType)
            {
                default:
                    OnValueChanged(FST_SettingsManager.MasterVolume);
                    break;

                case Slider_Type.Volume_Fx:
                    OnValueChanged(FST_SettingsManager.FxVolume);
                    break;

                case Slider_Type.Volume_Music:
                    OnValueChanged(FST_SettingsManager.MusicVolume);
                    break;

                case Slider_Type.Brightness:
                    OnValueChanged(FST_SettingsManager.Brightness);
                    break;
            }
        }

        private void OnValueChanged(float _value)
        {
            switch (SliderType)
            {
                default:
                    Mixer.SetFloat("MasterVolume", ConvertToDecibel(_value / Slider.maxValue)); //Dividing by max allows arbitrary positive slider maxValue
                    FST_SettingsManager.MasterVolume = _value;
                    break;

                case Slider_Type.Volume_Fx:
                    Mixer.SetFloat("SFXVolume", ConvertToDecibel(_value / Slider.maxValue)); //Dividing by max allows arbitrary positive slider maxValue
                    FST_SettingsManager.FxVolume = _value;
                    break;

                case Slider_Type.Volume_Music:
                    Mixer.SetFloat("MusicVolume", ConvertToDecibel(_value / Slider.maxValue)); //Dividing by max allows arbitrary positive slider maxValue
                    FST_SettingsManager.MusicVolume = _value;
                    break;

                case Slider_Type.Brightness:
                    FST_SettingsManager.Brightness = _value;
                    break;
            }
        }

        /// <summary>
        /// Converts a percentage fraction to decibels,
        /// with a lower clamp of 0.0001 for a minimum of -80dB, same as Unity's Mixers.
        /// </summary>
        private float ConvertToDecibel(float _value)
        {
            return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
        }
    }
}