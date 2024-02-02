using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiAgent : MonoBehaviour
{
    //all referencing needed for each CurrentState are done here
    public EnemyAIStateMachine stateMachine;
    public EnemyStateId initialState;
    public NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //stateMachine = new EnemyAIStateMachine(this);
        //to register a new CurrentState
        stateMachine.RegisterState(new EnemyChaseState());
        //stateMachine.RegisterState(new EnemyPatrolState());
        stateMachine.RegisterState(new EnemyDeathState());
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        stateMachine.Update();
    }
}
