using UnityEngine;
using System.Collections;

public class MsgCenter : MonoBehaviour
{
    public static MsgCenter m_Instance = null;
    public static MsgCenter Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new MsgCenter();
            }
            return m_Instance;
        }
    }
    private void Awake()
    {
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<AssetManager>();
        gameObject.AddComponent<CharactorMnanger>();
        gameObject.AddComponent<NPCManager>();
        gameObject.AddComponent<ILoaderManager>();
        gameObject.AddComponent<AssetManager>();
        //gameObject.AddComponent<GameManager>();
    }
    public void SendToMsg(MsgBase tmpMsg)
    {
        AnalysisMsg(tmpMsg);
    }
    private void AnalysisMsg(MsgBase tmpMsg)
    {
        ManagerID tmpId = tmpMsg.GetManager();
        switch (tmpId)
        {
            case ManagerID.AssestMnanger:
                AssetManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerID.AudioManager:
                break;
            case ManagerID.CharactorMnanger:
                CharactorMnanger.Instance.SendMsg(tmpMsg);
                break;
            case ManagerID.GameManager:
                break;
            case ManagerID.NetManager:
                
                break;
            case ManagerID.NPCManager:
                NPCManager.Instance.SendMsg(tmpMsg);
                break;
            case ManagerID.UIManager:
                UIManager.Instance.SendMsg(tmpMsg);
                break;
            default:
                break;
        }
    }
}
