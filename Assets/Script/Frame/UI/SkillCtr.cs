using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public enum BloodEvent
{
    eLooseBlood=ManagerID.UIManager+1,

}

public class SkillCtr:UIBase
{
    MsgBase skillMsg;
    SkillTime skillOneSkillTime;
    BloodCtr bloodCtr;

    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)BloodEvent.eLooseBlood,
        };
        RegistSelf(this, msgIds);
    }
    private void Start()
    {
        skillMsg = new MsgBase((ushort)CharactorEvent.eAttackBig);   
        GameObject skillOne = UIManager.Instance.GetGameObject("skillone");
        skillOne.GetComponent<UIBehaviour>().AddButtonDownListener(SkillOneButtonClick);
        skillOne.GetComponent<UIBehaviour>().AddButtonUpListener(SkillOneButtonClickUp);
        skillOneSkillTime = skillOne.GetComponent<SkillTime>();

        bloodCtr = UIManager.Instance.GetGameObject("hp").GetComponent<BloodCtr>();
    }

    private void SkillOneButtonClick(BaseEventData tmpEvent)
    {
        skillMsg.ChangeEventId((ushort)CharactorEvent.eAttackBig);
        SendMsg(skillMsg);
    }
    private void SkillOneButtonClickUp(BaseEventData tmpEvent)
    {
        skillMsg.ChangeEventId((ushort)CharactorEvent.eIdle);
        SendMsg(skillMsg);
    }

    public void ButtonClick()
    {
    }
    public override void ProccessEvent(MsgBase Msg)
    {
        switch (Msg.msgId)
        {
            case (ushort)BloodEvent.eLooseBlood:
                MsgFloat tmpMsg = (MsgFloat)Msg;
                bloodCtr.SetBlood(tmpMsg.value);
                break;
            default:
                break;
        }
    }
}
