using UnityEngine;
using System.Collections;
using System;

public class MsgBase
{
    public ushort msgId;

    public ManagerID GetManager()
    {
        int tmpId = msgId / FrameTools.MsgSpan;
        return (ManagerID)(tmpId * FrameTools.MsgSpan);
    }
    public MsgBase(ushort tmpMsg)
    {
        msgId = tmpMsg;
    }
    public MsgBase()
    {

    }

    internal void ChangeEventId(ushort msgId)
    {
        this.msgId = msgId;
    }
}
