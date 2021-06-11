using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ya : MonoBehaviour
{
    // Start is called before the first frame update
    private float movepos;
    public float speed=1;
    public AudioClip audioClip;
    private Vector3 pos;
    GameObject AttackAnimetion;
    private Animator animator;
    [SerializeField] float DeleteTime=8;
    //public Player player_sclipt;
    public bool hit;
    private float nowTime;
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        movepos = 1f;
        pos = transform.position;
        nowTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            move();
        }

        if (hit)
        {
            nowTime += Time.deltaTime;
            rigidbody.useGravity = true;
            if (transform.position.y < 0)
            {
                rigidbody.useGravity = false;
                pos = transform.position;
                pos.y =0.002f;
                transform.position = pos;
            }
        }

        
        if (nowTime >= DeleteTime)
        {
            Destroy(gameObject);
        }
    }
    //移動
    void move()
    {
        pos = transform.position;
        Vector3 r = transform.localEulerAngles;
        switch (r.y)
        {
            case 0:
                pos.z += movepos * speed * Time.deltaTime;
                break;
            case 90:
                pos.x += movepos * speed * Time.deltaTime;
                break;
            case 270:
                pos.x -= movepos * speed * Time.deltaTime;
                break;
            case 180:
                pos.z -= movepos * speed * Time.deltaTime;
                break;
        }
        transform.position = pos;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            hit = true;
        }

        if(other.tag == "Player")
        {
            PlayerScript player_sclipt = other.GetComponent<PlayerScript>();
            if (player_sclipt.Fade)
            {
                audioSource.PlayOneShot(audioClip, PlayerScript.FadeVolume);
            }
            else
            {
                GameObject p = other.gameObject;
                AttackAnimetion = other.transform.GetChild(8).gameObject;
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
            player_sclipt.attackEntityFrom(1);
            Destroy(gameObject);
            
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
