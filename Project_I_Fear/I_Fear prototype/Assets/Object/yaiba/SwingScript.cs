using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 1;
    public AudioClip Blade_SE;
    public AudioClip AttackSE;
    public float NoDamegeTime = 2;

    GameObject player;

    SoundPoint SP;
    GameObject ChildObject;
    [SerializeField, TooltipAttribute("角度")] public float Swing = 45;
    private Animator animator;
    GameObject AttackAnimetion;
    PlayerScript playerS;

    Vector3 pos;
    AudioSource audioSource;
    float AttackTime = 0;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerS = player.GetComponent<PlayerScript>();

        ChildObject = transform.GetChild(1).gameObject;
        SP = ChildObject.GetComponent<SoundPoint>();
        audioSource = this.GetComponent<AudioSource>();
        Vector3 R = transform.eulerAngles;
        R.z =Randam();
        transform.eulerAngles = R;
        AttackAnimetion = player.transform.GetChild(5).gameObject;
        animator = AttackAnimetion.GetComponent<Animator>();
        animator.SetBool("Attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Swing+10)>transform.localEulerAngles.z && transform.localEulerAngles.z > Swing)
        {
            Vector3 R = transform.eulerAngles;
            R.z = Swing;
            transform.eulerAngles = R;
            speed *= -1;
        }
        if(360 - (Swing + 10) < transform.localEulerAngles.z && transform.localEulerAngles.z < 360 - Swing)
        {
            Vector3 R = transform.eulerAngles;
            R.z = 360 - Swing;
            transform.eulerAngles = R;
            speed *= -1;
        }

        if (playerS.Fade&&SP.SoundON)
        {
            if (-360 < transform.localEulerAngles.z && transform.localEulerAngles.z < 1)
            {
                audioSource.PlayOneShot(Blade_SE, PlayerScript.FadeVolume);
            }
        }
        transform.Rotate(new Vector3(0, 0,  (speed*10)*Time.deltaTime));
    }
    float Randam()
    {
        float a,i=0;
        a = Random.Range(0, 2);
        if (a < 1) {
            i = Random.Range(0, 69);
        }
        if (1 < a) { 
            i = Random.Range(360 - Swing, 360); 
        }

        speed = Random.Range(3, 10);
        return i;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            if (Time.time - AttackTime > NoDamegeTime)
            {
                
                if (playerS.Fade)
                {
                    audioSource.PlayOneShot(AttackSE, PlayerScript.FadeVolume);
                }
                else
                {
                    if (!AttackAnimetion.activeSelf)
                    {
                        AttackAnimetion.SetActive(true);
                       
                    }
                    else
                    {
                       
                        animator.SetBool("Attack", true);

                    }
                }
                PlayerScript player = collider.GetComponent<PlayerScript>();
                player.attackEntityFrom(1);
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