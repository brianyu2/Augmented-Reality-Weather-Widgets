using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Net;

public class windTeller : MonoBehaviour
{
    public GameObject weatherTextObject;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=77336270cbb2a677ea2c04d0c7e754b5&units=imperial";
    string tempText = "";

    void Start()
    {

        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
        InvokeRepeating("UpdateTime", 0f, 10f);

    }

    void GetDataFromWeb()
    {

        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                string jsonText = webRequest.downloadHandler.text;
                int start = jsonText.IndexOf("wind");
                tempText = jsonText.Substring(start + 15, 3);
            }
        }
    }

    void UpdateTime()
    {
        weatherTextObject.GetComponent<TextMeshPro>().text = tempText + " mph";
    }
}