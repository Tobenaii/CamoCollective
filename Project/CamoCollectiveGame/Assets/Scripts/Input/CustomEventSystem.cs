using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventSystem : EventSystem
{
    // Update is called once per frame
    protected override void Update()
    {
        current = this;
        base.Update();
    }
}
