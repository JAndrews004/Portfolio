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
            newChannel.channelData = channel;
            newChannel.IsActive = false;
            newChannel.Requesters = new List<IDetectableEntity> { };
            communicationChannels.Add(newChannel);
        }
    }
    public void Publish(AICommunicationEvent e)
    {
        Debug.Log("Communication sent");
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
                //Debug.Log("Checking channel");
                if (channel.IsActive)
                {
                    //Debug.Log("Channel active");
                    CommunicationGroup communicationGroup;
                    if(channel.channelData.groupB == e.Sender.communicationGroup)
                    {
                        communicationGroup = channel.channelData.groupA;
                    }
                    else if (channel.channelData.groupA == e.Sender.communicationGroup)
                    {
                        communicationGroup = channel.channelData.groupB;
                    }
                    else
                    {
                        return;
                    }
                    //Debug.Log("Communication group selected");
                    if (communicationGroups.ContainsKey(communicationGroup))
                    {
                        foreach (AIController controller in communicationGroups[communicationGroup])
                        {
                            if (Vector3.Distance(e.Sender.transform.position, controller.transform.position) < communicationGroup.CommunicationRange)
                            {
                                if (controller != e.Sender)
                                {
                                    controller.aICommunicationReceiver.Receive(e);
                                    Debug.Log("Emergency communication sent");
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
        CommunicationChannel channelToActivate = null;
        foreach(CommunicationChannel channel in communicationChannels)
        {
            if(channel.channelData == e.channel.channelData)
            {
                channelToActivate  = channel;
                //Debug.Log($"Channel found with {channelToActivate.Requesters.Count} requesters");
                break;
            }
        }
        if (channelToActivate == null)
            return;

        if(e.action == ChannelAction.Request)
        {
            //Debug.Log($"Channel requested");
            if (!channelToActivate.Requesters.Contains(e.requester))
            {
                channelToActivate.Requesters.Add(e.requester);
                //Debug.Log($"Channel requesters {channelToActivate.Requesters.Count}");
            }

        }
        else if (e.action == ChannelAction.Release)
        {
            //Debug.Log($"Channel released");
            if (channelToActivate.Requesters.Contains(e.requester))
            {
                channelToActivate.Requesters.Remove(e.requester);
            }
        }
        UpdateChannelState(channelToActivate);
    }

    void UpdateChannelState(CommunicationChannel channel)
    {
        //Debug.Log($"Channel requesters {channel.Requesters.Count}");
        if (channel.Requesters.Count > 0)
        {
            channel.IsActive = true;
            //Debug.Log("Channel activated");
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
