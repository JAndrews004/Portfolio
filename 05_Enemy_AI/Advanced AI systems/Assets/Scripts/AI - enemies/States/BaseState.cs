using UnityEditor;
using UnityEngine;

public class BaseState : IAIState
{
    protected AIController controller;
    public string stateName { get; set; }
    protected BaseState(AIController controller)
    {
        this.controller = controller;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void SubscribeToEvent(StateMachine stateMachine) { }
    public virtual void UnSubscribeToEvent(StateMachine stateMachine) { }
    
}
