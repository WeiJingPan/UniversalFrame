using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class AssetBase:MonoBase
{
    public void RegistSelf(MonoBase mono, params ushort[] msgs)
    {

        AssetManager.Instance.RegistMsgs(mono, msgs);
    }
    public void UnRegistSelf(MonoBase mono, params ushort[] msgs)
    {
        AssetManager.Instance.UnRegistMsgs(mono, msgs);
    }
    public void SendMsg(MsgBase msg)
    {
        AssetManager.Instance.SendMsg(msg);
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
