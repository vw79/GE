using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = player.transform.position + offset;
            transform.position = newPosition;
        }
    }
}
