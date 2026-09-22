using UnityEngine;

public interface IAIState
{
    public string stateName { get; set; }
    void Enter();
    void Update();
    void Exit();

    void SubscribeToEvent(StateMachine stateMachine);
    void UnSubscribeToEvent(StateMachine stateMachine);
}
