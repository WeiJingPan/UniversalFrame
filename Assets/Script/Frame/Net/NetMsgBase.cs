using System;
using UnityEngine;
using System.Collections;

public class NetMsgBase:MsgBase
{
    public byte[] buffer;

    public NetMsgBase() { }
    public NetMsgBase(byte[] arr)
    {
        buffer = arr;
        this.msgId = BitConverter.ToUInt16(arr, 4);
    }

    public virtual byte[] GetNetBytes()
    {
        return buffer;
    }
}
