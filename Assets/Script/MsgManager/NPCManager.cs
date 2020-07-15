using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : ManagerBase
{

    public static NPCManager m_Instance = null;

    public static NPCManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new NPCManager();
            }
            return m_Instance;
        }
    }
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerID.NPCManager)
        {
            ProccessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}
