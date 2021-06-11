using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaikSound : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 oldpostion = Vector3.zero;
    Vector3 oldPlayerPostion;
    GameObject player;
    GameObject monster;
    PlayerScript player_sclipt;
    Monster_Script monster_script;
    AudioSource audioSource;
    AudioClip Nomal;
    AudioClip Verbell;

    bool sound = false;
    void Start()
    {
        monster = transform.root.gameObject;
        monster_script = monster.GetComponent<Monster_Script>();
        player = monster_script.player.gameObject;
        audioSource = GetComponent<AudioSource>();
        player_sclipt = monster_script.player.GetComponent<PlayerScript>();
        oldPlayerPostion = player_sclipt.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SoundCheng();

        if (oldpostion != Vector3.zero)
        {
            if (player_sclipt.Fade)
            {
                if (transform.position == oldpostion || !playerMove() )
                {
                    if (monster_script.Taget()!=player)
                    {
                        monster.transform.position = oldpostion;
                    }

                }
            }
            else if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (monster_script.Dead)
        {
            audioSource.Stop();
        }

        oldPlayerPostion = player_sclipt.transform.position;
        oldpostion = transform.position;

    }

    bool playerMove()
    {
        if (player_sclipt.transform.position == oldPlayerPostion)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void  SoundCheng()
    {

        if (player_sclipt.OnBell && player_sclipt.Fade)
        {
            if (!sound)
            {
                if (Verbell != null)
                {
                    audioSource.clip = Verbell;
                    audioSource.Play();
                }
                sound = true;
            }
        }
        if (!player_sclipt.Fade && monster_script.target == player)
        {
            audioSource.clip = Nomal;
            audioSource.Play();
        }

        }
        private void OnTriggerEnter(Collider other)
    {
        if (monster != null)
        {
            if (player_sclipt.Fade)
            {
                if (other.transform.tag == "Player")
                {
                    audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            audioSource.Stop();
        }
    }
}
