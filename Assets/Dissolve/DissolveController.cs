using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController: MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float RefreshRate = 0.025f; // Double of dissolveRate
    public GameObject sword;

    private Material[] skinnedMaterials;

    void Start()
    {
        if(skinnedMesh !=null)
            skinnedMaterials = skinnedMesh.materials;     
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Destroy(sword);
            StartCoroutine(Dissolve());
        }       
    }

    IEnumerator Dissolve()
    {
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
    }
}

