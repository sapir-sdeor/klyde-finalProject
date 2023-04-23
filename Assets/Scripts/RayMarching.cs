using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarching : MonoBehaviour
{

    [SerializeField] private float strength = 1f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private bool isCube = false;

    private Texture3D sdfTexture;

    private void Start()
    {
        int textureSize = 64; // Change this to adjust the resolution of the SDF texture

        sdfTexture = new Texture3D(textureSize, textureSize, textureSize, TextureFormat.RFloat, false);
        sdfTexture.wrapMode = TextureWrapMode.Clamp;
        sdfTexture.filterMode = FilterMode.Bilinear;

        Color[] colors = new Color[textureSize * textureSize * textureSize];

        Vector3 center = transform.position;
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                for (int z = 0; z < textureSize; z++)
                {
                    Vector3 position = new Vector3((float)x / textureSize, (float)y / textureSize, (float)z / textureSize);
                    position -= center;
                    float distance = isCube ? CubeSDF(position, radius) : SphereSDF(position, radius);
                    float density = strength / Mathf.Max(distance, 0.0001f); // Avoid division by zero
                    colors[x + y * textureSize + z * textureSize * textureSize] = new Color(density, 0, 0, 0);
                }
            }
        }

        sdfTexture.SetPixels(colors);
        sdfTexture.Apply();
    }

    private void OnEnable()
    {
        Shader.EnableKeyword("_METABALLS");
    }

    private void OnDisable()
    {
        Shader.DisableKeyword("_METABALLS");
    }

    private float SphereSDF(Vector3 position, float radius)
    {
        return position.magnitude - radius;
    }

    private float CubeSDF(Vector3 position, float size)
    {
        position = Vector3.Max((position) - Vector3.one * size, Vector3.zero);
        return position.magnitude;
    }

    private void OnValidate()
    {
        radius = Mathf.Max(radius, 0.01f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnDestroy()
    {
        Destroy(sdfTexture);
    }

    private void Update()
    {
        Shader.SetGlobalVector("_MetaballPosition", transform.position);
        Shader.SetGlobalFloat("_MetaballStrength", strength);
        Shader.SetGlobalFloat("_MetaballRadius", radius);
        Shader.SetGlobalTexture("_MetaballSDF", sdfTexture);
    }
}


