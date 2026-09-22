using System.Collections.Generic;
using UnityEngine;

public class CommunicationChannel
{
    public CommunicationGroup groupA;
    public CommunicationGroup groupB;

    public bool IsActive;
    public float CommunicationRange;

    public List<IDetectableEntity> Requesters = new List<IDetectableEntity> { };
}

[CreateAssetMenu(menuName = "Communication/Channel")]
public class CommunicationChannelSO : ScriptableObject
{
    public CommunicationGroup groupA;
    public CommunicationGroup groupB;

    public float CommunicationRange;
}