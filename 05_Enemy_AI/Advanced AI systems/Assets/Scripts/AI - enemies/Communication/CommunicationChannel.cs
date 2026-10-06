using System.Collections.Generic;
using UnityEngine;

public class CommunicationChannel
{
    public CommunicationChannelSO channelData;

    public bool IsActive;
    public List<IDetectableEntity> Requesters = new List<IDetectableEntity> { };
}

[CreateAssetMenu(menuName = "Communication/Channel")]
public class CommunicationChannelSO : ScriptableObject
{
    public CommunicationGroup groupA;
    public CommunicationGroup groupB;

    public float CommunicationRange;
}