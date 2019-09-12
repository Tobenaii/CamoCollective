using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLerper
{
    private float _timer;
    private float _newUpdateCheck;

    public void Reset()
    {
        _timer = 0;
    }

    private float GetPercent(float timeToFinish)
    {
        //Get the percent of the finish time with the current time
        if (_newUpdateCheck != Time.time)
            _timer += Time.deltaTime;
        _newUpdateCheck = Time.time;
        return _timer / timeToFinish;
    }

    //Everytime this is called, the Lerp will get a percentage closer towards the end
    public Vector3 Lerp(Vector3 init, Vector3 end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Vector3.Lerp(init, end, lerpPercent);
    }

    public float Lerp(float init, float end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Mathf.Lerp(init, end, lerpPercent);
    }

    public Quaternion Lerp(Quaternion init, Quaternion end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Quaternion.Lerp(init, end, lerpPercent);
    }

    public Quaternion Slerp(Quaternion init, Quaternion end, float timeToFinish)
    {
        float lerpPercent = GetPercent(timeToFinish);
        return Quaternion.Slerp(init, end, lerpPercent);
    }

    public Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float timeToFinish)
    {
        float t = GetPercent(timeToFinish);
        t = Mathf.Clamp(t, 0, 1);
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }
}