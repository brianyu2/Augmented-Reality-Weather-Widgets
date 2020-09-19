using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * The timeTeller class is used to print the current time.
 * - Created by Brian Yu
 */

public class timeTeller : MonoBehaviour
{
    public GameObject timeTextObject;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 10f);
    }

    // Update is called once per frame
    void UpdateTime()
    {
        // Prints the current time in h:mm tt format
        timeTextObject.GetComponent<TextMeshPro>().text = System.DateTime.Now.ToString("h:mm tt");
    }
}