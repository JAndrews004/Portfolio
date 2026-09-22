using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(AIController controller)
        : base(controller)
    {
        stateName = "Idle";
    }
    public override void Enter() 
    {
        //Debug.Log("Entering idle state");
        controller.SetMotorData(controller.characterMotor.direction, 0.0f, false);

    }
    public override void Update() 
    {
        controller.SetMotorData(controller.characterMotor.direction, 0.0f, false);
    }
    public override void Exit() { }
}
