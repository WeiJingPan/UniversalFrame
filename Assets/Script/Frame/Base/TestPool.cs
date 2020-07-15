using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    ObjectPool<MsgBase> msgPool = new ObjectPool<MsgBase>(null, null);
    ObjectPool<UIMsgEvent> uiMsgPool = new ObjectPool<UIMsgEvent>(null,null);

	// Use this for initialization
	void Start ()
    {
        MsgBase msg = msgPool.Get();

        msgPool.Release(msg);
	}
}
