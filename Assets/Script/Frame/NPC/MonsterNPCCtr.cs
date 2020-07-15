using UnityEngine;
using System.Collections;

public enum NPCMonsterEvent
{
    eAnimalIdle=ManagerID.NPCManager+1,
    eAnimalRun,
    eAnimalDie,
    eAnimalAttack,
}

public class MonsterNPCCtr: NPCBase
{

    public enum NPCState
    {
        eRun,
        eAttack,
        eDie,
        eIdle,
    }

    UnityEngine.AI.NavMeshAgent agent;
    Transform m_target;
    NPCState npcState;
    float timeCount;
    MsgBase npcAnimalMsg;
    float deltaDist;

    void Awake()
    {
        msgIds = new ushort[]
        {
        };
        RegistSelf(this, msgIds);
    }
    void Start()
    {
        agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_target = GameObject.FindWithTag("Player").transform;
        npcAnimalMsg = new MsgBase((ushort)NPCMonsterEvent.eAnimalIdle);
        timeCount = 0;
    }
    void Update()
    {
        if (npcState == NPCState.eAttack)
        {

        }else if (npcState == NPCState.eRun)
        {
            MoveToNPC();
        }else if (npcState == NPCState.eIdle)
        {

        }
        else if (npcState == NPCState.eDie)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 2)
            {
                timeCount = 0;
                npcState = NPCState.eIdle;
            }
        }
    }
    public void MoveToNPC()
    {
        Vector3 tmpDist = (m_target.position - transform.position);
        if (tmpDist.magnitude < 2)
        {
            if (npcState == NPCState.eRun)
            {
                timeCount = 0;
                npcState = NPCState.eAttack;
                npcAnimalMsg.ChangeEventId((ushort)NPCMonsterEvent.eAnimalAttack);
                SendMsg(npcAnimalMsg);
            }
        }
        else
        {
            if (npcState == NPCState.eAttack)
            {
                timeCount = 0;
                npcState = NPCState.eRun;
                npcAnimalMsg.ChangeEventId((ushort)NPCMonsterEvent.eAnimalRun);
                SendMsg(npcAnimalMsg);
            }
        }
        Debug.DrawLine(transform.position, m_target.position, Color.cyan);
        Vector3 tmpLoc = tmpDist - tmpDist.normalized * deltaDist;
        agent.destination = tmpLoc + transform.position;
        Debug.DrawLine(transform.position, agent.destination, Color.red);
        transform.LookAt(m_target.position);
    }
    public override void ProccessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            default:
                break;
        }
    }
}
