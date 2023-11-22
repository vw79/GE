using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    public List<Transform> targets;
    public Transform selectedTarget;

    private Transform myTransform;

    void Start()
    {
        targets = new List<Transform>();
        selectedTarget = null;
        myTransform = transform;

        AddAllEnemies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TargetEnemy();
            SelectTarget();
        }
    }

    public void AddAllEnemies()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in go)
        {
            AddTarget(enemy.transform);
        }
    }

    public void AddTarget (Transform enemy)
    {
        targets.Add(enemy);
    }

    private void SortTargetsByDistance()
    {
        targets.Sort(delegate (Transform t1, Transform t2)
        {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }

    private void TargetEnemy()
    {
        if (selectedTarget == null)
        {
            SortTargetsByDistance();
            selectedTarget = targets[0];
        }
        else
        {
            int index = targets.IndexOf(selectedTarget);

            if (index < targets.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }

            DeselectTarget();
            selectedTarget = targets[index];
        }
        SelectTarget();
    }

    private void SelectTarget()
    {
        selectedTarget.GetComponent<Renderer>().material.color = Color.red;

        PlayerAttack pa = (PlayerAttack)GetComponent("PlayerAttack");
        pa.target = selectedTarget.gameObject;
    }

    private void DeselectTarget()
    {
        selectedTarget.GetComponent<Renderer>().material.color = Color.blue;

        selectedTarget = null;
    }
}
