using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUN : MonoBehaviour
{
    Monster_Script Monster;
    // Start is called before the first frame update
    void Start()
    {
        Monster = GetComponent<Monster_Script>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Monster")
        {
            Monster = other.GetComponent<Monster_Script>();
            Monster.Dead = true;
           
        }

    }
}
