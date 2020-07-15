using UnityEngine;
using System.Collections;

public class UIEventMsg : MonoBehaviour 
{
}
public enum EUIEvent
{
    Load=ManagerID.UIManager,
    Regist,
    MaxValue,
}
public enum ENPCEvent
{
    load=ManagerID.NPCManager,
    Regist,
    MaxValue,
}
