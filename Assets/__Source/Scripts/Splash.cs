using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public GameObject Background;
    public Image LoadingImg;
    public Text Load;
    public GameObject LoadingPrefab;

    private void Awake()
    {
#if !UNITY_EDITOR
        Screen.sleepTimeout = -1;
#endif
        Application.targetFrameRate = 100;
        LoadingImg.fillAmount = 0;
    }

    private void Start()
    {
        StartCoroutine(LoadSceneAfter());
    }

    IEnumerator LoadSceneAfter()
    {
      //  yield return new WaitForSeconds(1f);

      //  Background.SetActive(false);
       // LoadingPrefab.SetActive(true);

        yield return new WaitForSeconds(2f);

        AsyncOperation Loading = SceneManager.LoadSceneAsync("MainMenu");
        while (!Loading.isDone)
        {
            float progressed = Mathf.Clamp01(Loading.progress / 0.9f);

            Load.text = progressed.ToString ();
            LoadingImg.fillAmount = progressed;

            yield return null;
        }
    }
}

