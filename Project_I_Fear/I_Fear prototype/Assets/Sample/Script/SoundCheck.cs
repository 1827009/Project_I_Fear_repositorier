using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheck : MonoBehaviour
{
    // Start is called before the first frame update
   
    bool end = false;

    void Start()
    {
      
    }
    public void SoundEnd(AudioSource sound,functionType callback)
    {
        StartCoroutine(Checking(callback,sound));
    }
    public delegate void functionType();
    private IEnumerator Checking(functionType callback, AudioSource sound)
    {
        while (!end)
        {
            yield return new WaitForFixedUpdate();
            if (!sound.isPlaying&&!end)
            {
                callback();
                end = true;
                break;
            }
        }
    }
}
