using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactorMnanger : ManagerBase
{

    public static CharactorMnanger m_Instance = null;

    public static CharactorMnanger Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new CharactorMnanger();
            }
            return m_Instance;
        }
    }
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerID.CharactorMnanger)
        {
            ProccessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}
