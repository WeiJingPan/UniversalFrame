using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class MonsterAnimal:CharactorBase
{
    private Animator m_animator;
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)CharactorEvent.eIdle,
            (ushort)CharactorEvent.eRun,
            (ushort)CharactorEvent.eAttack,
            (ushort)CharactorEvent.eAttackBig,
            (ushort)CharactorEvent.eJump,
            (ushort)CharactorEvent.eJoyStick,
            (ushort)CharactorEvent.eDie,
        };
        RegistSelf(this, msgIds);
    }
    public override void ProccessEvent(MsgBase msg)
    {
        switch (msg.msgId)
        {
            case (ushort)CharactorEvent.eIdle:
                m_animator.SetInteger("index", 0);
                Debug.Log("动画进入了待机状态！");
                break;
            case (ushort)CharactorEvent.eRun:
                MsgFloat tmpMsg = (MsgFloat)msg;
                Debug.Log("动画进入了跑的状态！"+tmpMsg.value);
                m_animator.SetFloat("Speed", tmpMsg.value);
                m_animator.SetInteger("index", 3);
                break;
            case (ushort)CharactorEvent.eAttack:
                break;
            case (ushort)CharactorEvent.eAttackBig:
                break;
            case (ushort)CharactorEvent.eJump:
                break;
            case (ushort)CharactorEvent.eJoyStick:
                break;
            case (ushort)CharactorEvent.eDie:
                break;
            default:
                break;
        }
    }
}
