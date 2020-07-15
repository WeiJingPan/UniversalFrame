using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillTime : MonoBehaviour
{
    Image maskImage;
    bool isPressed = false;
    float timeCount = 0;

	// Use this for initialization
	void Start ()
    {
        maskImage = transform.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isPressed)
        {
            timeCount += Time.deltaTime;
            maskImage.fillAmount = timeCount / 3.0f;
            if (timeCount > 3.0f)
            {
                timeCount = 0;
                isPressed = false;
                maskImage.fillAmount = 0;
            }
        }
	}
    public void ButtonPressed()
    {
        if (timeCount <= 0)
        {
            isPressed = true;
        }
    }
}
