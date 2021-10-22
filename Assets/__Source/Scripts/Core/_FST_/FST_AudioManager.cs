/////////////////////////////////////////////////////////////////////////////////
//
//  FST_AudioManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class is the hub for all audio, if online, it will sync 
//                  audio between players.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine.Audio;

namespace FastSkillTeam
{
    public class FST_AudioManager : MonoBehaviour
    {
        public static FST_AudioManager Instance;

#pragma warning disable CS0649
        [SerializeField] private AudioSource m_SFXAudioSource;
        [SerializeField] private AudioSource m_MusicAudioSource;
        [SerializeField] private AudioSource m_AmbientAudioSource;

        [SerializeField] private AudioClip m_SFX_BallToWall_Soft, m_SFX_BallToWall_Medium, m_SFX_BallToWall_Hard, m_SFX_BallToPost_Soft, m_SFX_BallToPost_Medium, m_SFX_BallToPost_Hard, m_SFX_DiscToDisc_Soft, m_SFX_DiscToDisc_Medium, m_SFX_DiscToDisc_Hard, m_SFX_DiscToBall_Soft, m_SFX_DiscToBall_Medium, m_SFX_DiscToBall_Hard, m_SFX_DiscToWall_Soft, m_SFX_DiscToWall_Medium, m_SFX_DiscToWall_Hard, m_MUSIC_Menu, m_MUSIC_InGame;
        [SerializeField] private AudioClip[] m_AMBIENCE_Crowd;
#pragma warning restore CS0649
        public enum AudioID : byte { SFX_BallToWall_Soft, SFX_BallToWall_Medium, SFX_BallToWall_Hard, SFX_BallToPost_Soft, SFX_BallToPost_Medium, SFX_BallToPost_Hard, SFX_DiscToDisc_Soft, SFX_DiscToDisc_Medium, SFX_DiscToDisc_Hard, SFX_DiscToBall_Soft, SFX_DiscToBall_Medium, SFX_DiscToBall_Hard, SFX_DiscToWall_Soft, SFX_DiscToWall_Medium, SFX_DiscToWall_Hard, MUSIC_Menu, MUSIC_InGame, AMBIENCE_Crowd }


        private AudioMixer m_Mixer = null;
        public AudioMixer Mixer { get { if (!m_Mixer) m_Mixer = Resources.Load("Mixer") as AudioMixer; return m_Mixer; } }


        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        bool netInit = false;
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            m_AmbientAudioSource.volume = 0;

            if (FST_Gameplay.IsMultiplayer && !netInit)
            {
             //   Debug.Log("FST_AudioManager > Register networked audio events");
                PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
                netInit = true;
            }
            else if (!FST_Gameplay.IsMultiplayer && netInit)
            {
             //   Debug.Log("FST_AudioManager > Unregister networked audio events");
                PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
                netInit = false;
            }

            if (scene.name == "MainMenu")
            {
                if (m_MusicAudioSource.isPlaying)
                {
                    if (m_MusicAudioSource.clip == m_MUSIC_InGame)
                        m_MusicAudioSource.Stop();
                }

                if (!m_MusicAudioSource.isPlaying)
                {
                    PlayAudioInternal(AudioID.MUSIC_Menu);
                }
            }
            else if(scene.name == "InGame")
            {
                m_MusicAudioSource.Stop();
                // PlayAudioInternal(AudioID.MUSIC_InGame);
            }
            else m_MusicAudioSource.Stop();
        }

        private void NetworkingClient_EventReceived(EventData eventData)
        {
            byte evCode = eventData.Code;

            if (evCode == FST_ByteCodes.AMBIENT_AUDIO_CODE)
                PlayAudioInternal(AudioID.AMBIENCE_Crowd, (int)eventData.CustomData);

            if (evCode == FST_ByteCodes.AUDIO_CODE)
            {
                byte b = (byte)eventData.CustomData;
                PlayAudioInternal((AudioID)b);
            }
        }

        //private float nextFire = 0;

        //private AudioID lastAudioId = AudioID.MUSIC_InGame;

        public void PlayAudio(AudioID audioID)
        {
            //if (lastAudioId == audioID)
            //    if (Time.time < nextFire)
            //        return;

            //nextFire = Time.time + 0.05f;

            //lastAudioId = audioID;

            if (audioID == AudioID.AMBIENCE_Crowd)
            {
                int rnd = Random.Range(0, m_AMBIENCE_Crowd.Length);

                if (FST_Gameplay.IsMultiplayer && FST_DiskPlayerManager.Instance && FST_DiskPlayerManager.Instance.IsOwner)
                    PhotonNetwork.RaiseEvent(FST_ByteCodes.AMBIENT_AUDIO_CODE, rnd, RaiseEventOptions.Default, SendOptions.SendUnreliable);

                PlayAudioInternal(audioID, rnd);

                return;
            }

            if (FST_Gameplay.IsMultiplayer && FST_DiskPlayerManager.Instance && FST_DiskPlayerManager.Instance.IsOwner)
                PhotonNetwork.RaiseEvent(FST_ByteCodes.AUDIO_CODE, (byte)audioID, RaiseEventOptions.Default, SendOptions.SendUnreliable);

            PlayAudioInternal(audioID);
        }

