using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeVer : MonoBehaviour
{
   PlayerScript p;
    GameObject ChildObject;
     public FadeImage fadeImage;
    // Start is called before the first frame update
    void Start()
    {
        ChildObject = transform.GetChild(0).gameObject;
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fadeImage = p.transform.Find("FadeCanvas").GetComponent<FadeImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p.Fade&&0.9f<fadeImage.Range)
        {
            ChildObject.SetActive(false);
        }
        else
        {
            ChildObject.SetActive(true);
        }
    }
}
