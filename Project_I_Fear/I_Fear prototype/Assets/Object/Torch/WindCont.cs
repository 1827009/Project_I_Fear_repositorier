using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCont : MonoBehaviour
{
    // Start is called before the first frame update
    public float count=0.5f;
    float time = 0;
    GameObject ChildObject;
    bool hit = false;
    void Start()
    {
        ChildObject = transform.GetChild(3).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            if (time == 0 || Time.time - time > count)
            {
                time = Time.time;
                Vector3 R = transform.eulerAngles;
                switch (transform.localEulerAngles.y)
                {
                    case 0:
                        R.y = 180;
                        break;
                    case 180:
                        R.y = 0;
                        break;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            hit = true;
            ChildObject.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            hit = false;
            ChildObject.SetActive(false);
        }
    }
}
