using UnityEngine;
using UnityEngine.UI;
namespace FastSkillTeam
{
    [RequireComponent(typeof(Button))]
    public class FST_Button_Difficulty : MonoBehaviour
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
                m_Button.onClick.AddListener(() => SetDifficulty());

            UpdateDisplayText(FST_SettingsManager.Difficulty);
        }
        private void OnDisable()
        {
            if (m_Button)
                m_Button.onClick.RemoveListener(() => SetDifficulty());
        }

        private void SetDifficulty()
        {
            if (m_IsNext == true)
            {
                if (FST_SettingsManager.Difficulty < 2)
                    UpdateDisplayText(++FST_SettingsManager.Difficulty);
            }
            else if(FST_SettingsManager.Difficulty > 0)
                UpdateDisplayText(--FST_SettingsManager.Difficulty);
        }

        private void UpdateDisplayText(int dif)
        {
            if (m_IndicatorText)
                m_IndicatorText.text = dif == 0 ? "Easy" : dif == 1 ? "Normal" : "Hard";
        }
    }

}
