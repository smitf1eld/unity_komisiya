using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformation : MonoBehaviour
{
    public float intensity = 0.2f;
    public float recoverySpeed = 2f;
    public float maxDeformation = 0.3f;

    private Mesh originalMesh;
    private Mesh clonedMesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;

    private void Start()
    {
        originalMesh = GetComponent<MeshFilter>().mesh;
        clonedMesh = Instantiate(originalMesh);
        GetComponent<MeshFilter>().mesh = clonedMesh;

        originalVertices = originalMesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
        System.Array.Copy(originalVertices, modifiedVertices, originalVertices.Length);
    }

    private void Update()
    {
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            modifiedVertices[i] = Vector3.Lerp(modifiedVertices[i], originalVertices[i], Time.deltaTime * recoverySpeed);
        }

        clonedMesh.vertices = modifiedVertices;
        clonedMesh.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            for (int i = 0; i < modifiedVertices.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(modifiedVertices[i]);
                float distance = Vector3.Distance(worldPos, contact.point);

                if (distance < 0.5f)
                {
                    Vector3 deformation = contact.normal * intensity;

                    if (deformation.magnitude > maxDeformation)
                    {
                        deformation = deformation.normalized * maxDeformation;
                    }

                    modifiedVertices[i] += deformation;
                }
            }
        }
    }
}