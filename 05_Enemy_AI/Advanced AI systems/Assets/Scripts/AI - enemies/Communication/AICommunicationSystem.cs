using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AICommunicationSystem : MonoBehaviour
{
    public List<CommunicationChannelSO> channels;
    Dictionary<CommunicationGroup, List<AIController>> communicationGroups = new Dictionary<CommunicationGroup, List<AIController>> { };
    List<CommunicationChannel> communicationChannels = new List<CommunicationChannel>() { };

    private void Start()
    {
        foreach (var channel in channels)
        {
            CommunicationChannel newChannel = new CommunicationChannel();
            newChannel.groupA = channel.groupA;
            newChannel.groupB = channel.groupB;
            newChannel.IsActive = false;
            newChannel.CommunicationRange = channel.CommunicationRange;
            communicationChannels.Add(newChannel);
        }
    }
    public void Publish(AICommunicationEvent e)
    {
        if (communicationGroups.ContainsKey(e.Sender.communicationGroup))
        {
           foreach(AIController controller in communicationGroups[e.Sender.communicationGroup])
           {
                if(Vector3.Distance(e.Sender.transform.position, controller.transform.position) < e.Sender.communicationGroup.CommunicationRange)
                {
                    if (controller != e.Sender)
                    {
                        controller.aICommunicationReceiver.Receive(e);
                    }
                }
                
           }

           //check active channels -> send communication to active channel recipient 
           foreach(CommunicationChannel channel in communicationChannels)
            {
                
                if (channel.IsActive)
                {
                    CommunicationGroup communicationGroup;
                    if(channel.groupB == e.Sender.communicationGroup)
                    {
                        communicationGroup = channel.groupA;
                    }
                    else if (channel.groupA == e.Sender.communicationGroup)
                    {
                        communicationGroup = channel.groupB;
                    }
                    else
                    {
                        return;
                    }
                    if (communicationGroups.ContainsKey(communicationGroup))
                    {
                        foreach (AIController controller in communicationGroups[communicationGroup])
                        {
                            if (Vector3.Distance(e.Sender.transform.position, controller.transform.position) < communicationGroup.CommunicationRange)
                            {
                                if (controller != e.Sender)
                                {
                                    controller.aICommunicationReceiver.Receive(e);
                                }
                            }

                        }
                    }

                }
                
            }

        }
    }

    public void AddToGroups(AIController controller)
    {
        if (communicationGroups.ContainsKey(controller.communicationGroup))
        {
            communicationGroups[controller.communicationGroup].Add(controller);
        }
        else
        {
            communicationGroups.Add(controller.communicationGroup, new List<AIController>() { controller });
        }
        Subscribe(controller);
    }
    public void RequestChannel(AIChannelRequestEvent e)
    {
        
        if(e.action == ChannelAction.Request)
        {
            if (!e.channel.Requesters.Contains(e.requester))
            {
                e.channel.Requesters.Add(e.requester);
            }

        }
        else
        {
            if (e.channel.Requesters.Contains(e.requester))
            {
                e.channel.Requesters.Remove(e.requester);
            }
        }
        UpdateChannelState(e.channel);
    }

    void UpdateChannelState(CommunicationChannel channel)
    {
        if(channel.Requesters.Count > 0)
        {
            channel.IsActive = true;
        }
        else
        {
            channel.IsActive = false;
        }
    }
    public void Subscribe(AIController controller)
    {
        controller.OnCommunicaitonSend += Publish;
        controller.OnChannelRequest += RequestChannel;
    }
    public void Unsubscribe(AIController controller)
    {
        controller.OnCommunicaitonSend -= Publish;
        controller.OnChannelRequest -= RequestChannel;
    }
}
