using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MesssageFadeOut : MonoBehaviour
{
    [SerializeField] float displayTime = 3;
    float imagrAlpha=0;
    Image image;
    bool fadeInDone;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!fadeInDone)
        {
            imagrAlpha += Time.deltaTime;
            if (imagrAlpha > displayTime * 0.5f)
                fadeInDone = true;
        }
        else
        {
            imagrAlpha -= Time.deltaTime;
        }

        image.color = new Color(1, 1, 1, imagrAlpha);
        if(imagrAlpha < 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
