using UnityEngine;
using System.Collections;

public enum AssetEvent
{
    HunkRes=ManagerID.AssestMnanger+1,
    ReleaseSingleObj,
    ReleaseBundleObj,
    ReleaseSceneObj,
    ReleaseSingleBundle,
    ReleaseSceneBundle,
    ReleaseAll,
}

//上层需要发给我们的
public class HunkAssetRes:MsgBase
{
    public string sceneName;
    public string bundleName;
    public string resName;
    public bool     isSingle;
    public ushort backMsgId;
    public HunkAssetRes(bool tmpSingle, ushort msgId, string tmpSceneName, string tmpBundle, string tmpRes, ushort tmpBackId)
        : base(msgId)
    {
        this.isSingle = tmpSingle;
        this.sceneName = tmpSceneName;
        this.bundleName = tmpBundle;
        this.resName = tmpRes;
        this.backMsgId = tmpBackId;
    }
}
//下层返回上层的
public class HunkAssetResBack : MsgBase
{
    public Object[] value;
    public HunkAssetResBack()
    {

    }
    public HunkAssetResBack(ushort msgId)
        : base(msgId)
    {
        this.msgId = 0;
        this.value = null;
    }
    public void Changer(ushort msgId, params Object[] tmpValue)
    {
        this.msgId = msgId;
        this.value = tmpValue;
    }
    public void Changer(ushort msgId)
    {
        this.msgId = msgId;
    }
    public void Changer(params Object[] tmpValue)
    {
        this.value = tmpValue;
    }
}
