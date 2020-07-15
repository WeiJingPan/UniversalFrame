using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ILoaderManager : MonoBehaviour 
{
    public static ILoaderManager Instance;
    void Awake()
    {
        Instance = this;
        //第一步 加载 IABManifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());
    }
    Dictionary<string, IABSceneManager> loadManager = new Dictionary<string, IABSceneManager>();
    //读取配置文件
    public void ReadConfiger(string sceneName)
    {
        if (!loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = new IABSceneManager(sceneName);
            tmpManager.ReadConfiger(sceneName);
            loadManager.Add(sceneName, tmpManager);
        }
    }
    public void LoadCallBack(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            StartCoroutine(tmpManager.LoadAssetSys(bundleName));
        }
        else
        {
            Debug.Log("bundle name is contain ==" + bundleName);
        }
    }
    //提供加载功能
    public void LoadAsset(string sceneName, string bundleName, LoaderProgrecess progress)
    {
        if (!loadManager.ContainsKey(sceneName))
        {
            ReadConfiger(sceneName);
        }
        IABSceneManager tmpManager = loadManager[sceneName];
        tmpManager.LoadAsset(bundleName, progress, LoadCallBack);
    }
    #region 由下层API提供
    public string GetBundleRetateName(string sceneName, string bundleName)
    {
        IABSceneManager tmpManager = loadManager[sceneName];
        if (tmpManager != null)
        {
            return tmpManager.GetBundleRetateName(bundleName);
        }
        return null;
    }
    public Object GetSingleResources(string sceneName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.GetSingleResources(bundleName, resName);
        }
        else
        {
            Debug.Log("sceneName ==" + sceneName + "bundleName ==" + bundleName + "is not load");
            return null;
        }
    }
    public Object[] GetMultiResources(string sceneName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.GetMultiResources(bundleName, resName);
        }
        else
        {
            Debug.Log("sceneName ==" + sceneName + "bundleName ==" + bundleName + "is not load");
            return null;
        }
    }
    //释放资源
    public void UnLoadResObj(string sceneName, string bundleName, string resName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeResObj(bundleName, resName);
        }
        else
        {
            Debug.Log("sceneName ==" + sceneName + "bundleName ==" + bundleName + "is not load");
        }
    }
    //释放整个包
    public void UnloadBundleResObjs(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeBundle(bundleName);
        }
    }
    //释放整个场景的Objects
    public void UnLoadAllResObjs(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllRes();
        }
    }
    //释放一个bundle
    public void UnLoadAssetBundle(string sceneName, string bundleName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeBundle(bundleName);
        }
    }
    //释放一个场景的全部bundle
    public void UnloadAllAssetBundle(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllBundle();
            System.GC.Collect();
        }
    }
    //释放一个场景的全部bundle和Object
    public void UnloadAllAssetBundleAndResObjs(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DisposeAllBundleAndRes();
            System.GC.Collect();
        }
    }
    public void DebugAllAssetBundle(string sceneName)
    {
        if (loadManager.ContainsKey(sceneName))
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            tmpManager.DebugAllAsset();
        }
    }
    #endregion
    public bool IsLoadBundleFinish(string sceneName, string bundle)
    {
        bool tmpBool = loadManager.ContainsKey(sceneName);
        if (tmpBool)
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.IsLoadingFinish(bundle);
        }
        return false;
    }
    public bool IsLoadingAssetBundle(string sceneName, string bundleName)
    {
        bool tmpBool = loadManager.ContainsKey(sceneName);
        if (tmpBool)
        {
            IABSceneManager tmpManager = loadManager[sceneName];
            return tmpManager.IsLoadingAssetBundle(bundleName);
        }
        return false;
    }
    void OnDestory()
    {
        loadManager.Clear();
        System.GC.Collect();
    }
}
