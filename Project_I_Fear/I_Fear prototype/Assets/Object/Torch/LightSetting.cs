using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetting : MonoBehaviour
{
    // Start is called before the first frame update

    Light light;
    float max;
    public float speed = 1;
    void Start()
    {
        light = GetComponent<Light>();
        max=light.intensity;
        speed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (max < light.intensity||light.intensity<0)
        {
            speed *= -1;
        }

        light.intensity+= 1 * speed * Time.deltaTime;

    }
}
