using UnityEngine;
using System.Collections;

public class ImagesLoader : MonoBehaviour
{
	private static Hashtable imageCache = new Hashtable ();

	public static Texture2D getTexture (IMAGE_TYPE IMAGE_ID)
	{
		if (imageCache [IMAGE_ID] == null) {
			
			if (imageCache.ContainsKey (IMAGE_ID)) {
				imageCache [IMAGE_ID] = Resources.Load ("TEXTURES/" + IMAGE_ID) as Texture2D;
			} else {
				imageCache.Add (IMAGE_ID, Resources.Load ("TEXTURES/" + IMAGE_ID) as Texture2D);
			}
#if DEBUG_
#endif
		}
		
		return imageCache [IMAGE_ID] as Texture2D;
	}

	public static void  ReleaseAllImages ()
	{
		if (imageCache.Count > 0) {
			imageCache.Clear ();	
			Resources.UnloadUnusedAssets ();
		}
	}
}
