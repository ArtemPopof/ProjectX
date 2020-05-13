using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisableLook: MonoBehaviour
{
    public List<Material> materials;

    public void ChangeMaterial(int index)
    {
        if (index > materials.Count)
        {
            Debug.LogError("Materials are not configured properly. Index " + index + " does not exist");
            return;
        }

        var meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.material = materials[index];
    }
}
