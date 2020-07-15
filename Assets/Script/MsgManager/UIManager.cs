using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : ManagerBase
{
    public static UIManager m_Instance;
    public static UIManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new UIManager();
            }
            return m_Instance;
        }
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerID.UIManager)
        {
            ProccessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
    public void RegistGameObject(string name, GameObject obj)
    {
        if(!sonMembers.ContainsKey(name)){
            sonMembers.Add(name,obj);
        }
    }
    public void UnRegistGameObject(string name)
    {
        if (!sonMembers.ContainsKey(name))
        {
            sonMembers.Remove(name);
        }
    }
    public GameObject GetGameObject(string name)
    {
        if (sonMembers.ContainsKey(name))
        {
            return sonMembers[name];
        }
        else
            return null;
    }
    private Dictionary<string, GameObject> sonMembers = new Dictionary<string, GameObject>();
}
