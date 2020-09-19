using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Net;

/**
 * The weatherTeller class is used to print and display the current weather status.
 * - Created by Brian Yu
 */
public class weatherTeller : MonoBehaviour
{
    // Each game object for different weather scenarios
    public GameObject weatherTextObject;
    public GameObject sunObject;
    public GameObject cloudObject1;
    public GameObject cloudObject2;
    public GameObject cloudObject3;
    public GameObject cloudObject4;
    public GameObject snowObject;
    public GameObject rainObject;
    public GameObject lightningObject;

    // Audio sources for different weather
    public AudioSource rain;
    public AudioSource thunder;
    public AudioSource clear;

    // Values for debug testing
    public int testDebugValue = 0;
    public float keyDelay = 0.03f; // Note: this is buggy
    private float timePassed = 0f;
    public bool debugWeather = true; // Use this to choose whether to debug the weather

    string weatherText = "";
    // URL for openweather api
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=77336270cbb2a677ea2c04d0c7e754b5&units=imperial";

    void Start()
    {
        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
        InvokeRepeating("UpdateTime", 0f, 10f);

        // Hide all game objects until necessary to display
        sunObject.SetActive(false);
        cloudObject1.SetActive(false);
        cloudObject2.SetActive(false);
        cloudObject3.SetActive(false);
        cloudObject4.SetActive(false);
        snowObject.SetActive(false);
        rainObject.SetActive(false);
        lightningObject.SetActive(false);
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
                // Handle the JSON data into resonable inputs
                int start = jsonText.IndexOf("icon");
                weatherText = jsonText.Substring(start + 7, 2);
            }
        }
    }

    // This version of update used to handle debugging
    void Update()
    {
        timePassed += Time.deltaTime;
        // Array to hold all possible weather conditions
        var weatherDebugArr = new string[] { "01", "02", "03", "04", "09", "10", "11", "13", "50" };
        // Rotate objects for motion
        sunObject.transform.Rotate(0, 0, 50 * Time.deltaTime);
        snowObject.transform.Rotate(0, 0, 50 * Time.deltaTime);
        // Debug way with arrow keys, note, current implementation requires multiple presses that might not work.
        // This is because without the conditions below, it will overflow or underflow when holding down.
        if (debugWeather && timePassed >= keyDelay)
        {
            weatherText = weatherDebugArr[testDebugValue];
            // Used to loop back to start
            if (Input.GetKey("right"))
            {
                testDebugValue++;
                if (testDebugValue > 8)
                {
                    testDebugValue = 0;
                }
            }
            // Used to loop to end
            if (Input.GetKey("left"))
            {
                testDebugValue--;
                if (testDebugValue < 0)
                {
                    testDebugValue = 8;
                }
            }
            // Reset timer to prevent passing multiple at the same time.
            timePassed = 0f;
        }
    }

    void UpdateTime()
    {
        weatherTextObject.GetComponent<TextMeshPro>().text = weatherText;
        // Stop all audio sources to prevent double playing audio
        clear.Stop();
        thunder.Stop();
        rain.Stop();
        // Switch statement for each weather condition
        // Note set everything to inactive except the ones that display the correct weather.
        // Also displays extra text to show the weather
        // Plays sound for each part that are necessary
        switch (weatherText)
        {
            case "01":
                // Clear Skies condition
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject3.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Clear Sky";
                sunObject.SetActive(true);
                clear.Play();
                break;
            case "02":
                // Few Clouds condition
                cloudObject2.SetActive(false);
                cloudObject3.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Few Clouds";
                sunObject.SetActive(true);
                cloudObject1.SetActive(true);
                clear.Play();
                break;
            case "03":
                // Scattered Clouds condition
                cloudObject1.SetActive(false);
                cloudObject3.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Scattered Clouds";
                sunObject.SetActive(true);
                cloudObject2.SetActive(true);
                clear.Play();
                break;
            case "04":
                // Broken Clouds condition
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Broken Clouds";
                sunObject.SetActive(true);
                cloudObject3.SetActive(true);
                clear.Play();
                break;
            case "09":
                // Shower Rain condition
                sunObject.SetActive(false);
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Shower Rain";
                rainObject.SetActive(true);
                cloudObject3.SetActive(true);
                rain.Play();
                break;
            case "10":
                // Rain condition
                sunObject.SetActive(false);
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject3.SetActive(false);
                snowObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Rain";
                cloudObject4.SetActive(true);
                rainObject.SetActive(true);
                rain.Play();
                break;
            case "11":
                // Thunderstorm condition
                thunder.Play();
                sunObject.SetActive(false);
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject3.SetActive(false);
                snowObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Thunderstorm";
                cloudObject4.SetActive(true);
                rainObject.SetActive(true);
                lightningObject.SetActive(true);
                break;
            case "13":
                // Snow condition
                sunObject.SetActive(false);
                cloudObject1.SetActive(false);
                cloudObject2.SetActive(false);
                cloudObject4.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Snow";
                cloudObject3.SetActive(true);
                snowObject.SetActive(true);
                break;
            case "50":
                // Mist condition
                sunObject.SetActive(false);
                cloudObject4.SetActive(false);
                snowObject.SetActive(false);
                rainObject.SetActive(false);
                lightningObject.SetActive(false);
                weatherTextObject.GetComponent<TextMeshPro>().text = "Mist";
                cloudObject1.SetActive(true);
                cloudObject2.SetActive(true);
                cloudObject3.SetActive(true);
                break;
        }
    }
}