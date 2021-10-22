using UnityEngine;
using UnityEngine.UI;

namespace FastSkillTeam
{
    public class FST_PlayerNamer : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Text UserNameDisplayText;
        [SerializeField] private Text FormationUserNameDisplayText;
#pragma warning restore CS0649
        void Start()
        {
            Done();
        }

        public void Done()
        {
            UserNameDisplayText.text = Photon.Pun.PhotonNetwork.LocalPlayer.NickName = FST_SettingsManager.PlayerName;
            FormationUserNameDisplayText.text = FST_SettingsManager.PlayerName + " Choose Your Formation";
        }
    }
}