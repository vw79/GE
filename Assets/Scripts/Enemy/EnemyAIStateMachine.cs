using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateId
{
    Chasing,
    Attacking,
    Death
}

public interface EnemyState
{
    EnemyStateId getID();
    void Enter(EnemyAI agent);
    void Update(EnemyAI agent);
    void Exit(EnemyAI agent);
}
public class EnemyAIStateMachine 
{
    public EnemyState[] states;
    public EnemyAI agent;
    public EnemyStateId currentState;

    public EnemyAIStateMachine(EnemyAI agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(EnemyStateId)).Length;
        states = new EnemyState[numStates];
    }

    public void RegisterState(EnemyState state)
    {
        int index = (int)state.getID();
        states[index] = state;
    }

    public EnemyState GetState(EnemyStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        Debug.Log(currentState);
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(EnemyStateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
