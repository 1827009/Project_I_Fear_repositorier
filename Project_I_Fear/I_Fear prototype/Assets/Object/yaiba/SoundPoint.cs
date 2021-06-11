using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoint : MonoBehaviour
{
    // Start is called before the first frame update
   public bool SoundON = false;

    // Update is called once per frame
    private void OnTriggerStay(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            SoundON = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            SoundON = false ;
        }
    }
}
