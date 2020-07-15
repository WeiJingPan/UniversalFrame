using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void LoadAssetBundleCallBack(string sceneName,string bundleName);

//存储单个的Object
public class AssetObj
{
    public List<Object> objs;
    public AssetObj(params Object[] tmpObj)
    {
        objs = new List<Object>();
        objs.AddRange(tmpObj);
    }
    public void ReleaseObj()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Resources.UnloadAsset(objs[i]);
        }
    }
}
//存的一个bundle包Object
public class AssetResObj
{
    public Dictionary<string, AssetObj> resObjs;
    public AssetResObj(string name, AssetObj tmp)
    {
        resObjs = new Dictionary<string, AssetObj>();
        resObjs.Add(name, tmp);
    }
    public void AddResObj(string name, AssetObj tmpObj)
    {
        resObjs.Add(name, tmpObj);
    }
    public void ReleaseAllResObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(resObjs.Keys);
        foreach (var key in resObjs.Keys)
        {
            ReleaseResObj(key);
        }
    }
    public void ReleaseResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            tmpObj.ReleaseObj();
        }
        else
        {
            Debug.Log("release object name is not exit==" + name);
        }
    }
    public List<Object> GetResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            return tmpObj.objs;
        }
        else
        {
            Debug.Log("release object name is not exit==" + name);
            return null;
        }
    }
}


/// <summary>
/// 对一个场景的所有bundle包的管理
/// </summary>
public class IABManager
{
    //把每个包都存起来
    Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();
    Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();

    string sceneName;
    public IABManager(string tmpSceneName)
    {
        sceneName = tmpSceneName;
    }
    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (!loadHelper.ContainsKey(bundleName))
            return false;
        else
            return true;
    }

    #region MyRegion
    //表示已经加载过bundle
    public Object GetSingleResource(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj[0];
            }
        }
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object tmpObj = loader.GetSingleResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            //缓存里面是否已经有了这个包
            if (loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else
        {
            return null;
        }

    }
    public Object[] GetMultiResources(string bundleName, string resName)
    {
        //表示是否已经缓存了物体
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<Object> tmpObj = tmpRes.GetResObj(resName);
            if (tmpObj != null)
            {
                return tmpObj.ToArray();
            }
        }
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            Object[] tmpObj = loader.GetMultiResource(resName);
            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            //缓存里面是否已经有了这个包
            if (loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else
        {
            return null;
        }

    }

    public void DisposeResObj(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseResObj(resName);
        }
    }
    //释放缓存物体
    public void DisposeResObj(string bundleName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseAllResObj();
        }
        Resources.UnloadUnusedAssets();
    }
    //释放多个缓存物体
    public void DisposeAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadObj.Keys);
        for (int i = 0; i < loadObj.Count; i++)
        {
            DisposeResObj(keys[i]);
        }
        loadObj.Clear();
    }
    public void DisposeBundle(string bundleName)
    {
        if(loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader=loadHelper[bundleName];
            List<string> depences = loader.GetDependence();
            for (int i = 0; i < depences.Count; i++)
            {
                if (loadHelper.ContainsKey(depences[i]))
                {
                    IABRelationManager dependence = loadHelper[depences[i]];
                    if (dependence.RemoveReference(bundleName))
                        DisposeBundle(dependence.GetBundleName());
                }
            }
            if (loader.GetReference().Count <= 0)
            {
                loader.Dispose();
                loadHelper.Remove(bundleName);
            }
        }
    }
    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);
        for (int i = 0; i < loadHelper.Count; i++)
        {
            IABRelationManager loader = loadHelper[keys[i]];
            loader.Dispose();
        }
        loadHelper.Clear();
    }
    public void DisposeAllBundleAndRes()
    {
        DisposeAllObj();
        DisposeAllBundle();
    }
    #endregion

    string[] GetDependence(string bundleName)
    {
        return IABManifestLoader.Instance.GetDependence(bundleName);
    }
    //对外的接口
    public void LoadAssetBundle(string bundleName, LoaderProgrecess progreces, LoadAssetBundleCallBack callBack)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progreces);
            loadHelper.Add(bundleName,loader);
            callBack(sceneName, bundleName);
        }
        else
        {
            Debug.Log("IABManager have contain bundle name==" + bundleName);
        }
    }
    public IEnumerator LoadAssetBundleDependences(string bundleName, string refName, LoaderProgrecess progreces)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            loader.Initial(bundleName, progreces);
            if (refName != null)
            {
                loader.AddReference(refName);
            }
            loadHelper.Add(bundleName, loader);
            yield return LoadAssetBundles(bundleName);
        }
        else
        {
            if (refName != null)
            {
                IABRelationManager loader=loadHelper[bundleName];
                loader.AddReference(bundleName);
            }
        }
    }
    /// <summary>
    /// 加载 assetbundle 必须先加载manifest
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundles(string bundleName)
    {
        while (!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager loader = loadHelper[bundleName];
        string[] dependences = GetDependence(bundleName);
        loader.SetDependence(dependences);
        for (int i = 0; i < dependences.Length; i++)
        {
            yield return LoadAssetBundleDependences(dependences[i],bundleName,loader.GetProgrecess());
        }
        yield return loader.LoadAssetBundle();
    }

    #region 有下层提供的API
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    public void DebugAssetBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            loader.DebuggerAsset();
        }
    }
    public bool IsLoadingFinish(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            return loader.IsBundleLoadFinish();
        }
        else
        {
            Debug.Log("IABRelation no contain bundle==" + bundleName);
            return false;
        }
    }
    
    #endregion
}
