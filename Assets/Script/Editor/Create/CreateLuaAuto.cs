using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System.Text;

public class CreateLuaAuto : MonoBehaviour
{
    [MenuItem("Assets/Create/U3DEventFrame Lua Script", false, 80)]
    public static void CreateNewLua()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateLuaScriptAsset>(),
            GetSelectedPathOrFallBack() + "New Lua.cs",
            null, "Assets/Script/Editor/Create/LuaClass.cs");
    }
    [MenuItem("Assets/Create/U3DEventFrame C# Script", false, 70)]
    public static void CreateEventCS()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateEventCSSCriptAsset>(),
            GetSelectedPathOrFallBack() + "New Script.cs",
            null, "Assets/Script/Editor/Create/EventCSClass.cs");
    }

    private static string GetSelectedPathOrFallBack()
    {
        string path = "Assets";
        foreach (UnityEngine.Object item in Selection.GetFiltered(typeof(UnityEngine.Object),SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(item);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
class CreateLuaScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }

    private UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

        Debug.Log("pathName ==" + pathName);

        text = Regex.Replace(text, "LuaClass", fileNameWithoutExtension);

        bool encoderShouldEmitUTF8Identifier = true;
        bool ThrowOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, ThrowOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}
class CreateEventCSSCriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }

    private UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

        Debug.Log("pathName ==" + pathName);

        text = Regex.Replace(text, "EventCSClass", fileNameWithoutExtension);

        bool encoderShouldEmitUTF8Identifier = true;
        bool ThrowOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, ThrowOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}
