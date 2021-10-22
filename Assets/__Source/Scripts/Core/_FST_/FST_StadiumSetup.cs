using System.Collections.Generic;
using UnityEngine;
namespace FastSkillTeam {
    public class FST_StadiumSetup : MonoBehaviour
    {
        [System.Serializable]
        public class Stadium
        {
            public string Name = "new stadium";
            public int SelectionIndex = 0;
            public GameObject StadiumParent;
            public Material BallMaterial;
            public Renderer[] PlayerGoalMats;
            public Renderer[] OpponentGoalMats;
            public void SetActive(bool active)
            {
                if (active)
                {
                    FST_DiskPlayerManager.Instance.PlayerGoalMats = PlayerGoalMats;
                    FST_DiskPlayerManager.Instance.OpponentGoalMats = OpponentGoalMats;
                    for (int i = 0; i < PlayerGoalMats.Length; i++)
                        PlayerGoalMats[i].sortingOrder = 5;
                    for (int i = 0; i < OpponentGoalMats.Length; i++)
                        OpponentGoalMats[i].sortingOrder = 5;
                }
                StadiumParent.SetActive(active);
            }
        }
#pragma warning disable CS0649
        [SerializeField] private List<Stadium> m_Stadiums = new List<Stadium>();
        [SerializeField] private Renderer m_BallRenderer;
#pragma warning restore CS0649
        void Start()
        {
            if(GameManager.SelectedStadium > m_Stadiums.Count)
            {
                Debug.Log("fixing bad image and stadium ID...");
                GameManager.SelectedStadium = m_Stadiums.Count;
            }

          //  Debug.Log("setting stadium " + GameManager.SelectedStadium + " active!");
            for (int i = 0; i < m_Stadiums.Count; i++)
            {
                if (m_Stadiums[i].SelectionIndex == GameManager.SelectedStadium)
                {
                    m_Stadiums[i].SetActive(true);
                    m_BallRenderer.sharedMaterial = m_Stadiums[i].BallMaterial;
                }
                else m_Stadiums[i].SetActive(false);
            }
        }
    }
}
