using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIBehaviour : MonoBehaviour
{
    void Awake()
    {
        UIManager.Instance.RegistGameObject(name, gameObject);
    }
    public void AddButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.AddListener(action);
        }
    }
    public void RemoveButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.RemoveListener(action);
        }
    }
    public void AddButtonDownListener(UnityAction<BaseEventData> action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            
        }
    }
    public void AddButtonUpListener(UnityAction<BaseEventData> action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();

        }
    }
    public void AddToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            Toggle btn = transform.GetComponent<Toggle>();
            btn.onValueChanged.AddListener(action);
        }
    }
    public void RemoveToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            Toggle btn = transform.GetComponent<Toggle>();
            btn.onValueChanged.RemoveListener(action);
        }
    }
    public void AddSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            Slider btn = transform.GetComponent<Slider>();
            btn.onValueChanged.AddListener(action);
        }
    }
    public void RemoveSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            Slider btn = transform.GetComponent<Slider>();
            btn.onValueChanged.RemoveListener(action);
        }
    }
    public void AddInputListener(UnityAction<string> action)
    {
        if (action != null)
        {
            InputField btn = transform.GetComponent<InputField>();
            btn.onValueChanged.AddListener(action);
        }
    }
}
