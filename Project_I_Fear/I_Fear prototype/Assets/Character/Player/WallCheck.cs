using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallCheck : MonoBehaviour
{
    
    AudioSource audioSource=null;
    public AudioClip audioClip;
    GameObject player;
    PlayerScript player_sclipt;
    void Start()
    {
        player = transform.root.gameObject;
        player_sclipt = player.GetComponent<PlayerScript>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame



   

    private void OnTriggerStay(Collider other)
    {
        if (player_sclipt.Fade)
        {
            if (other.transform.tag == "Wall")
            {
                if (0<OVRInput.RawAxis2D.LThumbstick)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClip, PlayerScript.FadeVolume);
                    }
                }
            }
        }
    }



}
