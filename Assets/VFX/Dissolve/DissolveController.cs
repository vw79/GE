using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController: MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;
    private float dissolveRate = 0.0125f;
    private float RefreshRate = 0.025f; // Double of dissolveRate
    private GameObject sword;
    private GameObject player;
    private Animator animator;
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private CapsuleCollider playerCollider;

    private Material[] skinnedMaterials;

    private void Awake()
    {
        skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        sword = GameObject.FindWithTag("Sword");
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    void Start()
    {
        if(skinnedMesh !=null)
            skinnedMaterials = skinnedMesh.materials;     
    }

    public void PlayerDeath()
    {
        playerCombat.enabled = false;
        playerController.enabled = false;
        animator.Play("PlayerDeath");
        StartCoroutine(DeathStart());
    }

    IEnumerator DeathStart()
    {
        yield return new WaitForSeconds(2.14f);
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        sword.SetActive(false);

        float counter = 0;
        while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            for(int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
            }

            yield return new WaitForSeconds(RefreshRate);
        }

        Destroy(player);
    }
}

