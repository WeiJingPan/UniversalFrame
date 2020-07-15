using UnityEngine;
using System.Collections;

//每帧回调
public delegate void LoaderProgrecess(string bundle,float progress);
//加载完成时调用
public delegate void LoadFinish(string bundle);

public class IABLoader 
{
    private string bundleName;

    private string commonBundlePath;

    private WWW commonLoader;

    private float commResLoaderProcess;

    private LoaderProgrecess loaderProgress;
    private LoadFinish loadFinish;

    private IABResLoader abResLoader;

    public IABLoader(LoaderProgrecess loaderProgrecess,LoadFinish loadFinish)
    {
        commonBundlePath = "";
        bundleName = "";
        commResLoaderProcess = 0;
        this.loaderProgress = loaderProgrecess;
        this.loadFinish = loadFinish;
        abResLoader = null;
    }
    //设置报名
    //scenes/load
    public void SetBundleName(string bundle)
    {
        this.bundleName = bundle;
    }

    public void LoadResources(string path)
    {
        commonBundlePath = path;
    }

    public IEnumerator CommonLoad()
    {
        commonLoader = new WWW(commonBundlePath);
        while (!commonLoader.isDone)
        {
            commResLoaderProcess = commonLoader.progress;
            if (loaderProgress != null)
            {
                loaderProgress(bundleName, commResLoaderProcess);
            }
            yield return commonLoader.progress;
            commResLoaderProcess = commonLoader.progress;
        }
        if (commResLoaderProcess >= 1.0)//表示已经加载完成
        {
            abResLoader = new IABResLoader(commonLoader.assetBundle);
            if (loaderProgress != null)
            {
                loaderProgress(bundleName, commResLoaderProcess);
            }
            if(loadFinish!=null)
                loadFinish(bundleName);
        }
        else
        {
            Debug.Log("load bundle error ==" + bundleName);
        }
        commonLoader = null;
    }

    #region   下层提供功能
    //Debug
    public void DebugerLoader()
    {
        if (abResLoader != null)
        {
            abResLoader.DebugAllRes();
        }
    }
    //获取单个资源
    public UnityEngine.Object GetResource(string name)
    {
        if (abResLoader != null)
            return abResLoader[name];
        else
            return null;
    }
    //获取多个资源
    public UnityEngine.Object[] GetResources(string name) 
    {
        if (abResLoader != null)
            return abResLoader.LoadResources(name);
        else
            return null;
    }
    //释放功能
    public void Dispose()
    {
        if (abResLoader != null)
        {
            abResLoader.Dispose();
            abResLoader = null;
        }
    }
    //卸载单个资源
    public void UnLoadAssetRes(Object tmpObj)
    {
        if (abResLoader != null)
            abResLoader.UnloadRes(tmpObj);
    }
    #endregion
}
