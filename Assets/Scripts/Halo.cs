using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halo : MonoBehaviour
{
    [SerializeField] private Material glowMaterial;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("world2"))
        {
            print("collision");
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material[] materials = new Material[meshRenderer.materials.Length + 1];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                materials[i] = meshRenderer.materials[i];
            }
            materials[materials.Length - 1] = glowMaterial;
            meshRenderer.materials = materials;
            
            MeshRenderer meshRenderer2 = other.gameObject.GetComponent<MeshRenderer>();
            Material[] materials2 = new Material[meshRenderer2.materials.Length + 1];
            for (int i = 0; i < meshRenderer2.materials.Length; i++)
            {
                materials2[i] = meshRenderer2.materials[i];
            }
            materials2[materials2.Length - 1] = glowMaterial;
            meshRenderer2.materials = materials2;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("world2"))
        {
            print("exit collision");
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material[] materials = new Material[meshRenderer.materials.Length - 1];
            int j = 0;
            for (int i = 0; i < materials.Length; i++)
            {
                if (meshRenderer.materials[i] != glowMaterial)
                {
                    materials[j] = meshRenderer.materials[i];
                    j++;
                }
            }
            meshRenderer.materials = materials;
            
            MeshRenderer meshRenderer2 = other.gameObject.GetComponent<MeshRenderer>();
            Material[] materials2 = new Material[meshRenderer2.materials.Length - 1];
            int k = 0;
            for (int i = 0; i < materials2.Length; i++)
            {
                if (meshRenderer2.materials[i] != glowMaterial)
                {
                    materials2[k] = meshRenderer2.materials[i];
                    k++;
                }
            }
            meshRenderer2.materials = materials2;
        }
    }
    
}
