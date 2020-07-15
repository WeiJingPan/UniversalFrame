using System;
using System.Collections.Generic;
using UnityEngine;

public class TeacherCallBackNode
{
    public string resName;
    public string bundleName;
    public string sceneName;
    public bool isSingle;
    public TeacherCallBackNode nextValue;
    public CallLuaFunction luaFunc;

    public TeacherCallBackNode(string tmpRes,string tmpBundle,string sceneName,CallLuaFunction tmpLua,bool single,TeacherCallBackNode tmpNode)
    {
        this.resName = tmpRes;
        this.bundleName = tmpBundle;
        this.sceneName = sceneName;
        this.luaFunc = tmpLua;
        this.isSingle = single;
        this.nextValue = tmpNode;
    }
    public void Dispose()
    {

    }
}
public class TeacherCallBackManager
{

    private static TeacherCallBackManager teacherManager = null;
    public static TeacherCallBackManager CallBackManager
    {
        get
        {
            if(teacherManager==null)
            {
                teacherManager = new TeacherCallBackManager();
            }
            return teacherManager;
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
    public void GetResources(string sceneName,string bundleName,string res,bool single,CallLuaFunction luaFunc)
    {
        if (!ILoaderManager.Instance.IsLoadBundleFinish(sceneName, bundleName))
        {
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, LoaderProgrecess);
            string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                TeacherCallBackNode tmpNode = new TeacherCallBackNode(sceneName,bundleName,res,luaFunc,single,null);
                CallBack.AddBundleCallBack(bundleFullName, tmpNode);
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
                object tmpObj = ILoaderManager.Instance.GetSingleResources(sceneName, bundleName, res);
                luaFunc.Call(sceneName, bundleName, res, tmpObj);
            }
            else
            {
                object[] tmpObj = ILoaderManager.Instance.GetMultiResources(sceneName, bundleName, res);
                luaFunc.Call(sceneName, bundleName, res, tmpObj);
            }
        }
        else
        {
            //已经加载但是没有完成
            string bundleFullName = ILoaderManager.Instance.GetBundleRetateName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                TeacherCallBackNode tmpNode = new TeacherCallBackNode(sceneName, bundleName, res, luaFunc, single, null);
                CallBack.AddBundleCallBack(bundleName, tmpNode);
            }
            else
                Debug.LogWarning("Do not contain bundle ==" + bundleName);
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

    Dictionary<string, TeacherCallBackNode> manager=null;
    private string sceneName;
    private string bundleName;
    private string res;
    private CallLuaFunction luaFunc;
    private bool single;
    private object p;

    public TeacherCallBackManager()
    {
        manager = new Dictionary<string, TeacherCallBackNode>();
    }

    public TeacherCallBackManager(string sceneName, string bundleName, string res, CallLuaFunction luaFunc, bool single, object p)
    {
        this.sceneName = sceneName;
        this.bundleName = bundleName;
        this.res = res;
        this.luaFunc = luaFunc;
        this.single = single;
        this.p = p;
    }

    public void AddBundleCallBack(string bundle,TeacherCallBackNode tmpNode)
    {
        if (manager.ContainsKey(bundle))
        {
            TeacherCallBackNode tmpAddNode = manager[bundle];
            while (tmpAddNode.nextValue != null)
            {
                tmpNode = tmpNode.nextValue;
            }
            tmpNode.nextValue = tmpNode;
        }
        else
        {
            manager.Add(bundle, tmpNode);
        }
    }
    public void Dispose(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            TeacherCallBackNode tmpNode = manager[bundle];
            while (tmpNode.nextValue != null)
            {
                TeacherCallBackNode curNode = tmpNode;
                tmpNode = tmpNode.nextValue;
                curNode.Dispose();
            }
            tmpNode.Dispose();
            manager.Remove(bundle);
        }
    }
    public void CallBackLua(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            TeacherCallBackNode tmpNode = manager[bundle];
            do
            {
                if (tmpNode.isSingle)
                {
                    object tmpObj = ILoaderManager.Instance.GetSingleResources(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
                    tmpNode.luaFunc.Call(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName, tmpObj);
                }
                else
                {
                    object[] tmpObj = ILoaderManager.Instance.GetMultiResources(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName);
                    tmpNode.luaFunc.Call(tmpNode.sceneName, tmpNode.bundleName, tmpNode.resName, tmpObj);
                }
                tmpNode = tmpNode.nextValue;
            }
            while (tmpNode != null);
        }
    }
}
public class LuaLoadReses
{
    private static LuaLoadReses instance = null;
    public static LuaLoadReses Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaLoadReses();
            }
            return instance;
        }
    }
}
class LLuaResLoader
{
       
}


