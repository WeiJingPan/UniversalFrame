using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetManager : ManagerBase
{
    public static AssetManager m_Instance;
    public static AssetManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new AssetManager();
            }
            return m_Instance;
        }
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerID.AssestMnanger)
        {
            ProccessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}
