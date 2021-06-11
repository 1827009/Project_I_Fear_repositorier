using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioClip;
    public float damage = 1;
    public float NoDamegeTime = 2;

    GameObject AttackAnimetion;
    private Animator animator;
    private AudioSource audioSource;
    private SoundCheck sound;
    private GameObject child;
    float AttackTime = 0;
    

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        sound = GetComponent<SoundCheck>();
        child = transform.Find("Trap02").gameObject;

    }


        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (Time.time - AttackTime > NoDamegeTime)
            {
                
                PlayerScript player = other.GetComponent<PlayerScript>();
                if (!audioSource.isPlaying && player.Fade)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
                else
                {
                    AttackAnimetion = other.transform.GetChild(6).gameObject;
                    animator = AttackAnimetion.GetComponent<Animator>();
                    if (!AttackAnimetion.activeSelf)
                    {
                        AttackAnimetion.SetActive(true);
                        
                    }
                    else
                    {

                        animator.SetBool("Attack", true);

                    }

                }
                other.gameObject.GetComponent<PlayerScript>().attackEntityFrom(1);

                sound.SoundEnd(audioSource, () => {
                    //child.transform.localScale = transform.localScale;
                    child.SetActive(true);
                    this.gameObject.transform.DetachChildren();
                    Destroy(this.gameObject);
                });
                // StartCoroutine(Checking(() => {Destroy(gameObject);}) );
                AttackTime = Time.time;
            }

        }

    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            animator.SetBool("Attack", false);
        }
    }


}

