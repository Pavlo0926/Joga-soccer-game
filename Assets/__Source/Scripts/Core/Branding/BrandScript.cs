using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrandScript : MonoBehaviour
{

	private int mBranding_exclusive = 0;
	//private int mBranding_id = 0;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//Assigning the brand_no on onclick
	public void currentBrand ()
	{
		Branding ((int.Parse (this.gameObject.name.ToString ())));
	}




	//Storing brands
	public void Branding (int brand_no)
	{
		mBranding_exclusive = brand_no;
		Debug.Log ("mbanding no-------" + mBranding_exclusive);
		GameManager.Instance.Current_brand = mBranding_exclusive;
		PlayerPrefs.SetInt ("Current_brand", mBranding_exclusive);



		if (mBranding_exclusive != 0) {
			
			if (GameManager.Instance.Brand_type [mBranding_exclusive - 1] == "Exclusive") {
				BrandManager.SharedInstance.mStart_downloading = false;
				PlayerPrefs.SetInt ("Current_brand_id", GameManager.Instance.Brand_id [mBranding_exclusive - 1]);
				PlayerPrefs.SetString ("Current_brand_type", GameManager.Instance.Brand_type [mBranding_exclusive - 1]);
				BrandManager.SharedInstance.GetBrands_BG ("" + GameManager.Instance.Brand_id [mBranding_exclusive - 1]);
				if (GameManager.Instance.BG.Count > 1) {
					GameManager.Instance.BG.RemoveAt (1);
					GameManager.Instance.Topbar.RemoveAt (1);
					GameManager.Instance.Downbar.RemoveAt (1);
					GameManager.Instance.mtopCornerbar.RemoveAt (1);
					GameManager.Instance.mdownCornerbar.RemoveAt (1);
				}
				UIController.Instance.Loading_2_panel.SetActive (true);
			} else {
				PlayerPrefs.SetInt ("Current_brand_id", GameManager.Instance.Brand_id [mBranding_exclusive - 1]);
				PlayerPrefs.SetString ("Current_brand_type", GameManager.Instance.Brand_type [mBranding_exclusive - 1]);
				BrandManager.SharedInstance.GetBrands_ads ("" + GameManager.Instance.Brand_id [mBranding_exclusive - 1]);
				GameManager.Instance.Brand_ads.Clear ();
				GameManager.Instance.Change_Ads ();
				UIController.Instance.Loading_2_panel.SetActive (true);
			}
		} else {
			PlayerPrefs.SetInt ("Current_brand_id", 0);
			PlayerPrefs.SetString ("Current_brand_type", "None");
			GameManager.Instance.Change_BG ();
		}
		
	}


}
