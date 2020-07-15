using UnityEngine;
using System.Collections;

public class Vector
{
    public string m_name;
    public float m_a;
    public float m_b;
    public Vector(string name,float a,float b)
    {
        m_name = name;
        m_a = a;
        m_b = b;
    }
    //public static bool operator ==(Vector lhs, Vector rhs)
    //{
    //    if (lhs.m_name == rhs.m_name&&lhs.m_a==rhs.m_a&&lhs.m_b==rhs.m_b)
    //        return true;
    //    else
    //        return false;
    //}
}

public class ComputeTest : MonoBehaviour 
{
    Vector tmpVectorA = new Vector("Hello",1,2);
    Vector tmpVectorB = new Vector("Hello",1,2);

    string tmpA = new string(new char[] { 'H','e','l','l','o'});
    string tmpB = new string(new char[] { 'H','e','l','l','o'});

	void Start () 
    {
        object objA = tmpA;
        object objB = tmpB;
        Debug.Log("tmpA == tmpA:" + (tmpA == tmpB));//true
        Debug.Log("tmpA.Equals(tmpB):" + tmpA.Equals(tmpB));//true

        Debug.Log("\nobjA == objB:" + (objA == objB));//false
        Debug.Log("objA.Equals(objB):" + objA.Equals(objB));//true

        Debug.Log("\ntmpVectorA == tmpVectorB:" +( tmpVectorA == tmpVectorB));//false
        Debug.Log("tmpVectorA.Equals(tmpVectorB):" + tmpVectorA.Equals(tmpVectorB));//false
	}
}
