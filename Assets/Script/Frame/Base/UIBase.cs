using UnityEngine;
using System.Collections;

public class UIBase : MonoBase 
{
    public void RegistSelf(MonoBase mono, params ushort[] msgs)
    {
        UIManager.Instance.RegistMsgs(mono, msgs);
    }
    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        UIManager.Instance.UnRegistMsgs(mono, msgs);
    }
    public void SendMsg(MsgBase msg)
    {
        UIManager.Instance.SendMsg(msg);
    }
    void OnDestory()
    {
        if (msgIds != null)
        {
            UnRegistSelf(this, msgIds);
        }
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        
    }
    public ushort[] msgIds;
}
