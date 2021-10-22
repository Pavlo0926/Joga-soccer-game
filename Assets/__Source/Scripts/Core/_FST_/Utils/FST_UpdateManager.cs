using Jiweman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
public static class FST_UpdateManager
{
    public static bool IsDone = false;
    public static void Test()
    {
        string savePath = "Resources/5MB.zip";//save path = parentfolder/filename, the rest of rootdir is handled here 

        if (!savePath.StartsWith("/"))
            savePath = savePath.Insert(0, "/");

        string url = "http://ipv4.download.thinkbroadband.com/5MB.zip";//where to download from

        Debug.Log("player path = " + Application.persistentDataPath + savePath);
        Debug.Log("editor path = " + Application.dataPath + savePath);
        PartialUpdateInternal(url, savePath);
    }
    public static void PartialUpdate(JSONNode data)
    {
        string savePath = data["data"]["savePath"];//save path = parentfolder/filename, the rest of rootdir is handled here 
        string url = data["data"]["url"];//where to download from

        PartialUpdateInternal(url, savePath);
    }

    public static void PartialUpdate(string url, string savePath)
    {
        PartialUpdateInternal(url, savePath);
    }

    private static void PartialUpdateInternal(string url, string savePath)
    {
        if (!savePath.StartsWith("/"))
            savePath = savePath.Insert(0, "/");

        //setup the download
#if UNITY_EDITOR
        string path = Application.dataPath + savePath;
#else
        string path = Application.persistentDataPath + savePath;
#endif

        DownloadHandlerFile downloadHandlerFile = new DownloadHandlerFile(path);

        UnityWebRequest unityWebRequest = new UnityWebRequest(url)
        {
            downloadHandler = downloadHandlerFile
        };
        unityWebRequest.SendWebRequest();

        MonoBehaviour surrogate = GameObject.FindObjectOfType<MonoBehaviour>();

        Debug.Log("download begin! savepath : " + path);

        surrogate.StartCoroutine(Download(unityWebRequest));
    }

    private static IEnumerator Download(UnityWebRequest webRequest)
    {
        IsDone = false;

        while (!webRequest.isDone && !webRequest.isHttpError)
        {    //loading progress bar here
            Debug.Log(webRequest.isDone);
            Debug.Log(webRequest.downloadProgress);
            yield return 0;
        }

        if (webRequest.isHttpError)
            Debug.LogWarning("download failed!");
        else Debug.Log("download done!");

        webRequest.Dispose();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#else
        //wait a frame for final write complete on android/iOS..
        yield return 0;
#endif
        IsDone = true;

        //for later, the server guys need to understand how this works first and learn how to create bundles
        //using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle("http://ipv4.download.thinkbroadband.com/5MB.zip"))
        //{
        //    yield return uwr.SendWebRequest();

        //    if (uwr.isNetworkError || uwr.isHttpError)
        //    {
        //        Debug.Log(uwr.error);
        //    }
        //    else
        //    {
        //        // Get downloaded asset bundle
        //        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        //        Debug.Log(bundle.name + "was dl");
        //    }
        //}
    }
    /*    public static void Save(byte[] data, string path)
    {
        var file = new FileInfo(path);
        file.Directory.Create();

        File.WriteAllBytes(path, data);

        data = null;
    }*/
}
