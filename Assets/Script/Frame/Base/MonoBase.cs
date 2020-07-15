using UnityEngine;
using System.Collections;

public abstract class MonoBase : MonoBehaviour
{
    public abstract void ProccessEvent(MsgBase tmpMsg);
}
