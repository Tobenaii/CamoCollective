using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCopy : MonoBehaviour
{
    [SerializeField]
    private Transform m_other;

    private Transform[] m_otherTransforms;
    private Transform[] m_transforms;

    private void Awake()
    {
        m_transforms = GetComponentsInChildren<Transform>();
        m_otherTransforms = m_other.GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        int i = 0;
        foreach (Transform t1 in m_transforms)
        {
            t1.localPosition = m_otherTransforms[i].localPosition;
            t1.localRotation = m_otherTransforms[i].localRotation;
            i++;
        }
    }
}
