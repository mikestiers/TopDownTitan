using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class AzureTableManager : MonoBehaviour
{
    public Text highScoresText;
    
    void Start()
    {
        StartCoroutine(GetTableData());
        Debug.Log("Azure started");
    }

    IEnumerator GetTableData()
    {
        string requestUri = $"https://topdowntitan.table.core.windows.net/highscores?sp=raud&st=2023-12-13T11:26:07Z&se=2024-01-06T00:27:00Z&sv=2022-11-02&sig=cH%2BPkbPPlBodZRYQZoCFuo5lzkHUGM204tYhrlM0jnU%3D&tn=highscores";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(requestUri))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                // Handle the response
                string responseData = webRequest.downloadHandler.text;
                Debug.Log(responseData);
                ParseXmlResponse(responseData);
            }
        }
    }
    public void ParseXmlResponse(string xml)
    {
        XDocument xmlDoc = XDocument.Parse(xml);

        List<Highscore> highscores = new List<Highscore>();

        XNamespace ns = xmlDoc.Root.GetDefaultNamespace();
        XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        foreach (XElement entry in xmlDoc.Root.Elements(ns + "entry"))
        {
            //Debug.Log("Entry: " + entry);

            XElement content = entry.Element(ns + "content");
            XElement properties = content?.Element(m + "properties");
            if (properties != null)
            {
                //Debug.Log("Properties found");

                // Now that we have the properties element, extract the individual properties
                XElement partitionKeyElement = properties.Element(d + "PartitionKey");
                XElement rowKeyElement = properties.Element(d + "RowKey");
                XElement timestampElement = properties.Element(d + "Timestamp");
                XElement nameElement = properties.Element(d + "name");
                XElement scoreElement = properties.Element(d + "score");

                // Check if all elements are found
                if (partitionKeyElement != null && rowKeyElement != null &&
                    timestampElement != null && nameElement != null && scoreElement != null)
                {
                    //Debug.Log($"Name: {nameElement.Value}, Score: {scoreElement.Value}");
                    highscores.Add(new Highscore
                    {
                        name = nameElement.Value,
                        score = int.Parse(scoreElement.Value)
                    });
                }
                else
                {
                    Debug.Log("One or more elements within properties are missing.");
                }
            }
            else
            {
                Debug.Log("Properties element not found");
            }
        }
        UpdateHighScoresUI(highscores);
    }

    [System.Serializable]
    public class Highscore
    {
        public string RowKey;
        public string PartitionKey;
        public string name;
        public int score;
    }

    private void UpdateHighScoresUI(List<Highscore> highscores)
    {
        var sortedHighScores = highscores.OrderByDescending(h => h.score).ToList();

        highScoresText.text = "High Scores:\n";
        foreach (var highscore in sortedHighScores)
        {
            highScoresText.text += $"{highscore.name}: {highscore.score}\n";
        }
    }
}
