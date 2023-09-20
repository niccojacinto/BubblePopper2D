using System.Collections;
using TMPro;
using UnityEngine;

public class TextWave : MonoBehaviour
{
    public float frequency = 1.0f; // Adjust the frequency of the wave.
    public float amplitude = 1.0f; // Adjust the amplitude of the wave.
    public float speed = 1.0f;     // Adjust the speed of the wave.

    private TextMesh textMesh;
    private Vector3[] originalVertices;
    private float time = 0.0f;

    private void Start()
    {
        //textMesh = GetComponent<TextMesh>();
        //originalVertices = textMesh.textInfo.meshInfo[0].vertices.Clone() as Vector3[];
        //textMesh
    }

    private void Update()
    {
        //time += Time.deltaTime * speed;

        //Vector3[] vertices = textMesh.textInfo.meshInfo[0].vertices;

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    Vector3 originalVertex = originalVertices[i];
        //    vertices[i] = originalVertex + Vector3.up * Mathf.Sin(time + originalVertex.x * frequency) * amplitude;
        //}

        //textMesh.UpdateVertexData();
    }
}
