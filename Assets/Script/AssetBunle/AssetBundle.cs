using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
public class AssetBundle : MonoBehaviour
{
    public string url;
    public UnityEngine.AssetBundle assetBundle; // Use fully qualified name
    public Text labelText;
    public string SceneNameToLoadAB;

    void Start()
    {
        StartCoroutine(DownloadFiles());
    }

    IEnumerator DownloadFiles()
    {
        if (assetBundle == null)
        {
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                Debug.Log("Using UnityWebRequest");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    yield break;
                }

                assetBundle = DownloadHandlerAssetBundle.GetContent(www);
            }
        }

        string[] scenes = assetBundle.GetAllScenePaths();
        Debug.Log("scenes.Length: " + scenes.Length);
        foreach (string scenename in scenes)
        {
            SceneNameToLoadAB = Path.GetFileNameWithoutExtension(scenename);
            Debug.Log("SceneNameInPath(foreach): " + Path.GetFileNameWithoutExtension(scenename));
        }

      //  labelText.text = "SceneNameToLoadAB: " + SceneNameToLoadAB;
    }

    public void LoadAssetBundleScene()
    {
        SceneManager.LoadScene(SceneNameToLoadAB);
        Debug.Log("clicked on btn to play the scene");
    }
}
