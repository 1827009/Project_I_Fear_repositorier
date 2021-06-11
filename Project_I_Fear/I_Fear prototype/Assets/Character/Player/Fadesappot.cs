using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadesappot : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerScript p;
    public GameObject parent;
    public GameObject HP;
    public FadeImage fadeImage;

    GameObject ChildObject;
    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        ChildObject = parent.transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (p.Fade && 0.9f < fadeImage.Range)
        {
            ChildObject.SetActive(true);
            HP.SetActive(false);
        }
        else
        {
            ChildObject.SetActive(false);
            HP.SetActive(true);

        }
    }
}
