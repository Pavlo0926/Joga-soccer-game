using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using WebSocketSharp;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class BrandManager : MonoBehaviour
{


	public List<Sprite> imgSprite = new List<Sprite> ();
	public int brandlogo_count;
	public int brandbg_count;
	public int brandads_count;
	public int mCurrent_brand_id;
	public string mCurrent_brand_type;
	public bool mStart_downloading;
	public bool internetConnection;

	public string logoCache;



	private static BrandManager _instance = null;

	public static BrandManager SharedInstance {
		get {
			// if the instance hasn't been assigned then search for it
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(BrandManager)) as BrandManager; 	
			}
			return _instance; 
		}
	}

	void Start ()
	{		
		//StartCoroutine (Loadingtheme ());
	}





	void Awake ()
	{
		mCurrent_brand_id = PlayerPrefs.GetInt ("Current_brand_id");
		mCurrent_brand_type = PlayerPrefs.GetString ("Current_brand_type");

	}

	public void GetBrands_BG (string id)
	{
		/* if (isNetworkAvailable ()) {
			string name = id.ToString ();

			bg_Imgdetails bgdata = new bg_Imgdetails ();
			bgdata.brandId = name;

			string bgreg = JsonMapper.ToJson (bgdata);

			Dictionary<string,string> headers = new Dictionary<string, string> ();

			headers.Add ("Content-Type", "application/json");

			byte[] pData = System.Text.Encoding.ASCII.GetBytes (bgreg.ToCharArray ());
			///POST by IIS hosting...

			Debug.Log ("JasonMapper   " + pData);
			WWW api = new WWW (WebSocketConstant.GET_BRANDS_BY_BG, pData, headers);

			StartCoroutine (waitDownloading_brandimg (api));
		} else {
			StartCoroutine (checkingInternet_time ());
		} */

	}


	public void GetBrands_logo ()
	{
		/* if (isNetworkAvailable ()) {
			string name = "";

			logodetails bgdata = new logodetails ();
			bgdata.imgType = name;


			string bgreg = JsonMapper.ToJson (bgdata);

			Dictionary<string,string> headers = new Dictionary<string, string> ();

			headers.Add ("Content-Type", "application/json");

			byte[] pData = System.Text.Encoding.ASCII.GetBytes (bgreg.ToCharArray ());
			///POST by IIS hosting...

			//Debug.Log ("JasonMapper   " + pData);
			WWW api = new WWW (WebSocketConstant.GET_BRANDS_BY_LOGO, pData, headers);

			StartCoroutine (waitDownloading_brandlist (api));
		} else {
			StartCoroutine (checkingInternet_time ());
		} */

	}


	public void GetBrands_ads (string id)
	{
		/* if (isNetworkAvailable ()) {

			string name = id.ToString ();

			bg_Imgdetails bgdata = new bg_Imgdetails ();
			bgdata.brandId = name;


			string bgreg = JsonMapper.ToJson (bgdata);


			Dictionary<string,string> headers = new Dictionary<string, string> ();

			headers.Add ("Content-Type", "application/json");

			byte[] pData = System.Text.Encoding.ASCII.GetBytes (bgreg.ToCharArray ());
			///POST by IIS hosting...

			//Debug.Log ("JasonMapper   " + pData);
			WWW api = new WWW (WebSocketConstant.GET_BRANDS_BY_BG, pData, headers);


			StartCoroutine (waitDownloading_ads (api));
		} else {
			StartCoroutine (checkingInternet_time ());
		} */

	}


	string logoUrl, topbarUrl, downbarUrl;

	// private IEnumerator waitDownloading_brandlist (WWW www)
	// {
	// 	yield return www;


	// 	//Debug.Log ("updating logo  " + www.text);


	// 	JsonData result = JsonMapper.ToObject (www.text);	
			

	// 	if (www.error == null) {
	// 		if (result ["status"].Equals (true)) {

	// 			Debug.Log ("Result waitDownloading_brandlist " + www.text);

	// 			brandlogo_count = result ["Brands"].Count;
	// 			//Debug.Log ("brand_count---" + brandlogo_count);

	// 			for (int i = 0; i < result ["Brands"].Count; i++) {
					
	// 				logoUrl = result ["Brands"] [i] ["Logo"].ToString ();	

				
	// 				GameManager.SharedInstance.mbrand_id.Add (int.Parse (result ["Brands"] [i] ["Brand_id"].ToString ()));
	// 				GameManager.SharedInstance.mbrand_type.Add (result ["Brands"] [i] ["Brand_type"].ToString ());

	// 				string image_Url = "";

	// 				if (logoUrl != "") {
	// 					image_Url = "http://52.15.74.142:8080" + logoUrl;
	// 				}

			
	// 				if (image_Url != "") {
	// 					WWW wwww = new WWW (image_Url);
	// 					yield return wwww;

	// 					if (!mStart_downloading)
	// 						File.Delete (Application.persistentDataPath + "/logo" + i + ".png");

	// 					if (File.Exists (Application.persistentDataPath + "/logo" + i + ".png")) {

	// 						byte[] imageBytes = File.ReadAllBytes (Application.persistentDataPath + "/logo" + i + ".png");

	// 						int imageWidth_logo = PlayerPrefs.GetInt ("imageWidth_logo");
	// 						int imageHeight_logo = PlayerPrefs.GetInt ("imageHeight_logo");


	// 						if (wwww.texture != null) {

	// 							//Debug.LogError ("loading logo locally ");

	// 							Texture2D localTexture = new Texture2D (imageWidth_logo, imageHeight_logo, TextureFormat.ARGB32, false);
	// 							localTexture.LoadImage (imageBytes);

	// 							Rect rec = new Rect (0, 0, imageWidth_logo, imageHeight_logo);
	// 							Sprite spriteToUse = Sprite.Create (localTexture, rec, new Vector2 (1f, 1f), 100);

	// 							spriteToUse.name = "brand_logo";
	

	// 							GameManager.SharedInstance.brand_iconImg.Add (spriteToUse);

	// 							wwww.Dispose ();
	// 							wwww = null;						
	// 						} 

	// 					} else {

	// 						if (wwww.texture != null) {

	// 							//Debug.LogError ("Downloading logo from server ");

	// 							Texture2D texture = new Texture2D (wwww.texture.width, wwww.texture.height, TextureFormat.ARGB32, false);

	// 							PlayerPrefs.SetInt ("imageHeight_logo", wwww.texture.height);
	// 							PlayerPrefs.SetInt ("imageWidth_logo", wwww.texture.width);
							
	// 							wwww.LoadImageIntoTexture (texture);

	// 							Rect rec = new Rect (0, 0, texture.width, texture.height);
	// 							Sprite spriteToUse = Sprite.Create (texture, rec, new Vector2 (1f, 1f), 100);

	// 							spriteToUse.name = "brand_logo";


	// 							Texture2D copyTexture = duplicateForPanTexture (spriteToUse.texture);
	// 							File.WriteAllBytes (Application.persistentDataPath + "/logo" + i + ".png", copyTexture.EncodeToPNG ());

	// 							GameManager.SharedInstance.brand_iconImg.Add (spriteToUse);
						
	// 							wwww.Dispose ();
	// 							wwww = null;
	// 						}

	// 					}
							
	// 				} else {
	// 				}
	// 			}
	// 			UIController.SharedInstance.BGprefab_brand ();
               
	// 		} 
	// 	}
	// 	if (mCurrent_brand_id != 0) {
	// 		if (mCurrent_brand_type == "Exclusive" || mCurrent_brand_type == "None") {
	// 			GetBrands_BG ("" + mCurrent_brand_id);
	// 		} else {
	// 			GetBrands_ads ("" + mCurrent_brand_id);
	// 		}
	// 	} else {
	// 		UIController.SharedInstance.onResonseGot ();
	// 	}
	// }


	// private IEnumerator waitDownloading_ads (WWW www)
	// {
	// 	yield return www;

	// 	Debug.Log ("Updating ads  " + www.text);
	// 	JsonData result = JsonMapper.ToObject (www.text);

	// 	if (www.error == null) {
	// 		if (result ["status"].Equals (true)) {
	// 			Debug.Log ("Successfull www " + www.text);
	// 			if (result ["Ads"].Equals (null)) {
	// 				yield return null;
	// 			}
	// 			brandads_count = result ["Ads"].Count;

	// 			for (int i = 0; i < result ["Ads"].Count; i++) {
	// 				logoUrl = result ["Ads"] [i] ["imgUrl"].ToString ();	

	// 				//Debug.Log ("brand_count_ads--- " + brandads_count);
	// 				string image_Url = "";

	// 				if (logoUrl != "") {
	// 					image_Url = "http://52.15.74.142:8080" + logoUrl;
	// 				}


	// 				if (image_Url != "") {
	// 					WWW wwww = new WWW (image_Url);
	// 					yield return wwww;

	// 					//if (!mStart_downloading)
	// 					File.Delete (Application.persistentDataPath + "/ads" + i + ".png");

	// 					if (File.Exists (Application.persistentDataPath + "/ads" + i + ".png")) {

	// 						byte[] imageBytes = File.ReadAllBytes (Application.persistentDataPath + "/ads" + i + ".png");

	// 						int imageWidth_ads = PlayerPrefs.GetInt ("imageWidth_ads");
	// 						int imageHeight_ads = PlayerPrefs.GetInt ("imageHeight_ads");


	// 						if (wwww.texture != null) {

	// 							//Debug.LogError ("loading logo locally ");

	// 							Texture2D localTexture = new Texture2D (imageWidth_ads, imageHeight_ads, TextureFormat.ARGB32, false);
	// 							localTexture.LoadImage (imageBytes);

	// 							Rect rec = new Rect (0, 0, imageWidth_ads, imageHeight_ads);
	// 							Sprite spriteToUse = Sprite.Create (localTexture, rec, new Vector2 (1f, 1f), 100);

	// 							spriteToUse.name = "brand_ads";


	// 							GameManager.SharedInstance.brand_ads.Add (spriteToUse);

	// 							//Debug.LogError ("loading logo locally ");

	// 							wwww.Dispose ();
	// 							wwww = null;						
	// 						} 

	// 					} else {

	// 						if (wwww.texture != null) {

	// 							Debug.LogError ("Downloading logo from server ");

	// 							Texture2D texture = new Texture2D (wwww.texture.width, wwww.texture.height, TextureFormat.ARGB32, false);

	// 							PlayerPrefs.SetInt ("imageHeight_ads", wwww.texture.height);
	// 							PlayerPrefs.SetInt ("imageWidth_ads", wwww.texture.width);
							
	// 							wwww.LoadImageIntoTexture (texture);

	// 							Rect rec = new Rect (0, 0, texture.width, texture.height);
	// 							Sprite spriteToUse = Sprite.Create (texture, rec, new Vector2 (1f, 1f), 100);

	// 							spriteToUse.name = "brand_ads";


	// 							Texture2D copyTexture = duplicateForPanTexture (spriteToUse.texture);
	// 							File.WriteAllBytes (Application.persistentDataPath + "/ads" + i + ".png", copyTexture.EncodeToPNG ());

	// 							GameManager.SharedInstance.brand_ads.Add (spriteToUse);
							
	// 							//Debug.LogError ("loading logo locally ");

	// 							wwww.Dispose ();
	// 							wwww = null;
	// 						}

	// 					}

	// 				} else {
	// 				}

	// 			}
	// 			UIController.SharedInstance.Ads_assigning ();
	// 			// UIController.SharedInstance.DeactiveLoadingPanel (0.1f);
	// 			Debug.Log("DeactiveLoading2Panel-1");
	// 			UIController.SharedInstance.DeactiveLoading2Panel(0.1f);
	// 		} 
	// 	}
	// 	UIController.SharedInstance.onResonseGot ();
	// }

	// private IEnumerator waitDownloading_brandimg (WWW www)
	// {
	// 	yield return www;

	// 	//Debug.Log ("Updating bg  " + www.text);
	// 	JsonData result = JsonMapper.ToObject (www.text);

	// 	if (www.error == null) {
	// 		if (result ["status"].Equals (true)) {
	// 			//Debug.Log ("Successfull www " + www.text);

	// 			for (int j = 1; j <= 5; j++) {
					
	// 				string stadiumName = "Stadium" + j; 
				
	// 				for (int i = 0; i < result [stadiumName].Count; i++) {
						
	// 					logoUrl = result [stadiumName] [i] ["Brand_img"].ToString ();		

	// 					string image_Url = "";

	// 					if (logoUrl != "") {
	// 						image_Url = "http://52.15.74.142:8080" + logoUrl;
	// 					}


	// 					if (image_Url != "") {
	// 						WWW wwww = new WWW (image_Url);
	// 						yield return wwww;

	// 						if (!mStart_downloading) {
	// 							File.Delete (Application.persistentDataPath + "/brandimages" + i + j + ".png");
	// 						} else if (stadiumName == "Stadium4" || stadiumName == "Stadium5") {
	// 							File.Delete (Application.persistentDataPath + "/brandimages" + i + j + ".png");
	// 						}

	// 						if (File.Exists (Application.persistentDataPath + "/brandimages" + i + j + ".png")) {


	// 							byte[] imageBytes = File.ReadAllBytes (Application.persistentDataPath + "/brandimages" + i + j + ".png");

	// 							int imageWidth = PlayerPrefs.GetInt ("imageWidth" + j);
	// 							int imageHeight = PlayerPrefs.GetInt ("imageHeight" + j);


	// 							//Debug.LogError ("loading brandimage locally " + Application.persistentDataPath + "/brandimages" + i + j + ".png");

	// 							Texture2D localTexture = new Texture2D (imageWidth, imageHeight, TextureFormat.ARGB32, false);
	// 							localTexture.LoadImage (imageBytes);

	// 							Rect rec = new Rect (0, 0, imageWidth, imageHeight);
	// 							Sprite spriteToUse = Sprite.Create (localTexture, rec, new Vector2 (1f, 1f), 100);

	// 							spriteToUse.name = "mainBG" + j;

	// 							switch (j) {
	// 							case 1:					
	// 								GameManager.SharedInstance.mBG.Add (spriteToUse);

	// 								break;

	// 							case 2:
	// 								GameManager.SharedInstance.mtopbar.Add (spriteToUse);
	// 								break;

	// 							case 3:
	// 								GameManager.SharedInstance.mdownbar.Add (spriteToUse);							
	// 								break;

	// 							case 4:
	// 								GameManager.SharedInstance.mtopCornerbar.Add (spriteToUse);
	// 								break;

	// 							case 5:
	// 								GameManager.SharedInstance.mdownCornerbar.Add (spriteToUse);
	// 								//GameManager.SharedInstance.Change_BG ();
	// 								break;
	// 							}


	// 						} else {


	// 							if (wwww.texture != null) {

	// 								//Debug.LogError ("Downloading brandimage from server " + Application.persistentDataPath + "/brandimages" + i + j + ".png");


	// 								Texture2D texture = new Texture2D (wwww.texture.width, wwww.texture.height, TextureFormat.ARGB32, false);

	// 								PlayerPrefs.SetInt ("imageHeight" + j, wwww.texture.height);
	// 								PlayerPrefs.SetInt ("imageWidth" + j, wwww.texture.width);
							
	// 								wwww.LoadImageIntoTexture (texture);

	// 								Rect rec = new Rect (0, 0, texture.width, texture.height);
	// 								Sprite spriteToUse = Sprite.Create (texture, rec, new Vector2 (1f, 1f), 100);

	// 								spriteToUse.name = "mainBG" + j;


	// 								Texture2D copyTexture = duplicateForPanTexture (spriteToUse.texture);
	// 								File.WriteAllBytes (Application.persistentDataPath + "/brandimages" + i + j + ".png", copyTexture.EncodeToPNG ());

	// 								switch (j) {
	// 								case 1:					
	// 									GameManager.SharedInstance.mBG.Add (spriteToUse);
	// 									break;

	// 								case 2:
	// 									GameManager.SharedInstance.mtopbar.Add (spriteToUse);
	// 									break;

	// 								case 3:
	// 									GameManager.SharedInstance.mdownbar.Add (spriteToUse);							
	// 									break;

	// 								case 4:
	// 									GameManager.SharedInstance.mtopCornerbar.Add (spriteToUse);
	// 									break;

	// 								case 5:
	// 									GameManager.SharedInstance.mdownCornerbar.Add (spriteToUse);
	// 									//GameManager.SharedInstance.Change_BG ();
	// 									break;
	// 								}


	// 								wwww.Dispose ();
	// 								wwww = null;
	// 							}

	// 						}

	// 					} else {
	// 					}
	// 				}

	// 			}
	// 			GameManager.SharedInstance.Change_BG ();
	// 			// Debug.Log("DeactiveLoading2Panel-2");
	// 			UIController.SharedInstance.DeactiveLoading2Panel (0.1f);

	// 		} 
	// 	}
	// 	UIController.SharedInstance.onResonseGot ();
	// }

	// Texture2D duplicateForPanTexture (Texture2D source)
	// {
	// 	RenderTexture renderTex = RenderTexture.GetTemporary (source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

	// 	Graphics.Blit (source, renderTex);
	// 	RenderTexture previous = RenderTexture.active;
	// 	RenderTexture.active = renderTex;
	// 	Texture2D readableText = new Texture2D (source.width, source.height);
	// 	readableText.ReadPixels (new Rect (0, 0, renderTex.width, renderTex.height), 0, 0);
	// 	readableText.Apply ();
	// 	RenderTexture.active = previous;
	// 	RenderTexture.ReleaseTemporary (renderTex);
	// 	return readableText;


	// }

	// IEnumerator checkingInternet_time ()
	// {
	// 	yield return new WaitForSeconds (3f);
	// 	UIController.SharedInstance.net_connection.SetActive (true);
	// 	if (UIController.SharedInstance.LoadingPanel != null) {
	// 		UIController.SharedInstance.LoadingPanel.SetActive (false);			
	// 	}
	// 	if (UIController.SharedInstance.Loading_2_panel != null)
	// 	{
	// 		// Debug.Log("DeactiveLoading2Panel-3");
	// 		UIController.SharedInstance.DeactiveLoading2Panel(0.5f);
	// 	}
	// }

	public void Loading_brandtheme ()
	{
		mStart_downloading = true;
		GetBrands_logo ();

	}

}


[System.Serializable]
public class logodetails
{
	public string imgType;
}

[System.Serializable]
public class bg_Imgdetails
{
	public string brandId;

}