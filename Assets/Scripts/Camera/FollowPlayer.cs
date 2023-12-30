using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerLayer");
        }

        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogError("PlayerLayer not found!");
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
