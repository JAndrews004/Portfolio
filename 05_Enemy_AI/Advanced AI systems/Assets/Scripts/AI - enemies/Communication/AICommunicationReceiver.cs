using UnityEngine;

public class AICommunicationReceiver
{
    AIController controller;
    public AICommunicationReceiver(AIController controller)
    {
        this.controller = controller;
    }
    public void Receive(AICommunicationEvent e)
    {
        Debug.Log($"Received communication from {e.Sender.communicationGroup.CommunicationGroupName}");
        Stimulus stimulus = new Stimulus();
        stimulus.position = e.Position;
        stimulus.type = StimulusType.Communication;
        stimulus.source = e.Target;

        stimulus.strength = 0.25f;

        stimulus.confidence = e.Confidence;
        
        controller.suspicionSystem.ReceiveCommunicationStimulus(stimulus);
    }
}
