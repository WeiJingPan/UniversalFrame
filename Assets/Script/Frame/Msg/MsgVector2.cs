using UnityEngine;
using System.Collections;

public class MsgVector2:MsgBase
{
    public Vector2 joyStick;
    public MsgVector2(ushort msgid,Vector2 joy)
    {
        this.joyStick = joy;
        this.msgId = msgid;
    }
}
