using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderPattern : MonoBehaviour
{
    	
}
public class AssetBundleLoad
{
    Dictionary<string, AssetBundleUnit> m_dicAssetBundle = new Dictionary<string, AssetBundleUnit>();
    public string m_assetBundleName;
    public void SetAssetBundleName(string name)
    {
        m_assetBundleName = name;
    }
    public IEnumerator LoadAssetBundle(string path)
    {
        WWW tmpLoader = new WWW(path);
        while (!tmpLoader.isDone)
        {
            yield return tmpLoader;
        }
        AssetBundleUnit unit = new AssetBundleUnit();
        unit.m_assetBunble = tmpLoader.assetBundle;
        m_dicAssetBundle.Add(m_assetBundleName, unit);
    }
    public Object LoadSingleRes(string bundleName, string res)
    {
        if (m_dicAssetBundle.ContainsKey(bundleName))
        {
            AssetBundleUnit tmpAB = m_dicAssetBundle[bundleName];
            return tmpAB.LoadRes(res);
        }
        return null;
    }
    public Object[] LoadAllRes(string bundleName, string res)
    {
        if (m_dicAssetBundle.ContainsKey(bundleName))
        {
            AssetBundleUnit tmpAB = m_dicAssetBundle[bundleName];
            return tmpAB.LoadAllRes(res);
        }
        return null;
    }
}
public class AssetBundleUnit
{
    public AssetBundle m_assetBunble;

    public Object LoadRes(string res_name)
    {
        return m_assetBunble.LoadAsset(res_name);
    }
    public Object[] LoadAllRes(string res_name)
    {
        return m_assetBunble.LoadAssetWithSubAssets(res_name);
    }
}
