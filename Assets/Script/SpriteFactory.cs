using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteFactory : MonoBehaviour 
{
    object[] m_sprite_list;
    int count = 1;

    void Start()
    {
        m_sprite_list = Resources.LoadAll("Number");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = new GameObject(count.ToString());
            go.transform.SetParent(transform);
            Image image=go.AddComponent<Image>();
            image.sprite = m_sprite_list[count%9] as Sprite;
            image.SetNativeSize();
            go.transform.localPosition = new Vector3(50 * (count-1), 50 * (count-1));
            count++;
        }
    }
}
