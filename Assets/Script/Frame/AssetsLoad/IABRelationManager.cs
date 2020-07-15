using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABRelationManager
{
    //依赖资源
    List<string> dependenceBundle;
    //引用资源
    List<string> referBundle;

    public IABLoader assetLoader;

    string theBundleName;
    LoaderProgrecess loaderProgrecess;

    public IABRelationManager()
    {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();
    }
    public LoaderProgrecess GetProgrecess()
    {
        return loaderProgrecess;
    }
    //添加引用
    public void AddReference(string bundleName)
    {
        referBundle.Add(bundleName);
    }
    //得到引用
    public List<string> GetReference()
    {
        return referBundle;
    }
    public bool RemoveReference(string bundleName)
    {
        for (int i = 0; i < referBundle.Count; i++)
        {
            if (bundleName.Equals(referBundle[i]))
            {
                referBundle.RemoveAt(i);
            }
        }
        if (referBundle.Count <= 0)
        {
            Dispose();
            return true;
        }
        return false;
    }

    public void SetDependence(string[] dependence)
    {
        if (dependence.Length > 0)
        {
            dependenceBundle.AddRange(dependence);
        }
    }
    public List<string> GetDependence()
    {
        return dependenceBundle;
    }
    public void RemoveDependence(string bundleName)
    {
        for (int i = 0; i < dependenceBundle.Count; i++)
        {
            if (bundleName.Equals(dependenceBundle[i]))
            {
                dependenceBundle.RemoveAt(i);
            }
        }
    }
    bool IsLoadFinish;
    public bool IsAssetBundleLoadFinish()
    {
        return IsLoadFinish;
    }
    public string GetBundleName()
    {
        return theBundleName;
    }

    public void Initial(string bundleName,LoaderProgrecess progrecess)
    {
        IsLoadFinish = false;
        theBundleName = bundleName;
        loaderProgrecess = progrecess;
        assetLoader = new IABLoader(progrecess, BundleLoadFinish);

        assetLoader.SetBundleName(bundleName);
        string bundlePath = IPathTools.GetWWWAssetBundlePath() + "/" + bundleName;
        assetLoader.LoadResources(bundlePath);
    }
    public void BundleLoadFinish(string bundleName)
    {
        IsLoadFinish = true;
    }
    public bool IsBundleLoadFinish()
    {
        return IsLoadFinish;
    }
    #region 有下层提供API
    public void DebuggerAsset()
    {
        if (assetLoader != null)
        {
            assetLoader.DebugerLoader();
        }
        else
        {
            Debug.Log("Asset load is null");
        }
    }
    public IEnumerator LoadAssetBundle()
    {
        yield return assetLoader.CommonLoad();
    }

    public void Dispose()
    {
        assetLoader.Dispose();
    }
    public Object GetSingleResource(string bundleName)
    {
        return assetLoader.GetResource(bundleName);
    }
    public Object[] GetMultiResource(string bundleName)
    {
        return assetLoader.GetResources(bundleName);
    }
    #endregion
}
