using UnityEngine;
using System.Collections;

public class EventCSClass:UIBase
{
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)EUIEvent.Load,
        };
        RegistSelf(this, msgIds);
    }
    public void ButtonClick()
    {
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)EUIEvent.Load:
                break;
            default:
                break;
        }
    }
}
