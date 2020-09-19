using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using TMPro;
using System.Net;

public class windTeller : MonoBehaviour
{
    public GameObject weatherTextObject;
    public GameObject windStaff;

    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=77336270cbb2a677ea2c04d0c7e754b5&units=imperial";
    string windSpeedText = "";
    string windDegText = "";
    bool once = true;

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
                windSpeedText = jsonText.Substring(start + 15, 4);
                int start2 = jsonText.IndexOf("deg");
                windDegText = jsonText.Substring(start2 + 5, 3);
            }
        }
    }

    void UpdateTime()
    {
        if (once)
        {
            windStaff.transform.Rotate(0, float.Parse(windDegText), 0);
        }
        once = false;
        weatherTextObject.GetComponent<TextMeshPro>().text = windSpeedText + " mph, " + windDegText + " deg";
    }
}