        private void PlayAudioInternal(AudioID audioID, int crowdChantId = 0)
        {
            AudioClip clip = null;
            int index = 0;
            switch (audioID)
            {
                case AudioID.AMBIENCE_Crowd:
                    clip = m_AMBIENCE_Crowd[crowdChantId];
                    index = 2;
                    break;
                case AudioID.MUSIC_InGame:
                    clip = m_MUSIC_InGame;
                    index = 1;
                    break;
                case AudioID.MUSIC_Menu:
                    clip = m_MUSIC_Menu;
                    index = 1;
                    break;
                case AudioID.SFX_BallToWall_Soft:
                    clip = m_SFX_BallToWall_Soft;
                    break;
                case AudioID.SFX_BallToWall_Medium:
                    clip = m_SFX_BallToWall_Medium;
                    break;
                case AudioID.SFX_BallToWall_Hard:
                    clip = m_SFX_BallToWall_Hard;
                    break;
                case AudioID.SFX_BallToPost_Soft:
                    clip = m_SFX_BallToPost_Soft;
                    break;
                case AudioID.SFX_BallToPost_Medium:
                    clip = m_SFX_BallToPost_Medium;
                    break;
                case AudioID.SFX_BallToPost_Hard:
                    clip = m_SFX_BallToPost_Hard;
                    break;
                case AudioID.SFX_DiscToBall_Hard:
                    clip = m_SFX_DiscToBall_Hard;
                    break;
                case AudioID.SFX_DiscToBall_Medium:
                    clip = m_SFX_DiscToBall_Medium;
                    break;
                case AudioID.SFX_DiscToBall_Soft:
                    clip = m_SFX_DiscToBall_Soft;
                    break;
                case AudioID.SFX_DiscToDisc_Hard:
                    clip = m_SFX_DiscToDisc_Hard;
                    break;
                case AudioID.SFX_DiscToDisc_Medium:
                    clip = m_SFX_DiscToDisc_Medium;
                    break;
                case AudioID.SFX_DiscToDisc_Soft:
                    clip = m_SFX_DiscToDisc_Soft;
                    break;
                case AudioID.SFX_DiscToWall_Hard:
                    clip = m_SFX_DiscToWall_Hard;
                    break;
                case AudioID.SFX_DiscToWall_Medium:
                    clip = m_SFX_DiscToWall_Medium;
                    break;
                case AudioID.SFX_DiscToWall_Soft:
                    clip = m_SFX_DiscToWall_Soft;
                    break;
            }

            if (!clip)
            {
                Debug.LogWarning("FST_AudioManager > Clip to play was null!");
                return;
            }

            if (index == 0)
                m_SFXAudioSource.PlayOneShot(clip);
            else if (index == 1)
            {
                m_MusicAudioSource.Stop();
                m_MusicAudioSource.clip = clip;
                m_MusicAudioSource.Play();
            }
            else
            {
                if (!m_AmbientAudioSource.isPlaying)
                {
                    StartCoroutine(StartFade(m_AmbientAudioSource, 2.5f, FST_SettingsManager.FxVolume));
                    StartCoroutine(FadeOutAtEndOfClip(m_AmbientAudioSource, clip, 2.5f));
                    m_AmbientAudioSource.clip = clip;
                    m_AmbientAudioSource.Play();
                }
            }
        }

        private IEnumerator FadeOutAtEndOfClip(AudioSource source, AudioClip clip, float duration)
        {
            if (clip == null)
            {
                Debug.Log("NO CLIP");
                yield break;
            }

            float t = Time.time + clip.length - (duration + 0.1f);

            while (Time.time < t)
                yield return null;

            StartCoroutine(StartFade(source, duration, 0));

            yield break;
        }

        /// <summary>
        /// Fades the mixer
        /// </summary>
        /// <param name="exposedParam">"SFXVolume", "MusicVolume", "MasterVolume"</param>
        /// <param name="duration"></param>
        /// <param name="targetVolume"></param>
        /// <returns></returns>
        private IEnumerator StartFade(string exposedParam, float duration, float targetVolume)
        {
            if (!Mixer.GetFloat(exposedParam, out float currentVol))
                yield break;

            float currentTime = 0;
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                Mixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }

        private IEnumerator StartFade(AudioSource source, float duration, float targetVolume)
        {
            float t = 0;
            float start = source.volume;

            while (t < duration)
            {
                t += Time.deltaTime;
                source.volume = Mathf.Lerp(start, targetVolume, t / duration);
                yield return null;
            }

            if(targetVolume == 0)
                source.Stop();

            yield break;
        }
    }
}
