using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float refreshRate;

    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
    }

    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh mesh = new Mesh();
            skinnedMesh.BakeMesh(mesh);

            Vector3[] vertices = mesh.vertices;
            Mesh mesh2 = new Mesh();
            mesh2.vertices = vertices;

            VFXGraph.SetMesh("Mesh", mesh2);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
