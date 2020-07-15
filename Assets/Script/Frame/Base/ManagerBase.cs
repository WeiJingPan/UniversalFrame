using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventNode
{
    //当前数据
    public MonoBase data;
    //下一节点
    public EventNode next;

    public EventNode(MonoBase tmpMono)
    {
        this.data = tmpMono;
        this.next = null;
    }
}
public class ManagerBase : MonoBase 
{
    //存储注册的消息
    private Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

    public void RegistMsgs(MonoBase mono, params ushort[] msgs)
    {
        RegistGameObject(mono.name, mono.gameObject);
        EventNode tmpEvent;
        foreach (var id in msgs)
        {
            tmpEvent = new EventNode(mono);
            RegistMsg(id, tmpEvent);
        }
    }
    public void RegistMsg(ushort id, EventNode node)
    {
        if (!eventTree.ContainsKey(id))
        {
            eventTree.Add(id,node);
        }
        else
        {
            EventNode tmpNode = eventTree[id];
            while (tmpNode.next != null)
            {
                tmpNode = tmpNode.next;
            }
            tmpNode.next = node;
        }
    }
    public void UnRegistMsgs(MonoBase mono,params ushort[] msgs)
    {
        foreach (var id in msgs)
        {
            UnRegistMsg(id, mono);
        }
    }
    public void UnRegistMsg(ushort id, MonoBase node)
    {
        if (eventTree.ContainsKey(id))
        {
             EventNode tmpNode = eventTree[id];
             if (tmpNode.data == node)
            {
                if (tmpNode.next == null)
                    eventTree.Remove(id);
                else
                {
                    tmpNode = tmpNode.next;
                }
            }
            else
            {
                while (tmpNode.next.data != node)
                {
                    tmpNode = tmpNode.next;
                }
                tmpNode.next = tmpNode.next.next;
            }
            UnRegistGameObject(node.name);
        }
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        if (!eventTree.ContainsKey(tmpMsg.msgId))
        {
            Debug.LogError("msg not contain msgid==" + tmpMsg.msgId);
            Debug.LogError("Msg Manager" + tmpMsg.GetManager());
            return;
        }
        else
        {
            if (eventTree.Count > 0)
            {
                EventNode tmp = eventTree[tmpMsg.msgId];
                do
                {
                    tmp.data.ProccessEvent(tmpMsg);
                    tmp = tmp.next;
                }
                while (tmp != null);
            }
        }
    }
    public void RegistGameObject(string name, GameObject obj)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Add(name, obj);
        }
    }
    public void UnRegistGameObject(string name)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Remove(name);
        }
    }
    public GameObject GetGameObject(string name)
    {
        if (sonMembers.ContainsKey(name))
        {
            return sonMembers[name];
        }
        else
            return null;
    }
    private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();
}
