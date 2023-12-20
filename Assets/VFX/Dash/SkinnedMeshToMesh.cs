using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float refreshRate;
    public PlayerController playerController;

    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
    }

    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            if (playerController.IsDashing)
            {
                Mesh mesh = new Mesh();
                skinnedMesh.BakeMesh(mesh);

                Vector3[] vertices = mesh.vertices;
                Mesh mesh2 = new Mesh();
                mesh2.vertices = vertices;

                VFXGraph.SetMesh("Mesh", mesh2);
                VFXGraph.enabled = true; 
            }
            else
            {
                VFXGraph.enabled = false; 
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
