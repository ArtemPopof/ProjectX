using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    public bool isDebugMode = true;

    void Start()
    {
        if (!isDebugMode)
        {
            removeDebugRenderingComponent();
        }
    }

    private void removeDebugRenderingComponent()
    {
        GetComponent<MeshRenderer>().gameObject.SetActive(false);
    }
}
