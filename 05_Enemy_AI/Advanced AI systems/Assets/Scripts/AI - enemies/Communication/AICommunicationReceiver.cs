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
        Debug.Log($"Received communication from {e.Sender.communicationGroup.CommunicationGroupName} with a confidence of {e.Confidence}");
        Stimulus stimulus = new Stimulus();
        stimulus.position = e.Position;
        stimulus.type = StimulusType.Communication;
        stimulus.source = e.Target;

        stimulus.strength = 1f;

        stimulus.confidence = e.Confidence;
        
        controller.suspicionSystem.ReceiveCommunicationStimulus(stimulus);
    }
}
