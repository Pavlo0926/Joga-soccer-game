using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
	private Vector3 originalPos;

    private void Awake()
	{
		originalPos = transform.localPosition;

		if (GetComponent<Button> () != null) 
            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MainMenuEvents(gameObject.name));
		else if (GetComponent<Text> () != null) 
			GameManager.Instance.AssignTextInstanceToObject (gameObject.name, gameObject);
		else if (GetComponent<TextMesh> () != null) 
			GameManager.Instance.AssignTextInstanceToObject (gameObject.name, gameObject);
	}

    private void OnDestroy()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().onClick.RemoveListener(() => GameManager.Instance.MainMenuEvents(gameObject.name));
    }

    public void ResetToOriginalPos()
	{
		transform.localPosition = originalPos;
	}
}
