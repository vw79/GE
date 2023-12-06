using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player; 
    public Vector3 minCameraPos, maxCameraPos; 

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 newPos = player.transform.position;

            newPos.x = Mathf.Clamp(newPos.x, minCameraPos.x, maxCameraPos.x);
            newPos.y = Mathf.Clamp(newPos.y, minCameraPos.y, maxCameraPos.y);
            newPos.z = Mathf.Clamp(newPos.z, minCameraPos.z, maxCameraPos.z); 

            transform.position = newPos;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minCameraPos.x, minCameraPos.y, minCameraPos.z), new Vector3(maxCameraPos.x, minCameraPos.y, minCameraPos.z));
        Gizmos.DrawLine(new Vector3(maxCameraPos.x, minCameraPos.y, minCameraPos.z), new Vector3(maxCameraPos.x, maxCameraPos.y, maxCameraPos.z));
        Gizmos.DrawLine(new Vector3(maxCameraPos.x, maxCameraPos.y, maxCameraPos.z), new Vector3(minCameraPos.x, maxCameraPos.y, minCameraPos.z));
        Gizmos.DrawLine(new Vector3(minCameraPos.x, maxCameraPos.y, minCameraPos.z), new Vector3(minCameraPos.x, minCameraPos.y, minCameraPos.z));
    }
}