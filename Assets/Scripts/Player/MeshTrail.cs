using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 0.2f;
    public float cooldownTime = 1f; // Added cooldown time

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.05f;
    public float meshDestroyDelay = 0.2f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;

    private bool isTrailActive = false;
    private float nextTrailActivationTime = 0f; // Added next activation time
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public PlayerController playerController;

    private void Update()
    {
        if (playerController.IsDashing == true && !isTrailActive && Time.time > nextTrailActivationTime)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
            nextTrailActivationTime = Time.time + cooldownTime; // Set the next activation time
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }
}
