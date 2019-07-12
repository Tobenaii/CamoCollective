using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTesting : MonoBehaviour
{
    public Text t;
    string text;
    // Update is called once per frame
    void Update()
    {
        text = "";
        foreach (string controller in Input.GetJoystickNames())
        {
            text += controller + "\n";
        }
        t.text = text;
    }
}
