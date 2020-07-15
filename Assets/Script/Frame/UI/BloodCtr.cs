using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodCtr: MonoBehaviour
{
    Image blood;
    void Awake()
    {
        UIManager.Instance.RegistGameObject(name, gameObject);
    }
    private void Start()
    {
        blood = transform.GetComponent<Image>();
    }
    public void SetBlood(float tmpBlood)
    {
        blood.fillAmount =Mathf.Clamp01(tmpBlood);
    }
}
