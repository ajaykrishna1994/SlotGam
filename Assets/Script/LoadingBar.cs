using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{

    public Image sliderNew;
    public TextMeshProUGUI progressText;
    public string assetBundleUrl;
    public string SceneNameToLoadAB;

    private AudioManager audiomanager;
    private UnityEngine.AssetBundle assetBundle;

    void Start()
    {
        audiomanager = FindAnyObjectByType<AudioManager>();
        audiomanager.Play("Bgm");
        StartCoroutine(DownloadAndDisplayProgress(assetBundleUrl));
    }

    IEnumerator DownloadAndDisplayProgress(string url)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            www.SendWebRequest();

            while (!www.isDone)
            {
                float progress = Mathf.Clamp01(www.downloadProgress / 0.9f);
                sliderNew.fillAmount = progress;
                progressText.text = (progress * 100f).ToString("F0") + "%";
                yield return null;
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                yield break;
            }

            assetBundle = DownloadHandlerAssetBundle.GetContent(www);

            // Ensure progress is 100% before proceeding
            sliderNew.fillAmount = 1f;
            progressText.text = "100%";
        }

        // Load the scene from the asset bundle
        string[] scenes = assetBundle.GetAllScenePaths();
        if (scenes.Length > 0)
        {
            SceneNameToLoadAB = Path.GetFileNameWithoutExtension(scenes[0]);
            SceneManager.LoadScene(SceneNameToLoadAB);
        }
    }

}
