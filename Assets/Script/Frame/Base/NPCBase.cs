using UnityEngine;
using System.Collections;

public class NPCBase : MonoBase 
{

    public void RegistSelf(MonoBase mono, params ushort[] msgs)
    {
        NPCManager.Instance.RegistMsgs(mono, msgs);
    }
    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        NPCManager.Instance.RegistMsgs(mono, msgs);
    }
    public void SendMsg(MsgBase msg)
    {
        NPCManager.Instance.SendMsg(msg);
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
