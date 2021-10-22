using UnityEngine;
public class AssignBrands : MonoBehaviour
{
	void Start ()
	{
		if (gameObject.name == "bottombar") 
		{
			GameManager.Instance.DownbarObject = gameObject;
		}

		if (gameObject.name == "topbar") {
			GameManager.Instance.TopbarObject = gameObject;
		}

		if (gameObject.name == "Bg" && gameObject.activeInHierarchy) 
		{
            if (!GameManager.Instance.BgObjects.Contains(gameObject))
                GameManager.Instance.BgObjects.Add(gameObject);
            if (!GameManager.Instance.TopCornerbarObject.Contains(gameObject.transform.GetChild(0).gameObject))
                GameManager.Instance.TopCornerbarObject.Add (gameObject.transform.GetChild(0).gameObject);
            if (!GameManager.Instance.DownCornerbarObject.Contains(gameObject.transform.GetChild(1).gameObject))
                GameManager.Instance.DownCornerbarObject.Add (gameObject.transform.GetChild (1).gameObject);
		}
	}
}
