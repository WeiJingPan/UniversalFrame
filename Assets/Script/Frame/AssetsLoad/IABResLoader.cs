using UnityEngine;
using System.Collections;
using System;

public class IABResLoader:IDisposable
{
    private AssetBundle ABRes;

    public IABResLoader(AssetBundle tmpBundle)
    {
        ABRes = tmpBundle;
    }
    public UnityEngine.Object this[string resName]
    {
        get
        {
            if (this.ABRes == null || !this.ABRes.Contains(resName))
            {
                Debug.Log("res not contain");
                return null;
            }
            return ABRes.LoadAsset(resName);
        }
    }
    public UnityEngine.Object[] LoadResources(string resName)
    {
        if (this.ABRes == null || !this.ABRes.Contains(resName))
        {
            Debug.Log("res not contain");
            return null;
        }
        return ABRes.LoadAssetWithSubAssets(resName);
    }
    public void UnloadRes(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }
    public void Dispose()
    {
        if (ABRes==null)
        {
            return;
        }
        ABRes.Unload(false);
    }
    public void DebugAllRes()
    {
        string[] tmpAssetName = ABRes.GetAllAssetNames();
        for (int i = 0; i < tmpAssetName.Length; i++)
        {
            Debug.Log("ABRes contains ：" + tmpAssetName[i]);
        }
    }
}
