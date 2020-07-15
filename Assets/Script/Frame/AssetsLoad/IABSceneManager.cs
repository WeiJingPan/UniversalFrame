using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class IABSceneManager 
{
    private Dictionary<string, string> allAsset;
    IABManager abManager;
    public IABSceneManager(string sceneName)
    {
        string path = Application.persistentDataPath + "/AssetBundle/" + sceneName + "Record.txt";
        abManager = new IABManager(sceneName);
        allAsset = new Dictionary<string, string>();
        ReadConfig(path);
    }
    /// <summary>
    /// 场景名称
    /// </summary>
    /// <param name="fileName"></param>
    public void ReadConfiger(string fileName)
    {
        string textFileName = "Record.txt";
        string path = IPathTools.GetAssetBundlePath() + "/" + fileName + textFileName;
        ReadConfig(path);
    }
    private void ReadConfig(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader br = new StreamReader(fs);
        string line = br.ReadLine();
        int totalCount=int.Parse(line);
        for (int i = 0; i < totalCount; i++)
        {
            string tmpStr = br.ReadLine();
            string[] tmpArr=tmpStr.Split(" ".ToCharArray());

            allAsset.Add(tmpArr[0], tmpArr[1]);
        }
        br.Close();
        fs.Close();
    }
    public void LoadAsset(string bundleName,LoaderProgrecess progress,LoadAssetBundleCallBack callBack)
    {
        //scene/load.ld
        if (allAsset.ContainsKey(bundleName))
        {
            string tmpValue = allAsset[bundleName];
            abManager.LoadAssetBundle(tmpValue, progress, callBack);
        }
        else
        {
            Debug.Log("Dont contain the bundle ==" + bundleName);
        }
    }
    public string GetBundleRetateName(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return allAsset[bundleName];
        }
        return null;
    }

    #region 由下层提供功能


    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return abManager.LoadAssetBundles(bundleName);

    }
    public Object GetSingleResources(string bundleName, string resName)
    {
        if(allAsset.ContainsKey(bundleName))
            return abManager.GetSingleResource(allAsset[bundleName], resName);
        Debug.Log("Dont contain the bundle ==" + bundleName);
        return null;
    }
    public Object[] GetMultiResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
            return abManager.GetMultiResources(allAsset[bundleName], resName);
        Debug.Log("Dont contain the bundle ==" + bundleName);
        return null;
    }
    public void DisposeResObj(string bundleName, string res)
    {
        if (allAsset.ContainsKey(bundleName))
            abManager.DisposeResObj(allAsset[bundleName]);
        Debug.Log("Dont contain the bundle ==" + bundleName);
    }
    public void DisposeAllRes()
    {
        abManager.DisposeAllObj();
    }
    public void DisposeBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
            abManager.DisposeBundle(bundleName);
    }
    public void DisposeAllBundle()
    {
        abManager.DisposeAllBundle();
        allAsset.Clear();
    }
    public void DisposeAllBundleAndRes()
    {
        abManager.DisposeAllBundleAndRes();
        allAsset.Clear();
    }
    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();
        keys.AddRange(allAsset.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            abManager.DebugAssetBundle(allAsset[keys[i]]);
        }
    }
    public bool IsLoadingFinish(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingAssetBundle(allAsset[bundleName]);
        }
        Debug.Log("is not contain bundle ==" + bundleName);
        return false;
    }
    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.IsLoadingAssetBundle(allAsset[bundleName]);
        }
        Debug.Log("is not contain bundle ==" + bundleName);
        return false;
    }
    #endregion
}
