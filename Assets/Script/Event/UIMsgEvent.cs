using UnityEngine;
using System.Collections;

public class UIMsgEvent : MonoBehaviour 
{
    public enum LoadEvent
    {
        LoagOne=ManagerID.UIManager+1,
        LoadTwo,
        MaxValue,
    }
    public enum RegistEvent
    {
        RegistOne=LoadEvent.MaxValue+1,
        RegistTwo,
        MaxValue,
    }
    public enum PackEvent
    {
        PackOne=RegistEvent.MaxValue+1,
        PackTwo,
        MaxValue,
    }
}
