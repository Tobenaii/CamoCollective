using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    public void Jump()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 20, ForceMode.Impulse);
    }

    public void Move(Vector2 stick)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(stick.x * 100, 0, stick.y * 100);
    }
}
