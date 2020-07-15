using UnityEngine;
using System.Collections;

public class Citizen
{
    public float m_tax;
    public Citizen(float tax)
    {
        m_tax = tax;
    }
    public virtual void GetCalculateTax()
    {
        Debug.Log("普通公民的税是：" + m_tax * 0.1);
    }
}
public class Person : Citizen
{
    public Person(float tax):base(tax)
    {
        m_tax = tax;
    }
    public override void GetCalculateTax()
    {
        Debug.Log("特殊个人的税是：" + m_tax * 0.2);
    }
}
public class Company : Citizen
{
    public Company(float tax):base(tax)
    {
        m_tax = tax;
    }
    public override void GetCalculateTax()
    {
        Debug.Log("公司人员的税是：" + m_tax * 0.3);
    }
}

public class Office
{
    public void GetYourTax(Citizen citizen)
    {
        citizen.GetCalculateTax();
    }
}

public class StrategyPattern : MonoBehaviour 
{
    Office office = new Office();
    Person person = new Person(500);
    Company company = new Company(500);
	void Start () 
    {
        office.GetYourTax(person);
        office.GetYourTax(company);
	}
}
