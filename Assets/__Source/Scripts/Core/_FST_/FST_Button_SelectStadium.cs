using UnityEngine;
using UnityEngine.UI;
namespace FastSkillTeam
{
    public class FST_Button_SelectStadium : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private bool m_IsNext = false;
        [SerializeField] private Text m_IndicatorText;
#pragma warning restore CS0649
        private Button m_Button = null;
        private void OnEnable()
        {
            if (!m_Button)
                m_Button = GetComponent<Button>();

            if (m_Button)
                m_Button.onClick.AddListener(() => SetStadium());

            UpdateDisplayText(GameManager.SelectedStadium);
        }
        private void OnDisable()
        {
            if (m_Button)
                m_Button.onClick.RemoveListener(() => SetStadium());
        }

        private void SetStadium()
        {
            if (m_IsNext == true)
            {
                if (GameManager.SelectedStadium < 5)
                    UpdateDisplayText(++GameManager.SelectedStadium);
            }
            else if (GameManager.SelectedStadium > 0)
                UpdateDisplayText(--GameManager.SelectedStadium);
        }

        public void UpdateDisplayText(int choice)
        {
            if (m_IndicatorText)
                m_IndicatorText.text = choice == 0 ? "Nairobi" : choice == 1 ? "Ice" : choice == 2 ? "Spider" : choice == 3 ? "Beauty" : choice == 4 ? "Planet" : "Bat";
        }
    }
}
