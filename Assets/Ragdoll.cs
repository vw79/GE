using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public CapsuleCollider mainCollider;
    public Rigidbody mainRigidbody;
    public GameObject mobs;
    public Animator animator;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    void Start()
    {
        GetRagdollCollider();
        RagdollOff();
    }

    void GetRagdollCollider()
    {
        ragdollColliders = mobs.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = mobs.GetComponentsInChildren<Rigidbody>();

    }
    public void RagdollOn()
    {
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }

        foreach(Rigidbody rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        animator.enabled = false;
        mainCollider.enabled = false;
        mainRigidbody.isKinematic = true;
    }

    private void RagdollOff()
    {
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        foreach(Rigidbody rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
        mainCollider.enabled = true;
        mainRigidbody.isKinematic = false;
    }
}
