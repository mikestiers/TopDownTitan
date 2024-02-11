using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class AzureDataPoster : MonoBehaviour
{
    public void AddHighscore(AzureTableManager.Highscore highscore)
    {
        StartCoroutine(PostHighscore(highscore));
    }

    private IEnumerator PostHighscore(AzureTableManager.Highscore highscore)
    {
        string requestUri = "https://topdowntitan.table.core.windows.net/highscores?sp=raud&st=2023-12-13T11:26:07Z&se=2024-01-06T00:27:00Z&sv=2022-11-02&sig=cH%2BPkbPPlBodZRYQZoCFuo5lzkHUGM204tYhrlM0jnU%3D&tn=highscores";

        // Serialize the highscore object into JSON
        string jsonData = JsonUtility.ToJson(highscore);

        using (UnityWebRequest webRequest = new UnityWebRequest(requestUri, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error posting data: {webRequest.error}");
            }
            else
            {
                Debug.Log("Data posted successfully");
                HUD.singleton.postHighScoreButton.GetComponentInChildren<Text>().text = "Posted!";
                HUD.singleton.postHighScoreButton.interactable = false;
            }
        }
    }
}
