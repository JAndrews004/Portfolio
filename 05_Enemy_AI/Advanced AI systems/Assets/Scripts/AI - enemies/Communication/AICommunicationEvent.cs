using UnityEngine;

public class AICommunicationEvent
{
    public AICommunicationType Type;
    public AIController Sender;
    public Vector3 Position;
    public IDetectableEntity Target;
    public float Confidence;
    public float TimeStamp;
}

public class AIChannelRequestEvent
{
    public CommunicationChannel channel;
    public IDetectableEntity requester;
    public ChannelAction action;
}

public enum AICommunicationType
{
    Investigating,
    PlayerSpotted,
    Suspicious,
    Emergency,
}

public enum ChannelAction
{
    Request,
    Release,

}