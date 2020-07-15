using UnityEngine;
using System.Collections;

public class IABManifestLoader 
{
    public AssetBundleManifest assetManifest;
    public string manifestPath;
    private bool isLoadFinish;
    public AssetBundle manifestLoader;

    public IABManifestLoader()
    {
        assetManifest = null;
        manifestLoader = null;
        isLoadFinish = false;
        manifestPath =IPathTools.GetWWWAssetBundlePath()+"/"+ IPathTools.GetPlatformFolderName(Application.platform);
    }
    public bool IsLoadFinish()
    {
        return isLoadFinish;
    }
    public IEnumerator LoadManifest()
    {
        Debug.Log("manifestPath ==" + manifestPath);
        WWW manifest = new WWW(manifestPath);
        yield return manifest;
        if (!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        }
        else
        {
            if (manifest.progress>=1.0f)
            {
                manifestLoader = manifest.assetBundle;
                assetManifest = manifestLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                isLoadFinish = true;
                Debug.Log("manifest load finish");
            }
        }
    }
    public string[] GetDependence(string name)
    {
        return assetManifest.GetAllDependencies(name);
    }
    public void UnLoadManifest()
    {
        manifestLoader.Unload(true);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    public void SetManifestPath(string path)
    {
        manifestPath = path;
    }
    private static IABManifestLoader instance = null;
    public static IABManifestLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IABManifestLoader();
            }
            return instance;
        }
    }

}
