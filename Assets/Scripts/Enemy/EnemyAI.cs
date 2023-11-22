using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public int moveSpeed;
    public int rotationSpeed;
    public int maxDistance;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        maxDistance = 2;
    }

    void Update()
    {
        //Look at target
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        { 
            //Move towards target
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
