using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void NativeResCallBack(NativeResCallBackNode tmpNode);

public class NativeResCallBackNode
{
    public string sceneName;
    public string bundleName;
    public string resName;
    public bool   isSingle;
    public ushort backMsgId;

    public NativeResCallBack callBack;
    public NativeResCallBackNode nextNode;
    public NativeResCallBackNode(bool tmpSingle, string tmpSceneName, string tmpBundle, string tmpRes, ushort tmpBackId,NativeResCallBack callBack,NativeResCallBackNode tmpNode)
    {
        this.isSingle = tmpSingle;
        this.sceneName = tmpSceneName;
        this.bundleName = tmpBundle;
        this.resName = tmpRes;
        this.backMsgId = tmpBackId;
        this.callBack = callBack;
        this.nextNode = tmpNode;
    }
    public void Dispose()
    {
        sceneName = null;
        bundleName = null;
        resName=null;
        callBack = null;
        nextNode = null;
    }
}

public class NativeResCallBackManager
{
    Dictionary<string, NativeResCallBackNode> manager = null;
    public NativeResCallBackManager()
    {
        manager = new Dictionary<string, NativeResCallBackNode>();
    }
    //来了请求添加
    public void AddBundle(string bundle, NativeResCallBackNode curNode)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResCallBackNode tmpNode = manager[bundle];
            while (tmpNode.nextNode != null)
            {
                tmpNode = tmpNode.nextNode;
            }
            tmpNode.nextNode = curNode;
        }
        else
        {
            manager.Add(bundle, curNode);
        }
    }
    //加载完成后，消息向上层传递完成了，就把命令
    public void Dispose(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResCallBackNode tmpNode = manager[bundle];
            while (tmpNode.nextNode != null)
            {
                NativeResCallBackNode curNode = tmpNode;
                tmpNode = tmpNode.nextNode;
                curNode.Dispose();
                manager.Remove(bundle);
            }
        }
    }

    internal void AddBundleCallBack(string bundleFullName, TeacherCallBackNode tmpNode)
    {
        throw new NotImplementedException();
    }

    public void CallBackRes(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            NativeResCallBackNode tmpNode = manager[bundle];
            do
            {
                tmpNode.callBack(tmpNode);
                tmpNode = tmpNode.nextNode;
            }
            while (tmpNode != null);
        }
    }
}

public class NativeResLoader : AssetBase 
{
    HunkAssetResBack resBackMsg = null;
    HunkAssetResBack ReleaseBack
    {
        get
        {
            if (resBackMsg == null)
            {
                resBackMsg = new HunkAssetResBack();
            }
            return resBackMsg;
        }
    }
    NativeResCallBackManager callBack = null;
    NativeResCallBackManager CallBack
    {
        get
        {
            if (callBack == null)
            {
                callBack = new NativeResCallBackManager();
            }
            return callBack;
        }
    }
    void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)AssetEvent.ReleaseSingleObj,
            (ushort)AssetEvent.ReleaseBundleObj,
            (ushort)AssetEvent.ReleaseSceneObj,
            (ushort)AssetEvent.ReleaseSingleBundle,
            (ushort)AssetEvent.ReleaseSceneBundle,
            (ushort)AssetEvent.ReleaseAll,
            (ushort)AssetEvent.HunkRes,

        };
        RegistSelf(this,msgIds);
    }
    public override void ProccessEvent(MsgBase recMsg)
    {
        HunkAssetRes tmpMsg = (HunkAssetRes)recMsg;
        switch (recMsg.msgId)
        {
            case (ushort)AssetEvent.ReleaseSingleObj:
                ILoaderManager.Instance.UnLoadResObj(tmpMsg.sceneName, tmpMsg.bundleName,tmpMsg.resName);
                break;
            case (ushort)AssetEvent.ReleaseBundleObj:
                ILoaderManager.Instance.UnloadBundleResObjs(tmpMsg.sceneName, tmpMsg.bundleName);
                break;
            case (ushort)AssetEvent.ReleaseSceneObj:
                ILoaderManager.Instance.UnLoadAllResObjs(tmpMsg.sceneName);
                break;
            case (ushort)AssetEvent.ReleaseSingleBundle:
                ILoaderManager.Instance.UnLoadAssetBundle(tmpMsg.sceneName,tmpMsg.bundleName);
                break;
            case (ushort)AssetEvent.ReleaseSceneBundle:
                ILoaderManager.Instance.UnloadAllAssetBundle(tmpMsg.sceneName);
                break;
            case (ushort)AssetEvent.ReleaseAll:
                ILoaderManager.Instance.UnloadAllAssetBundleAndResObjs(tmpMsg.sceneName);
                break;
                //请求资源
            case (ushort)AssetEvent.HunkRes:
                tmpMsg = (HunkAssetRes)recMsg;
                GetResource(tmpMsg.sceneName, tmpMsg.bundleName, tmpMsg.resName, tmpMsg.isSingle, tmpMsg.backMsgId);
                break;
        }
    }
    public void SendToBackMsg(NativeResCallBackNode tmpNode)
    {
        if (tmpNode.isSingle)
        {
            Debug.Log("tmpNode === finish");
            UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);

            Debug.Log("tmpNode 1111 === finish ==" + tmpObj.name);
            //this.ReleaseBack.Changer(tmpNode.backMsgId, tmpObj);
            resBackMsg.Changer(tmpNode.backMsgId, tmpObj);

            Debug.Log("tmpNode 2222 === finish");
            SendMsg(resBackMsg);
        }
        else
        {
            UnityEngine.Object[] tmpObj = ILoaderManager.Instance.GetMultiResources(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
            this.resBackMsg.Changer(tmpNode.backMsgId, tmpObj);
            SendMsg(resBackMsg);
        }
    }

    //sceneOne/load.ld
    void LoaderProgrecess(string bundleName, float progress)
    {
        if (progress >= 1.0f)
        {
            //上层的回调
            CallBack.CallBackRes(bundleName);
            CallBack.Dispose(bundleName);
        }
    }
    public void GetResource(string sceneName, string bundleName, string res, bool single, ushort backId)
    {
        if (!ILoaderManager.Instance.IsLoadBundleFinish(sceneName, bundleName))
        {
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, LoaderProgrecess);
            string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(single, sceneName, bundleName, res, backId, SendToBackMsg, null);
                CallBack.AddBundle(bundleFullName, tmpNode);
                Debug.Log("GetRescources ==" + bundleFullName);
            }
            else
            {
                Debug.LogWarning("Do not contain bundle ==" + bundleName);
            }
        }
            //表示已经加载完成
        else if (ILoaderManager.Instance.IsLoadBundleFinish(sceneName, bundleName))
        {
            if (single)
            {
                UnityEngine.Object tmpObj = ILoaderManager.Instance.GetSingleResources(sceneName, bundleName, res);
                this.ReleaseBack.Changer(backId, tmpObj);
                SendMsg(ReleaseBack);
            }
            else
            {
                UnityEngine.Object[] tmpObj = ILoaderManager.Instance.GetMultiResources(sceneName, bundleName, res);
                this.ReleaseBack.Changer(backId, tmpObj);
                SendMsg(ReleaseBack);
            }
        }
        else
        {
            //已经加载但是没有完成
            string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResCallBackNode tmpNode = new NativeResCallBackNode(single, sceneName, bundleName, res, backId, SendToBackMsg, null);
                CallBack.AddBundle(bundleName, tmpNode);
            }
            else
                Debug.LogWarning("Do not contain bundle ==" + bundleName);
        }
    }
    private void SendMsg(HunkAssetResBack ReleaseBack)
    {
        
    }
}
