using UnityEngine;
using System.Collections;

public class ChangeEquip : MonoBehaviour
{
    //要被换的物体
    public Transform idPlayer;
    //备胎需要替换的物体
    public Transform idPartPlayer;

    public void ChangeEquipment(Transform boneObj,Transform rootObj)
    {
        SkinnedMeshRenderer[] m_skinMeshRenders = boneObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var tmpRender in m_skinMeshRenders)
        {
            ProcessMeshRender(tmpRender, rootObj);
        }
    }
    /// <summary>
    /// 重新new一个skinnedMeshRender然后找骨骼替换
    /// </summary>
    /// <param name="thisRender"></param>
    /// <param name="rootObj"></param>
    private void ProcessMeshRender(SkinnedMeshRenderer thisRender,Transform rootObj)
    {
        GameObject newObj = new GameObject(thisRender.gameObject.name);
        newObj.transform.parent = rootObj.transform;
        SkinnedMeshRenderer newRender = newObj.AddComponent<SkinnedMeshRenderer>();
        Transform[] myBones = new Transform[thisRender.bones.Length];
        for (int i = 0; i < thisRender.bones.Length; i++)
        {
            myBones[i] = FindChildByName(thisRender.bones[i].name, rootObj);
        }
        newRender.rootBone = rootObj;
        newRender.bones = myBones;
        newRender.sharedMaterial = thisRender.sharedMaterial;
        newRender.material = thisRender.material;
    }
	
    private Transform FindChildByName(string thisName,Transform thisObj)
    {
        Transform resultObj=null;
        if (thisObj.name == thisName)
        {
            return thisObj.transform;
        }
        for (int i = 0; i < thisObj.childCount; i++)
        {
            resultObj = FindChildByName(thisName, thisObj.GetChild(i));
            if (resultObj!=null)
            {
                return resultObj;
            }
        }
        return resultObj;
    }
}
