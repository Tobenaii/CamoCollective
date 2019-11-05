using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorToggler : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Cursor.visible = !Cursor.visible;
    }
}
