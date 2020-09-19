using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using TMPro;
using System.Net;

public class tempTeller : MonoBehaviour
{
    public GameObject weatherTextObject;
    public GameObject weatherTextObject2;

    public GameObject mercury;
    public GameObject water;

    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=77336270cbb2a677ea2c04d0c7e754b5&units=imperial";
    string tempText = "";
    string humidityText = "";

    bool once = true;
    void Start()
    {
        // wait a couple seconds to start and then refresh every 900 seconds
        var waterRenderer = water.GetComponent<Renderer>();
        waterRenderer.material.SetColor("_Color", Color.blue);
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
                int start = jsonText.IndexOf("temp");
                tempText = jsonText.Substring(start + 6, 4);

                int start2 = jsonText.IndexOf("humidity");
                humidityText = jsonText.Substring(start2 + 10, 2);
            }
        }
    }

    void UpdateTime()
    {
        if (once)
        {
            Vector3 positionChange = new Vector3(mercury.transform.position.x, mercury.transform.position.y - ((100-float.Parse(tempText)) / 100) * 0.04f, mercury.transform.position.z);
            Vector3 scaleChange = new Vector3(0.005f, (float.Parse(tempText) / 100) * 0.04f, 0.005f);
            mercury.transform.position = positionChange;
            mercury.transform.localScale = scaleChange;
            Vector3 positionChangeWater = new Vector3(water.transform.position.x, water.transform.position.y - ((100-float.Parse(humidityText))/100) * 0.02f, water.transform.position.z);
            Vector3 scaleChangeWater = new Vector3(0.035f, (float.Parse(humidityText)/100) * 0.02f, 0.035f);
            water.transform.position = positionChangeWater;
            water.transform.localScale = scaleChangeWater;
        }
        once = false;
        weatherTextObject2.GetComponent<TextMeshPro>().text = humidityText + "%";
        weatherTextObject.GetComponent<TextMeshPro>().text = tempText + " F";
    }
}