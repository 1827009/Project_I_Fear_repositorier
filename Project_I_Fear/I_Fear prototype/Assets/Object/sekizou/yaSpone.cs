using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yaSpone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] ya;
    [SerializeField] float appearTime;


    private float nowTime;
    void Start()
    {
        nowTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime >= appearTime)
        {
            nowTime = 0;

            appearYa();
        }
    }

    void appearYa()
    {
        var rondomValue = Random.Range(0, ya.Length);
        Vector3 copy = transform.position;
        copy.y += 0.2f;
        Vector3 r = transform.localEulerAngles;
        GameObject.Instantiate(ya[rondomValue], copy,Quaternion.Euler(90f,r.y,0f));

        
        nowTime = 0f;
    }
}
