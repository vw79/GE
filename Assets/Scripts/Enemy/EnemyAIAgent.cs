using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyDeathState;

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
        stateMachine.RegisterState(new EnemyIdle());
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        stateMachine.Update();
    }
}
