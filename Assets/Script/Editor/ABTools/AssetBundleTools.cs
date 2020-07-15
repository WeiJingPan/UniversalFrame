using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetBundleTools 
{
    [MenuItem("Itools/BuildAssetBundle")]
    public static void BuilderAssetBundle()
    {
        //streamingAssetPath/Window
        //string outPath = Application.streamingAssetsPath + "/AssetBundle";
        string outPath = IPathTools.GetAssetBundlePath();
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
    [MenuItem("Itools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //E:/UnityApplication/UniversalFrame/Assets/AB/Scenes
        string path = Application.streamingAssetsPath + "/AB/Scenes";
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_red) + path);
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileInfoes = dir.GetFileSystemInfos();
        //E:\UnityApplication\UniversalFrame\Assets\AB\Scenes\Load
        for (int i = 0; i < fileInfoes.Length; i++)
        {
            FileSystemInfo tmpFile = fileInfoes[i];
            if (tmpFile is DirectoryInfo)
            {
                string tmpPath = Path.Combine(path, tmpFile.Name);
                ScenesOverView(tmpPath);
            }
        }
        AssetDatabase.Refresh();
    }

    private static void ScenesOverView(string tmpPath)
    {
        string textFileName = "Record.txt";
        //E:/UnityApplication/UniversalFrame/Assets/AB/Scenes\LoadRecord.txt
        string txtPath = tmpPath + textFileName;
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_red) + txtPath);
        FileStream fs = new FileStream(txtPath, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
        Dictionary<string, string> readDict = new Dictionary<string, string>();
        ChangeHead(tmpPath, readDict);
        //第一行是总行数
        sw.Write(readDict.Count);
        foreach (var key in readDict.Keys)
        {
            sw.Write(key);
            sw.Write("      ");
            sw.Write(readDict[key]);
            sw.Write("\n");
        }
        sw.Close();
        fs.Close();
    }
    //截取相对路径  //
    public static void ChangeHead(string fullPath, Dictionary<string, string> theWriter)
    {
        int tmpCount = fullPath.IndexOf("Assets");
        int tmpLength = fullPath.Length;
        //Assets/AB/Scenes\Load
        string replacePath = fullPath.Substring(tmpCount, tmpLength - tmpCount);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_red) + replacePath);
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if (dir != null)
        {
            ListFiles(dir, replacePath, theWriter);
        }
        else
        {
            Debug.Log("This path is no exit!");
        }
    }
    
    //遍历场景中的每一个功能文件夹
    public static void ListFiles(DirectoryInfo info, string replacePath, Dictionary<string, string> theWriter)
    {
        if (!info.Exists)
        {
            Debug.Log("The Path is not exit!");
            return;
        }
        DirectoryInfo dir=info as DirectoryInfo;
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i <files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;

            if (file != null)
            {
                ChangeMark(file, replacePath, theWriter);
            }
            else
            {
                ListFiles((DirectoryInfo)files[i], replacePath, theWriter);
            }
        }
    }
    public static void ChangeMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> theWriter)
    {
        if (tmpFile.Extension == ".meta")
        {
            return;
        }
        //AB\Scenes\LoadTwo
        string markStr = GetBundlePath(tmpFile, replacePath);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_red) + markStr);
        ChangeAssetMark(tmpFile, markStr, theWriter);
    }
    public static string GetBundlePath(FileInfo file, string replacePath)
    {
        string tmpPath = file.FullName;
        //E:\UnityApplication\UniversalFrame\Assets\AB\Scenes\Load\ABC.prefab
        tmpPath = FixedWindowPath(tmpPath);
        //Assets\AB\Scenes\Load
        replacePath = FixedWindowPath(replacePath);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_green) + tmpPath);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_red) + replacePath);
        int assetCount = tmpPath.IndexOf(replacePath);
        assetCount += replacePath.Length + 1;
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_blue) + file.Name);
        int nameCount = tmpPath.LastIndexOf(file.Name);
        int tmpCount = replacePath.IndexOf("\\");
        //AB\Scenes\LoadTwo
        string sceneHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_main) + sceneHead);
        int tmpLength = nameCount - assetCount;
        if (tmpLength > 0)
        {
            string subString = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
            Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_green) + subString);
            string[] result = subString.Split("/".ToCharArray());
            return sceneHead + "/" + result[0];
        }
        else
            return sceneHead;
    }
    public static void ChangeAssetMark(FileInfo tmpFile, string markStr, Dictionary<string, string> theWriter)
    {
        string fullPath = tmpFile.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        //Assets\AB\Scenes\LoadTwo\BBB.prefab
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_yellow) + assetPath);
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);

        //以下为标记
        //AB\Scenes\LoadTwo
        importer.assetBundleName = markStr;
        Debug.Log(CDebugSet.SetTextColor("AssetBundleTools", CDebugSet.color_yellow) + markStr);
        if (tmpFile.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        }
        else
        {
            importer.assetBundleVariant = "ld";
        }
        string modelName="";
        string[] subMark=markStr.Split("\\".ToCharArray());
        if (subMark.Length > 1)
        {
            modelName = subMark[subMark.Length-1];
        }
        else
        {
            modelName = markStr;
        }
        string modelPath = markStr.ToLower() + "." + importer.assetBundleVariant;
        if (!theWriter.ContainsKey(modelName))
        {
            theWriter.Add(modelName, modelPath);
        }
    }
    public static string FixedWindowPath(string path)
    {
        path = path.Replace("/", "\\");
        return path;
    }
}
