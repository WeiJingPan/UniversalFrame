using UnityEngine;
using System.Collections;

public class CharactorBase : MonoBase 
{

    public void RegistSelf(MonoBase mono, params ushort[] msgs)
    {
        CharactorMnanger.Instance.RegistMsgs(mono, msgs);
    }
    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        CharactorMnanger.Instance.UnRegistMsgs(mono, msgs);
    }
    public void SendMsg(MsgBase msg)
    {
        CharactorMnanger.Instance.SendMsg(msg);
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
