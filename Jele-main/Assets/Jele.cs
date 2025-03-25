using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jele : MonoBehaviour
{
    public float stiffness = 5f;
    public float damping = 0.5f;
    public float mass = 1f;

    private Mesh originalMesh;
    private Mesh clonedMesh;
    private Vector3[] originalVertices;
    private JellyVertex[] jellyVertices;

    private void Start()
    {
        originalMesh = GetComponent<MeshFilter>().mesh;
        clonedMesh = Instantiate(originalMesh);
        GetComponent<MeshFilter>().mesh = clonedMesh;

        originalVertices = originalMesh.vertices;
        jellyVertices = new JellyVertex[originalVertices.Length];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(transform.TransformPoint(originalVertices[i]));
        }
    }

    private void Update()
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].Update(stiffness, damping, mass, transform.TransformPoint(originalVertices[i]));
            originalVertices[i] = transform.InverseTransformPoint(jellyVertices[i].position);
        }

        clonedMesh.vertices = originalVertices;
        clonedMesh.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            for (int i = 0; i < jellyVertices.Length; i++)
            {
                float force = collision.relativeVelocity.magnitude * 0.1f;
                jellyVertices[i].ApplyForce((jellyVertices[i].position - contact.point).normalized * force);
            }
        }
    }
}

public class JellyVertex
{
    public Vector3 position;
    private Vector3 velocity;
    private Vector3 force;

    public JellyVertex(Vector3 position)
    {
        this.position = position;
    }

    public void Update(float stiffness, float damping, float mass, Vector3 restPosition)
    {
        force = (restPosition - position) * stiffness - velocity * damping;
        velocity += force / mass * Time.deltaTime;
        position += velocity * Time.deltaTime;
    }

    public void ApplyForce(Vector3 force)
    {
        velocity += force;
    }
}