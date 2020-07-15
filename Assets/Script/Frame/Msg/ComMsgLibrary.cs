using UnityEngine;
using System.Collections;

public class MsgFloat : MsgBase
{
    public float value;
    public MsgFloat(ushort msgid, float value)
    {
        this.value = value;
        this.msgId = msgid;
    }
}
