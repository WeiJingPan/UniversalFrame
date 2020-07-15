using UnityEngine;
using System.Collections;


/// <summary>
/// 顶点动画螺旋
/// </summary>
public class MyTwist : MonoBehaviour
{
    Mesh TwistMesh;
    Vector3[] baseVertex;
    Vector3[] baseNormal;
    float twist;
    float inputSensitive = 1.5f;
    private void Start()
    {
        TwistMesh = GetComponent<MeshFilter>().mesh;
        baseVertex = TwistMesh.vertices;
        baseNormal = TwistMesh.normals;
    }
    public void ChangeMesh()
    {
        twist = Input.GetAxis("Horizontal") * inputSensitive * Time.deltaTime;

        TwistMesh = GetComponent<MeshFilter>().mesh;
        baseVertex = TwistMesh.vertices;
        baseNormal = TwistMesh.normals;

        Vector3[] vertexs = new Vector3[baseVertex.Length];
        Vector3[] normals = new Vector3[baseVertex.Length];
        for (int i = 0; i < vertexs.Length; i++)
        {
            vertexs[i] = DoTwist(baseVertex[i], baseVertex[i].y * twist);
            normals[i] = DoTwist(baseNormal[i], baseVertex[i].y * twist);
        }
        TwistMesh.vertices = vertexs;
        TwistMesh.normals = normals;
        TwistMesh.RecalculateBounds();
        TwistMesh.RecalculateNormals();
    }
    private Vector3 DoTwist(Vector3 pos,float t)
    {
        float st = Mathf.Sin(t);
        float ct = Mathf.Cos(t);
        Vector3 result = Vector3.zero;
        result.x = pos.x + ct - pos.z * st;
        result.y = pos.y;
        result.z = pos.x * st + pos.z * ct;
        return result;
    }
}
