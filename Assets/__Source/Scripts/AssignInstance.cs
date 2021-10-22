using UnityEngine;

public class AssignInstance : MonoBehaviour {

    public bool DisableAfter = false;

    void Awake()
    {
        if (GameManager.Instance)
            GameManager.Instance.AssignInstanceToObject(gameObject.name, gameObject);

        if (DisableAfter)
            gameObject.SetActive(false);
    }

	public void ForTutorialSkip()
	{
        Debug.Log("CHECK");
		UIController.Instance.Tutorialscreen.SetActive(false);
		PlayerPrefs.SetInt("FirstTime",1);
	}
}
