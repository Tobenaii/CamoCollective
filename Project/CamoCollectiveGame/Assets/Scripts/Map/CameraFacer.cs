using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(timer) * 0.1f, transform.position.z);
        //transform.LookAt(Camera.main.transform);
        //transform.Rotate(Vector3.up, 180);
    }
}
