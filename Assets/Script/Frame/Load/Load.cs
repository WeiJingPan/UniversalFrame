using UnityEngine;
using System.Collections;

public class Load : UIBase
{
    void Awake()
    {
        msgIds=new ushort[]
        {
            (ushort)EUIEvent.Load,
            (ushort)EUIEvent.Regist,
        };
        RegistSelf(this, msgIds);
        UIManager.Instance.GetGameObject("lighton").GetComponent<UIBehaviour>().AddButtonListener(ButtonClick);
    }
    public void ButtonClick()
    {
        MsgBase tmpBase = new MsgBase((ushort)EUIEvent.Load);
        SendMsg(tmpBase);
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)EUIEvent.Load:
                break;
            case (ushort)EUIEvent.Regist:
                break;
            default:
                break;
        }
    }
}
