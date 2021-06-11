using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMessage : MonoBehaviour
{
    Image image;
    float alpha;
    const float displayTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime;
            image.color = new Color(1, 1, 1, alpha);
        }
        else
            image.enabled = false;
    }

    public void LogStartUp(Sprite sprite)
    {
        image.enabled = true;
        image.sprite = sprite;
        alpha = displayTime;
    }
}
