using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public Monster_Script monster_Script;

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            monster_Script.Search();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            monster_Script.hit=false;
        }
    }
}
