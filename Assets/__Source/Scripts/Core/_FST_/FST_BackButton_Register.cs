using UnityEngine;
using UnityEngine.UI;

public class FST_BackButton_Register : MonoBehaviour
{
    private Button m_Button;
    private void OnEnable()
    {
        if (!m_Button)
            m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(() => GoBack());
    }
    private void OnDisable()
    {
        if (m_Button)
            m_Button.onClick.RemoveListener(() => GoBack());
    }

    private void GoBack()
    {
        UIController.Instance.uiHandler.ShowRegistrationPanel(false);
        if (JiwemanRegisterdataManager.SharedInstance)
            JiwemanRegisterdataManager.SharedInstance.RegisterSuccess();
    }
}
