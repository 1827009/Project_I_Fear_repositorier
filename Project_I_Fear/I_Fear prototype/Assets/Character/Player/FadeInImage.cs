using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeSpeed=1;
    [SerializeField] float wait;
    float alpha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wait -= Time.deltaTime;
        if (wait <= 0 && image.color.a < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    }
}
