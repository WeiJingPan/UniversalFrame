using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
public class MonsterNPCAnimal: NPCBase
{
    private Animator m_animator;
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)NPCMonsterEvent.eAnimalIdle,
            (ushort)NPCMonsterEvent.eAnimalRun,
            (ushort)NPCMonsterEvent.eAnimalDie,
            (ushort)NPCMonsterEvent.eAnimalAttack,
        };
        RegistSelf(this, msgIds);
    }
    void Start()
    {
        m_animator = transform.GetComponent<Animator>();
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)NPCMonsterEvent.eAnimalIdle:
                m_animator.SetInteger("index", 0);
                break;
            case (ushort)NPCMonsterEvent.eAnimalRun:
                break;
            case (ushort)NPCMonsterEvent.eAnimalDie:
                break;
            case (ushort)NPCMonsterEvent.eAnimalAttack:
                break;
            default:
                break;
        }
    }
}
